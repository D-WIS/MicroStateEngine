using DWIS.Client.ReferenceImplementation;
using DWIS.Client.ReferenceImplementation.OPCFoundation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using DWIS.MicroState.Model;
using DWIS.API.DTO;

namespace DWIS.MicroState.SignalsGenerator.Test
{
    public class Worker : BackgroundService
    {
        private ILogger<DWISClientOPCF>? _loggerDWISClient;
        private ILogger<Worker>? _logger;
        private IOPCUADWISClient? DWISClient_ = null;
        private SignalGroup _signalGroup = new SignalGroup();
        private QueryResult? signalGroupPlaceHolder_ = null;
        private TimeSpan _loopSpan;

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
            if (Directory.Exists(homeDirectory))
            {
                string configName = homeDirectory + Path.DirectorySeparatorChar + "config.json";
                if (File.Exists(configName))
                {
                    string jsonContent = File.ReadAllText(configName);
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
        private void DefineMicroStateSemantic()
        {
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                if (_signalGroup != null)
                {
                    _signalGroup.RegisterToBlackboard(DWISClient_, ref signalGroupPlaceHolder_);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ConnectToBlackboard();
            DefineMicroStateSemantic();
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                
                if (_signalGroup != null && signalGroupPlaceHolder_ != null)
                {
                    await Loop(cancellationToken);
                }
            }
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            Random rnd = new Random(); 
            PeriodicTimer timer = new PeriodicTimer(_loopSpan);
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                if (DWISClient_ != null && _signalGroup != null && signalGroupPlaceHolder_ != null)
                {
                    _signalGroup.AxialVelocityTopOfString.Mean = rnd.NextDouble();
                    _signalGroup.AxialVelocityTopOfString.StandardDeviation = 0.01;
                    _signalGroup.RotationalVelocityTopOfString.Mean = rnd.NextDouble();
                    _signalGroup.RotationalVelocityTopOfString.StandardDeviation = 0.01;
                    _signalGroup.FlowTopOfString.Mean = rnd.NextDouble();
                    _signalGroup.FlowTopOfString.StandardDeviation = 0.00001;
                    // etc.
                    _logger?.LogInformation("Sending new signals");
                    _signalGroup.SendToBlackboard(DWISClient_, signalGroupPlaceHolder_);
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
