using DWIS.Client.ReferenceImplementation;
using DWIS.Client.ReferenceImplementation.OPCFoundation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using DWIS.MicroState.Model;

namespace DWIS.MicroState.StateReader.Test
{
    public class Worker : BackgroundService
    {
        private ILogger<DWISClientOPCF>? _loggerDWISClient;
        private ILogger<Worker>? _logger;
        private IOPCUADWISClient? DWISClient_ = null;
        private List<AcquiredSignals>? _acquiredDeterministicMicroStates = null;
        private List<AcquiredSignals>? _acquiredProbabilisticMicroStates = null;
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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ConnectToBlackboard();
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                Type type = typeof(MicroStates);
                Assembly assembly = type.Assembly;
                Dictionary<string, QuerySpecification>? queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName);
                if (queries != null && queries.Count > 0)
                {
                    if (_acquiredDeterministicMicroStates == null)
                    {
                        _acquiredDeterministicMicroStates = new List<AcquiredSignals>();
                    }
                    _acquiredDeterministicMicroStates.Clear();
                    foreach (var kvp in queries)
                    {
                        if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                        {
                            string sparql = kvp.Value.SparQL;
                            var result = DWISClient_.GetQueryResult(sparql);
                            if (result != null && result.Results != null && result.Results.Count > 0)
                            {
                                _acquiredDeterministicMicroStates.Add(AcquiredSignals.CreateWithSubscription(new string[] { kvp.Value.SparQL }, new string[] { kvp.Key }, 0, DWISClient_));
                            }
                        }
                    }
                }
                type = typeof(ProbabilisticMicroStates);
                assembly = type.Assembly;
                queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName);
                if (queries != null && queries.Count > 0)
                {
                    if (_acquiredProbabilisticMicroStates == null)
                    {
                        _acquiredProbabilisticMicroStates = new List<AcquiredSignals>();
                    }
                    _acquiredProbabilisticMicroStates.Clear();
                    foreach (var kvp in queries)
                    {
                        if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                        {
                            string sparql = kvp.Value.SparQL;
                            var result = DWISClient_.GetQueryResult(sparql);
                            if (result != null && result.Results != null && result.Results.Count > 0)
                            {
                                _acquiredProbabilisticMicroStates.Add(AcquiredSignals.CreateWithSubscription(new string[] { kvp.Value.SparQL }, new string[] { kvp.Key }, 0, DWISClient_));
                            }
                        }
                    }
                }
                if (_acquiredDeterministicMicroStates != null && _acquiredProbabilisticMicroStates != null)
                {
                    await Loop(cancellationToken);
                }
            }
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            PeriodicTimer timer = new PeriodicTimer(_loopSpan);
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                MicroStates? microState = null;
                try
                {
                    if (_acquiredDeterministicMicroStates != null && _acquiredDeterministicMicroStates.Count > 0)
                    {
                        foreach (var acquiredSignal in _acquiredDeterministicMicroStates)
                        {
                            if (acquiredSignal != null && acquiredSignal.Count > 0)
                            {
                                foreach (var kvp in acquiredSignal)
                                {
                                    if (kvp.Value != null && kvp.Value.Count > 0 && kvp.Value[0] != null)
                                    {
                                        string? jsonString = kvp.Value[0].GetValue<string>();
                                        if (!string.IsNullOrEmpty(jsonString))
                                        {
                                            microState = JsonSerializer.Deserialize<MicroStates>(jsonString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
                if (microState != null && _logger != null)
                {
                    _logger.LogInformation("Determinist microstate timestamp: " + microState.Value.TimeStampUTC.ToLocalTime().ToLongTimeString());
                }
                ProbabilisticMicroStates? probabilisticMicroState = null;
                try
                {
                    if (_acquiredProbabilisticMicroStates != null && _acquiredProbabilisticMicroStates.Count > 0)
                    {
                        foreach (var acquiredSignal in _acquiredProbabilisticMicroStates)
                        {
                            if (acquiredSignal != null && acquiredSignal.Count > 0)
                            {
                                foreach (var kvp in acquiredSignal)
                                {
                                    if (kvp.Value != null && kvp.Value.Count > 0 && kvp.Value[0] != null)
                                    {
                                        string? jsonString = kvp.Value[0].GetValue<string>();
                                        if (!string.IsNullOrEmpty(jsonString))
                                        {
                                            probabilisticMicroState = JsonSerializer.Deserialize<ProbabilisticMicroStates>(jsonString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
                if (probabilisticMicroState != null && _logger != null)
                {
                    _logger.LogInformation("Probabilistic microstate timestamp: " + probabilisticMicroState.TimeStampUTC.ToLocalTime().ToLongTimeString());
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
