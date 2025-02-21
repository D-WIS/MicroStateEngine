using DWIS.Client.ReferenceImplementation;
using DWIS.Client.ReferenceImplementation.OPCFoundation;
using System.Text.Json;
using DWIS.MicroState.Model;
using DWIS.API.DTO;
using System.Reflection;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.Common;

namespace DWIS.MicroState.ThresholdsServer
{
    public class Worker : BackgroundService
    {
        private ILogger<DWISClientOPCF>? _loggerDWISClient;
        private ILogger<Worker>? _logger;
        private IOPCUADWISClient? DWISClient_ = null;
        private Thresholds _thresholds = new Thresholds();
        private QueryResult? thresholdsPlaceHolder_ = null;
        private TimeSpan _loopSpan;
        private object _lock = new object();

        private Configuration Configuration { get; set; } = new Configuration();

        public Worker(ILogger<Worker>? logger, ILogger<DWISClientOPCF>? loggerDWISClient)
        {
            _logger = logger;
            _loggerDWISClient = loggerDWISClient;
            Initialize();
        }

        private void Initialize()
        {
            string homeDirectory = ".." + Path.DirectorySeparatorChar + "home";
            if (!Directory.Exists(homeDirectory))
            {
                try
                {
                    Directory.CreateDirectory(homeDirectory);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Impossible to create home directory for local storage");
                }
            }
            lock (_lock)
            {
                AcquireConfiguration();
            }           
            if (_logger != null)
            {
                _logger.LogInformation("Configuration Loop Duration: " + Configuration.LoopDuration.ToString());
                _logger.LogInformation("Configuration OPCUAURAL: " + Configuration.OPCUAURL);
            }
            string hostName = System.Net.Dns.GetHostName();
            if (!string.IsNullOrEmpty(hostName))
            {
                var ip = System.Net.Dns.GetHostEntry(hostName);
                if (ip != null && ip.AddressList != null && ip.AddressList.Length > 0 && _logger != null)
                {
                    _logger.LogInformation("My IP Address: " + ip.AddressList[0].ToString());
                }
            }
        }
        private void AcquireConfiguration()
        {
            string homeDirectory = ".." + Path.DirectorySeparatorChar + "home";
            if (Directory.Exists(homeDirectory))
            {
                string configName = homeDirectory + Path.DirectorySeparatorChar + "config.json";
                if (File.Exists(configName))
                {
                    string jsonContent = string.Empty;
                    try
                    {
                        jsonContent = File.ReadAllText(configName);
                    }
                    catch (Exception e)
                    {
                        if (_logger != null)
                        {
                            _logger.LogError(e.ToString());
                        }
                    }
                    if (!string.IsNullOrEmpty(jsonContent))
                    {
                        try
                        {
                            Configuration? config = JsonSerializer.Deserialize<Configuration>(jsonContent);
                            if (config != null)
                            {
                                Configuration = config;
                            }
                        }
                        catch (Exception e)
                        {
                            if (_logger != null)
                            {
                                _logger.LogError(e.ToString());
                            }
                        }
                    }
                }
                else
                {
                    string defaultConfigJson = JsonSerializer.Serialize(Configuration);
                    using (StreamWriter writer = new StreamWriter(configName))
                    {
                        writer.WriteLine(defaultConfigJson);
                    }
                }
            }
        }
        private void DefineMicroStateSemantic()
        {
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                if (_thresholds != null)
                {
                    _thresholds.RegisterToBlackboard(DWISClient_, ref thresholdsPlaceHolder_);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ConnectToBlackboard();
            DefineMicroStateSemantic();
            if (DWISClient_ != null && DWISClient_.Connected)
            {

                if (_thresholds != null && thresholdsPlaceHolder_ != null)
                {
                    await Loop(cancellationToken);
                }
            }
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            Random rnd = new Random();
            PeriodicTimer timer = new PeriodicTimer(_loopSpan);
            PropertyInfo[] thresholdsProperties = typeof(Thresholds).GetProperties();
            PropertyInfo[] configurationProperties = typeof(Configuration).GetProperties();
            FieldInfo[] configurationStaticFields = typeof(Configuration).GetFields(BindingFlags.Static | BindingFlags.Public);
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                if (DWISClient_ != null && _thresholds != null && thresholdsPlaceHolder_ != null)
                {
                    lock (_lock)
                    {
                        AcquireConfiguration();
                    }
                    bool changed = false;
                    foreach (var property in thresholdsProperties)
                    {
                        if (property != null && property.PropertyType == typeof(ScalarDrillingProperty))
                        {
                            object? thresholdValue = property.GetValue(_thresholds);
                            if (thresholdValue != null && thresholdValue is ScalarDrillingProperty scalarDrillProp)
                            {
                                double? dvalue1 = scalarDrillProp.ScalarValue;
                                PropertyInfo? configurationProperty = null;
                                foreach (var confProp in configurationProperties)
                                {
                                    if (confProp != null && confProp.Name == property.Name)
                                    {
                                        configurationProperty = confProp;
                                        break;
                                    }
                                }
                                bool managed = false;
                                if (configurationProperty != null && configurationProperty.PropertyType == typeof(double))
                                {
                                    lock (_lock)
                                    {
                                        object? configurationValue = configurationProperty.GetValue(Configuration);
                                        if (configurationValue != null && configurationValue is double dvalue2)
                                        {
                                            managed = true;
                                            if (!Numeric.EQ(dvalue1, dvalue2))
                                            {
                                                scalarDrillProp.ScalarValue = dvalue2;
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                                if (!managed)
                                { 
                                    FieldInfo? configurationField = null;
                                    foreach (var confField in configurationStaticFields)
                                    {
                                        if (confField != null && confField.Name  == property.Name + "Default")
                                        {
                                            configurationField = confField;
                                            break;
                                        }
                                    }
                                    if (configurationField != null && configurationField.FieldType == typeof(double))
                                    {
                                        lock (_lock)
                                        {
                                            object? configurationValue = configurationField.GetValue(null);
                                            if (configurationValue != null && configurationValue is double dvalue2)
                                            {
                                                if (!Numeric.EQ(dvalue1, dvalue2))
                                                {
                                                    scalarDrillProp.ScalarValue = dvalue2;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (changed)
                    {
                        _thresholds.SendToBlackboard(DWISClient_, thresholdsPlaceHolder_);
                        _logger?.LogInformation("Sent new thresholds");
                    }
                }
            }
        }

        private void ConnectToBlackboard()
        {
            try
            {
                if (Configuration != null && !string.IsNullOrEmpty(Configuration.OPCUAURL))
                {
                    DefaultDWISClientConfiguration defaultDWISClientConfiguration = new DefaultDWISClientConfiguration();
                    defaultDWISClientConfiguration.UseWebAPI = false;
                    defaultDWISClientConfiguration.ServerAddress = Configuration.OPCUAURL;
                    _loopSpan = Configuration.LoopDuration;
                    DWISClient_ = new DWISClientOPCF(defaultDWISClientConfiguration, _loggerDWISClient);
                }
            }
            catch (Exception e)
            {
                if (_logger != null)
                {
                    _logger.LogError(e.ToString());
                }
            }
        }
    }
}
