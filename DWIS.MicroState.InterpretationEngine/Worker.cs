using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.General.Common;
using DWIS.Client.ReferenceImplementation;
using DWIS.API.DTO;
using DWIS.SPARQL.Utils;
using DWIS.Vocabulary.Schemas;
using System.Reflection;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace DWIS.MicroState.InterpretationEngine
{
    public class Worker : BackgroundService
    {
        private Configuration Configuration { get; set; } = new Configuration();
        private readonly ILogger<Worker> _logger;
        private IOPCUADWISClient? DWISClient_ = null;

        private MicroStates currentDeterministicMicroStates_ = new MicroStates();
        private QueryResult? deterministicMicroStatePlaceHolder_ = null;

        private ProbabilisticMicroStates currentProbabilisticMicroStates_ = new ProbabilisticMicroStates();
        private Dictionary<string, QueryResult>? probabilisticMicroStatePlaceHolders_ = null;

        private Thresholds microStateThresholds_ = new Thresholds();
        private Dictionary<string, List<AcquiredSignals>> microStateThresholdsPlaceHolders_ = new Dictionary<string, List<AcquiredSignals>>();

        private SignalGroup microStateSignalInputs_ = new SignalGroup();
        private Dictionary<string, List<AcquiredSignals>> microStateSignalPlaceHolders_ = new Dictionary<string, List<AcquiredSignals>>();

        private object lock_ = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            Initialize();
        }

        private void ConnectToDDHub()
        {
            try
            {
                if (Configuration != null && !string.IsNullOrEmpty(Configuration.OPCUAURL))
                {
                    DefaultDWISClientConfiguration defaultDWISClientConfiguration = new DefaultDWISClientConfiguration();
                    defaultDWISClientConfiguration.UseWebAPI = false;
                    defaultDWISClientConfiguration.ServerAddress = Configuration.OPCUAURL; // "opc.tcp://localhost:48030";
                    DWISClient_ = null; // new DWISClient(defaultDWISClientConfiguration, new UAApplicationConfiguration(), null, null, new DWIS.OPCUA.UALicenseManager.LicenseManager());
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

        private void DefineMicroStateSemantic()
        {
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                if (currentProbabilisticMicroStates_ != null)
                {
                    currentProbabilisticMicroStates_.RegisterToDDHub(DWISClient_, probabilisticMicroStatePlaceHolders_ ?? new Dictionary<string, QueryResult>());
                }
                currentDeterministicMicroStates_.RegisterToDDHub(DWISClient_, deterministicMicroStatePlaceHolder_ ?? new QueryResult());
            }
        }
        private void AcquireMicroStatesThresholds()
        {
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                microStateThresholds_.RegisterToDDHub(DWISClient_, microStateThresholdsPlaceHolders_);
            }
        }

        private void AcquireSignalInputs()
        {
            if (DWISClient_ != null && DWISClient_.Connected)
            {
                microStateSignalInputs_.RegisterToDDHub(DWISClient_, microStateSignalPlaceHolders_);
            }
        }
        private void RefreshThresholds()
        {
            if (microStateThresholds_ != null && microStateThresholdsPlaceHolders_ != null)
            {
                foreach (var kvp in microStateThresholdsPlaceHolders_)
                {
                    if (!string.IsNullOrEmpty(kvp.Key) && kvp.Value != null && kvp.Value.Count > 0)
                    {
                        Type type = microStateThresholdsPlaceHolders_.GetType();
                        PropertyInfo? propertyInfo = type.GetProperty(kvp.Key);
                        if (propertyInfo != null)
                        {
                            object? obj = propertyInfo.GetValue(microStateThresholds_);
                            if (obj is not null and DrillingProperty drillingProperty)
                            {
                                if (drillingProperty is GaussianDrillingProperty gaussianDrillingProperty)
                                {
                                    double defaultStandardDeviation = 1e-6;
                                    List<byte>? sensorOptions = null;
                                    List<byte>? fullScaleOptions = null;
                                    var optionalFacts = propertyInfo.GetCustomAttributes<OptionalFactAttribute>();
                                    var semanticSensor = propertyInfo.GetCustomAttribute<SemanticSensorVariableAttribute>();
                                    var semanticFullScale = propertyInfo.GetCustomAttribute<SemanticFullScaleVariableAttribute>();
                                    var semanticGaussian = propertyInfo.GetCustomAttribute<SemanticGaussianVariableAttribute>();
                                    if (semanticGaussian != null && semanticGaussian.DefaultStandardDeviation != null)
                                    {
                                        defaultStandardDeviation = semanticGaussian.DefaultStandardDeviation.Value;
                                    }
                                    if (optionalFacts != null && semanticSensor != null && !string.IsNullOrEmpty(semanticSensor.PrecisionVariable) && !string.IsNullOrEmpty(semanticSensor.AccuracyVariable))
                                    {
                                        List<byte> options = [];
                                        foreach (var option in optionalFacts)
                                        {
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticSensor.PrecisionVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticSensor.PrecisionVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticSensor.AccuracyVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticSensor.AccuracyVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                        }
                                        if (options.Count > 0)
                                        {
                                            sensorOptions = options;
                                        }
                                    }
                                    if (optionalFacts != null && semanticFullScale != null && !string.IsNullOrEmpty(semanticFullScale.FullScaleVariable) && !string.IsNullOrEmpty(semanticFullScale.ProportionErrorVariable))
                                    {
                                        List<byte> options = [];
                                        foreach (var option in optionalFacts)
                                        {
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticFullScale.FullScaleVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticFullScale.FullScaleVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticFullScale.ProportionErrorVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticFullScale.ProportionErrorVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                        }
                                        if (options.Count > 0)
                                        {
                                            fullScaleOptions = options;
                                        }
                                    }
                                    gaussianDrillingProperty.FuseData(kvp.Value, defaultStandardDeviation, sensorOptions, fullScaleOptions);
                                }
                                else
                                {
                                    drillingProperty.FuseData(kvp.Value);
                                }
                            }
                        }
                    }
                }
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectToDDHub();
            DefineMicroStateSemantic();
            AcquireMicroStatesThresholds();
            AcquireSignalInputs();
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime d1 = DateTime.UtcNow;
                RefreshThresholds();
                RefreshSignals();
                DateTime d2 = DateTime.UtcNow;
                TimeSpan elapsed = d2 - d1;
                TimeSpan loopDuration = TimeSpan.FromSeconds(1.0);
                if (Configuration != null && Configuration.LoopDuration > TimeSpan.Zero)
                {
                    loopDuration = Configuration.LoopDuration;
                }
                int waitMiliseconds = (int)(loopDuration - elapsed).TotalMilliseconds;
                if (waitMiliseconds > 0)
                {
                    await Task.Delay(waitMiliseconds, stoppingToken);
                }
            }
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
                    _logger.LogError(ex, "Impossible to create home directory for local storage");
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
                            Configuration? config = JsonConvert.DeserializeObject<Configuration>(jsonContent);
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
                    string defaultConfigJson = JsonConvert.SerializeObject(Configuration);
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
 
        private void RefreshSignals()
        {
            MicroStates microStates = new();
            SignalGroup? signals = null;
            Thresholds? thresholds = null;
            if (microStateSignalInputs_ != null && microStateSignalPlaceHolders_ != null)
            {
                lock (lock_)
                {
                    signals = new SignalGroup(microStateSignalInputs_);
                    thresholds = new Thresholds(microStateThresholds_);
                }
                foreach (var kvp in microStateSignalPlaceHolders_)
                {
                    if (!string.IsNullOrEmpty(kvp.Key) && kvp.Value != null && kvp.Value.Count > 0)
                    {
                        Type type = microStateSignalPlaceHolders_.GetType();
                        PropertyInfo? propertyInfo = type.GetProperty(kvp.Key);
                        if (propertyInfo != null)
                        {
                            object? obj = propertyInfo.GetValue(signals);
                            if (obj is not null and DrillingProperty drillingProperty)
                            {
                                if (drillingProperty is GaussianDrillingProperty gaussianDrillingProperty)
                                {
                                    double defaultStandardDeviation = 1e-6;
                                    List<byte>? sensorOptions = null;
                                    List<byte>? fullScaleOptions = null;
                                    var optionalFacts = propertyInfo.GetCustomAttributes<OptionalFactAttribute>();
                                    var semanticSensor = propertyInfo.GetCustomAttribute<SemanticSensorVariableAttribute>();
                                    var semanticFullScale = propertyInfo.GetCustomAttribute<SemanticFullScaleVariableAttribute>();
                                    var semanticGaussian = propertyInfo.GetCustomAttribute<SemanticGaussianVariableAttribute>();
                                    if (semanticGaussian != null && semanticGaussian.DefaultStandardDeviation != null)
                                    {
                                        defaultStandardDeviation = semanticGaussian.DefaultStandardDeviation.Value;
                                    }
                                    if (optionalFacts != null && semanticSensor != null && !string.IsNullOrEmpty(semanticSensor.PrecisionVariable) && !string.IsNullOrEmpty(semanticSensor.AccuracyVariable))
                                    {
                                        List<byte> options = [];
                                        foreach (var option in optionalFacts)
                                        {
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticSensor.PrecisionVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticSensor.PrecisionVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticSensor.AccuracyVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticSensor.AccuracyVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                        }
                                        if (options.Count > 0)
                                        {
                                            sensorOptions = options;
                                        }
                                    }
                                    if (optionalFacts != null && semanticFullScale != null && !string.IsNullOrEmpty(semanticFullScale.FullScaleVariable) && !string.IsNullOrEmpty(semanticFullScale.ProportionErrorVariable))
                                    {
                                        List<byte> options = [];
                                        foreach (var option in optionalFacts)
                                        {
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticFullScale.FullScaleVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticFullScale.FullScaleVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.SubjectName) && option.SubjectName.Equals(semanticFullScale.ProportionErrorVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                            if (option != null && !string.IsNullOrEmpty(option.ObjectName) && option.ObjectName.Equals(semanticFullScale.ProportionErrorVariable) && !options.Contains(option.GroupIndex))
                                            {
                                                options.Add(option.GroupIndex);
                                            }
                                        }
                                        if (options.Count > 0)
                                        {
                                            fullScaleOptions = options;
                                        }
                                    }
                                    gaussianDrillingProperty.FuseData(kvp.Value, defaultStandardDeviation, sensorOptions, fullScaleOptions);
                                }
                                else
                                {
                                    drillingProperty.FuseData(kvp.Value);
                                }
                            }
                        }
                    }
                }
                lock (lock_)
                {
                    signals.CopyTo(microStateSignalInputs_);
                }
            }
            ProbabilisticMicroStates probMicroStates = new ProbabilisticMicroStates();
            if (signals != null && thresholds != null)
            {
                if (signals.AxialVelocityTopOfString?.Mean != null &&
                    thresholds.ZeroAxialVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.AxialVelocityTopOfString.Mean, 0, thresholds.ZeroAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityTopOfString.Mean, 0, thresholds.ZeroAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityTopOfString, code);
                    if (probMicroStates.AxialVelocityTopOfString != null && probMicroStates.AxialVelocityTopOfString.Probabilities != null && probMicroStates.AxialVelocityTopOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.AxialVelocityTopOfString.ProbabilityGT(thresholds.ZeroAxialVelocityTopOfStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.AxialVelocityTopOfString.ProbabilityLT(-thresholds.ZeroAxialVelocityTopOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.AxialVelocityTopOfString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.AxialVelocityTopOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.AxialVelocityTopOfString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.StandardDeviationAxialVelocityTopOfString?.Mean != null &&
                    thresholds.StableAxialVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityTopOfString.Mean, thresholds.StableAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityTopOfString, code);
                    if (probMicroStates.StableAxialVelocityTopOfString != null)
                    {
                        probMicroStates.StableAxialVelocityTopOfString.Probability = signals.StandardDeviationAxialVelocityTopOfString.ProbabilityLT(thresholds.StableAxialVelocityTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.RotationalVelocityTopOfString?.Mean != null &&
                    thresholds.ZeroRotationalVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.RotationalVelocityTopOfString.Mean, 0, thresholds.ZeroRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationalVelocityTopOfString.Mean, 0, thresholds.ZeroRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityTopOfString, code);
                    if (probMicroStates.RotationalVelocityTopOfString != null && probMicroStates.RotationalVelocityTopOfString.Probabilities != null && probMicroStates.RotationalVelocityTopOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.RotationalVelocityTopOfString.ProbabilityGT(thresholds.ZeroRotationalVelocityTopOfStringThreshold.ScalarValue.Value); ;
                        double? prob3 = signals.RotationalVelocityTopOfString.ProbabilityLT(-thresholds.ZeroRotationalVelocityTopOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.RotationalVelocityTopOfString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.RotationalVelocityTopOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.RotationalVelocityTopOfString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.StandardDeviationRotationalVelocityTopOfString?.Mean != null &&
                    thresholds.StableRotationalVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityTopOfString.Mean, thresholds.StableRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityTopOfString, code);
                    if (probMicroStates.StableRotationalVelocityTopOfString != null)
                    {
                        probMicroStates.StableRotationalVelocityTopOfString.Probability = signals.StandardDeviationRotationalVelocityTopOfString.ProbabilityLT(thresholds.StableRotationalVelocityTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowTopOfString?.Mean != null &&
                    thresholds.ZeroFlowTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowTopOfString.Mean, 0, thresholds.ZeroFlowTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtTopOfString, code);
                    if (probMicroStates.FlowAtTopOfString != null)
                    {
                        probMicroStates.FlowAtTopOfString.Probability = signals.FlowTopOfString.ProbabilityGT(thresholds.ZeroFlowTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowTopOfString?.Mean != null &&
                    thresholds.StableFlowTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowTopOfString.Mean, thresholds.StableFlowTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtTopOfString, code);
                    if (probMicroStates.StableFlowAtTopOfString != null)
                    {
                        probMicroStates.StableFlowAtTopOfString.Probability = signals.StandardDeviationFlowTopOfString.ProbabilityLT(thresholds.StableFlowTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.TensionTopOfString?.Mean != null &&
                    signals.ForceBottomTopDrive?.Mean != null &&
                    signals.ForceElevator?.Mean != null &&
                    thresholds.ZeroTensionTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.TensionTopOfString.Mean, signals.ForceBottomTopDrive.Mean, thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value) ||
                        Numeric.EQ(signals.TensionTopOfString.Mean, signals.ForceElevator.Mean, thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (!Numeric.EQ(signals.TensionTopOfString.Mean, 0, thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value) &&
                             Numeric.EQ(signals.ForceBottomTopDrive.Mean, 0, thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value) &&
                             Numeric.EQ(signals.ForceElevator.Mean, 0, thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.SlipState, code);
                    if (probMicroStates.SlipState != null && probMicroStates.SlipState.Probabilities != null && probMicroStates.SlipState.Probabilities.Length == 3)
                    {
                        GaussianDrillingProperty low = new GaussianDrillingProperty() { Mean = signals.ForceBottomTopDrive.Mean - thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value, StandardDeviation = signals.ForceBottomTopDrive.StandardDeviation };
                        GaussianDrillingProperty high = new GaussianDrillingProperty() { Mean = signals.ForceBottomTopDrive.Mean + thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value, StandardDeviation = signals.ForceBottomTopDrive.StandardDeviation };
                        double? prob1 = signals.TensionTopOfString.ProbabilityLT(low);
                        double? prob2 = signals.TensionTopOfString.ProbabilityGT(high);
                        double? probA = 1.0 - prob1 - prob2;
                        low = new GaussianDrillingProperty() { Mean = signals.ForceElevator.Mean - thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value, StandardDeviation = signals.ForceElevator.StandardDeviation };
                        high = new GaussianDrillingProperty() { Mean = signals.ForceElevator.Mean + thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value, StandardDeviation = signals.ForceElevator.StandardDeviation };
                        prob1 = signals.TensionTopOfString.ProbabilityLT(low);
                        prob2 = signals.TensionTopOfString.ProbabilityGT(high);
                        double? probB = 1.0 - prob1 - prob2;
                        prob1 = signals.TensionTopOfString.ProbabilityGT(thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        prob2 = signals.TensionTopOfString.ProbabilityLT(-thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        double? probX1 = prob1 + prob2;
                        prob1 = signals.ForceBottomTopDrive.ProbabilityGT(thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        prob2 = signals.ForceBottomTopDrive.ProbabilityLT(-thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        double? probX2 = 1.0 - prob1 - prob2;
                        prob1 = signals.ForceElevator.ProbabilityGT(thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        prob2 = signals.ForceElevator.ProbabilityLT(-thresholds.ZeroTensionTopOfStringThreshold.ScalarValue.Value);
                        double? probX3 = 1.0 - prob1 - prob2;
                        if (probA != null && probB != null && probX1 != null && probX2 != null && probX3 != null)
                        {
                            probMicroStates.SlipState.Probabilities[0] = probA.Value + probB.Value - probA.Value * probB.Value;
                            probMicroStates.SlipState.Probabilities[1] = probX1.Value * probX2.Value * probX3.Value;
                            probMicroStates.SlipState.Probabilities[2] = 1.0 - probMicroStates.SlipState.Probabilities[0] - probMicroStates.SlipState.Probabilities[1];
                        }
                    }
                }
                if (signals.StandardDeviationTensionTopOfString?.Mean != null &&
                    thresholds.StableTensionTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationTensionTopOfString.Mean, thresholds.StableTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTensionTopOfString, code);
                    if (probMicroStates.StableTensionTopOfString != null)
                    {
                        probMicroStates.StableTensionTopOfString.Probability = signals.StandardDeviationTensionTopOfString.ProbabilityLT(thresholds.StableTensionTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.PressureTopOfString?.Mean != null &&
                    thresholds.ZeroPressureTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.PressureTopOfString.Mean, Constants.EarthStandardAtmosphericPressure, thresholds.ZeroPressureTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PressureTopOfString, code);
                    if (probMicroStates.PressureTopOfString != null)
                    {
                        probMicroStates.PressureTopOfString.Probability = signals.PressureTopOfString.ProbabilityGT(Constants.EarthStandardAtmosphericPressure + thresholds.ZeroPressureTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationPressureTopOfString?.Mean != null &&
                    thresholds.StablePressureTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationPressureTopOfString.Mean, thresholds.StablePressureTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StablePressureTopOfString, code);
                    if (probMicroStates.StablePressureTopOfString != null)
                    {
                        probMicroStates.StablePressureTopOfString.Probability = signals.StandardDeviationPressureTopOfString.ProbabilityLT(thresholds.StablePressureTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.TorqueTopOfString?.Mean != null &&
                    thresholds.ZeroTorqueTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.TorqueTopOfString.Mean, 0, thresholds.ZeroTorqueTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TorqueTopOfString, code);
                    if (probMicroStates.TorqueTopOfString != null)
                    {
                        probMicroStates.TorqueTopOfString.Probability = signals.TorqueTopOfString.ProbabilityGT(thresholds.ZeroTorqueTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationTorqueTopOfString?.Mean != null &&
                    thresholds.StableTorqueTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationTorqueTopOfString.Mean, thresholds.StableTorqueTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTorqueTopOfString, code);
                    if (probMicroStates.StableTorqueTopOfString != null)
                    {
                        probMicroStates.StableTorqueTopOfString.Probability = signals.StandardDeviationTorqueTopOfString.ProbabilityLT(thresholds.StableTorqueTopOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowAnnulusOutlet?.Mean != null &&
                    thresholds.ZeroFlowAnnulusOutletThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowAnnulusOutlet.Mean, 0, thresholds.ZeroFlowAnnulusOutletThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtAnnulusOutlet, code);
                    if (probMicroStates.FlowAtAnnulusOutlet != null)
                    {
                        probMicroStates.FlowAtAnnulusOutlet.Probability = signals.FlowAnnulusOutlet.ProbabilityGT(thresholds.ZeroFlowAnnulusOutletThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowAnnulusOutlet?.Mean != null &&
                    thresholds.StableFlowAnnulusOutletThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowAnnulusOutlet.Mean, thresholds.StableFlowAnnulusOutletThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtAnnulusOutlet, code);
                    if (probMicroStates.StableFlowAtAnnulusOutlet != null)
                    {
                        probMicroStates.StableFlowAtAnnulusOutlet.Probability = signals.StandardDeviationFlowAnnulusOutlet.ProbabilityLT(thresholds.StableFlowAnnulusOutletThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowCuttingsAnnulusOutlet?.Mean != null &&
                    thresholds.ZeroCuttingsFlowAnnulusOutletThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowCuttingsAnnulusOutlet.Mean, 0, thresholds.ZeroCuttingsFlowAnnulusOutletThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsReturnAtAnnulusOutlet, code);
                    if (probMicroStates.CuttingsReturnAtAnnulusOutlet != null)
                    {
                        probMicroStates.CuttingsReturnAtAnnulusOutlet.Probability = signals.FlowCuttingsAnnulusOutlet.ProbabilityGT(thresholds.ZeroCuttingsFlowAnnulusOutletThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ForceBottomOfStringOnRock?.Mean != null &&
                    thresholds.ZeroBottomOfStringRockForceThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.ForceBottomOfStringOnRock.Mean, 0, thresholds.ZeroBottomOfStringRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomBottomOfString, code);
                    if (probMicroStates.OnBottomBottomOfString != null)
                    {
                        probMicroStates.OnBottomBottomOfString.Probability = signals.ForceBottomOfStringOnRock.ProbabilityGT(thresholds.ZeroBottomOfStringRockForceThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationForceBottomOfStringOnRock?.Mean != null &&
                    thresholds.StableBottomOfStringRockForceThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationForceBottomOfStringOnRock.Mean, thresholds.StableBottomOfStringRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBottomOfStringRockForce, code);
                    if (probMicroStates.StableBottomOfStringRockForce != null)
                    {
                        probMicroStates.StableBottomOfStringRockForce.Probability = signals.StandardDeviationForceBottomOfStringOnRock.ProbabilityLT(thresholds.StableBottomOfStringRockForceThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ForceHoleOpenerOnRock?.Mean != null &&
                    thresholds.ZeroHoleOpenerOnRockForceThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.ForceHoleOpenerOnRock.Mean, 0, thresholds.ZeroHoleOpenerOnRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomHoleOpener, code);
                    if (probMicroStates.OnBottomHoleOpener != null)
                    {
                        probMicroStates.OnBottomHoleOpener.Probability = signals.ForceHoleOpenerOnRock.ProbabilityGT(thresholds.ZeroHoleOpenerOnRockForceThreshold.ScalarValue.Value);
                    }
                }
                if (signals.RotationaVelocityBottomOfString?.Mean != null &&
                    thresholds.ZeroRotationalVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.RotationaVelocityBottomOfString.Mean, 0, thresholds.ZeroRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationaVelocityBottomOfString.Mean, thresholds.ZeroRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityBottomOfString, code);
                    if (probMicroStates.RotationalVelocityBottomOfString != null && probMicroStates.RotationalVelocityBottomOfString.Probabilities != null && probMicroStates.RotationalVelocityBottomOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.RotationaVelocityBottomOfString.ProbabilityGT(thresholds.ZeroRotationalVelocityBottomOfStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.RotationaVelocityBottomOfString.ProbabilityLT(-thresholds.ZeroRotationalVelocityBottomOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.RotationalVelocityBottomOfString.Probabilities[0] = 1 - prob2.Value - prob3.Value;
                            probMicroStates.RotationalVelocityBottomOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.RotationalVelocityBottomOfString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.StandardDeviationRotationalVelocityBottomOfString?.Mean != null &&
                    thresholds.StableRotationalVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityBottomOfString.Mean, thresholds.StableRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityBottomOfString, code);
                    if (probMicroStates.StableRotationalVelocityBottomOfString != null)
                    {
                        probMicroStates.StableRotationalVelocityBottomOfString.Probability = signals.StandardDeviationRotationalVelocityBottomOfString.ProbabilityLT(thresholds.StableRotationalVelocityBottomOfStringThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowCuttingsBottomHole?.Mean != null &&
                    thresholds.ZeroCuttingsFlowBottomHoleThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowCuttingsBottomHole.Mean, 0, thresholds.ZeroCuttingsFlowBottomHoleThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Drilling, code);
                    if (probMicroStates.Drilling != null)
                    {
                        probMicroStates.Drilling.Probability = signals.FlowCuttingsBottomHole.ProbabilityGT(thresholds.ZeroCuttingsFlowBottomHoleThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowCuttingsTopOfRateHole?.Mean != null && thresholds.ZeroCuttingsFlowTopOfRatHoleThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowCuttingsTopOfRateHole.Mean, 0, thresholds.ZeroCuttingsFlowTopOfRatHoleThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HoleOpening, code);
                    if (probMicroStates.HoleOpening != null)
                    {
                        probMicroStates.HoleOpening.Probability = signals.FlowCuttingsTopOfRateHole.ProbabilityGT(thresholds.ZeroCuttingsFlowTopOfRatHoleThreshold.ScalarValue.Value);
                    }
                }
                if (signals.AxialVelocityBottomOfString?.Mean != null && thresholds.ZeroAxialVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.AxialVelocityBottomOfString.Mean, 0, thresholds.ZeroAxialVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityBottomOfString.Mean, 0, thresholds.ZeroAxialVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityBottomOfString, code);
                    if (probMicroStates.AxialVelocityBottomOfString != null && probMicroStates.AxialVelocityBottomOfString.Probabilities != null && probMicroStates.AxialVelocityBottomOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.AxialVelocityBottomOfString.ProbabilityGT(thresholds.ZeroAxialVelocityBottomOfStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.AxialVelocityBottomOfString.ProbabilityLT(-thresholds.ZeroAxialVelocityBottomOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.AxialVelocityBottomOfString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.AxialVelocityBottomOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.AxialVelocityBottomOfString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.StandardDeviationAxialVelocityBottomOfString?.Mean != null && thresholds.StableAxialVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityBottomOfString.Mean, thresholds.StableAxialVelocityBottomOfStringThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityBottomOfString, code);
                    if (probMicroStates.StableAxialVelocityBottomOfString != null)
                    {
                        probMicroStates.StableAxialVelocityBottomOfString.Probability = signals.StandardDeviationAxialVelocityBottomOfString.ProbabilityLT(thresholds.StableAxialVelocityBottomOfStringThreshold.ScalarValue);
                    }
                }
                if (signals.FlowBottomOfString?.Mean != null && thresholds.ZeroFlowBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.FlowBottomOfString.Mean, 0, thresholds.ZeroFlowBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowBottomOfString.Mean, 0, thresholds.ZeroFlowBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowBottomOfString, code);
                    if (probMicroStates.FlowBottomOfString != null && probMicroStates.FlowBottomOfString.Probabilities != null && probMicroStates.FlowBottomOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.FlowBottomOfString.ProbabilityGT(thresholds.ZeroFlowBottomOfStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.FlowBottomOfString.ProbabilityLT(-thresholds.ZeroFlowBottomOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.FlowBottomOfString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.FlowBottomOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.FlowBottomOfString.Probabilities[2] = prob3.Value;
                        }

                    }
                }
                if (signals.StableFlowBottomOfString?.Mean != null && thresholds.StableFlowBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StableFlowBottomOfString.Mean, thresholds.StableFlowBottomOfStringThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowBottomOfString, code);
                    if (probMicroStates.StableFlowBottomOfString != null)
                    {
                        probMicroStates.StableFlowBottomOfString.Probability = signals.StableFlowBottomOfString.ProbabilityLT(thresholds.StableFlowBottomOfStringThreshold.ScalarValue);
                    }
                }
                if (signals.FlowHoleOpener?.Mean != null && thresholds.ZeroFlowHoleOpenerThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.FlowHoleOpener.Mean, 0, thresholds.ZeroFlowHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowHoleOpener.Mean, 0, thresholds.ZeroFlowHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowHoleOpener, code);
                    if (probMicroStates.FlowHoleOpener != null && probMicroStates.FlowHoleOpener.Probabilities != null && probMicroStates.FlowHoleOpener.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.FlowHoleOpener.ProbabilityGT(thresholds.ZeroFlowHoleOpenerThreshold.ScalarValue.Value);
                        double? prob3 = signals.FlowHoleOpener.ProbabilityLT(-thresholds.ZeroFlowHoleOpenerThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.FlowHoleOpener.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.FlowHoleOpener.Probabilities[1] = prob2.Value;
                            probMicroStates.FlowHoleOpener.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.StableFlowHoleOpener?.Mean != null && thresholds.StableFlowHoleOpenerThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StableFlowHoleOpener.Mean, thresholds.StableFlowHoleOpenerThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowHoleOpener, code);
                    if (probMicroStates.StableFlowHoleOpener != null)
                    {
                        probMicroStates.StableFlowHoleOpener.Probability = signals.StableFlowHoleOpener.ProbabilityLT(thresholds.StableFlowHoleOpenerThreshold.ScalarValue);
                    }
                }
                if (signals.ForceOnLedge?.Mean != null && thresholds.ForceOnLedgeThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.ForceOnLedge.Mean, 0, thresholds.ForceOnLedgeThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnLedge.Mean, 0, thresholds.ForceOnLedgeThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LedgeKeySeat, code);
                    if (probMicroStates.LedgeKeySeat != null && probMicroStates.LedgeKeySeat.Probabilities != null && probMicroStates.LedgeKeySeat.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.ForceOnLedge.ProbabilityGT(thresholds.ForceOnLedgeThreshold.ScalarValue.Value);
                        double? prob3 = signals.ForceOnLedge.ProbabilityLT(-thresholds.ForceOnLedgeThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.LedgeKeySeat.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.LedgeKeySeat.Probabilities[1] = prob2.Value;
                            probMicroStates.LedgeKeySeat.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.ForceOnCuttingsBed?.Mean != null && thresholds.ForceOnCuttingsBedThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.ForceOnCuttingsBed.Mean, 0, thresholds.ForceOnCuttingsBedThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnCuttingsBed.Mean, 0, thresholds.ForceOnCuttingsBedThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsBed, code);
                    if (probMicroStates.CuttingsBed != null && probMicroStates.CuttingsBed.Probabilities != null && probMicroStates.CuttingsBed.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.ForceOnCuttingsBed.ProbabilityGT(thresholds.ForceOnCuttingsBedThreshold.ScalarValue.Value);
                        double? prob3 = signals.ForceOnCuttingsBed.ProbabilityLT(-thresholds.ForceOnCuttingsBedThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.CuttingsBed.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.CuttingsBed.Probabilities[1] = prob2.Value;
                            probMicroStates.CuttingsBed.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.ForceDifferentialSticking?.Mean != null && thresholds.ForceDifferentialStickingThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.ForceDifferentialSticking.Mean, 0, thresholds.ForceDifferentialStickingThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.DifferentialSticking, code);
                    if (probMicroStates.DifferentialSticking != null)
                    {
                        probMicroStates.DifferentialSticking.Probability = signals.ForceDifferentialSticking.ProbabilityGT(thresholds.ForceDifferentialStickingThreshold.ScalarValue.Value);
                    }
                }
                if (signals.TensionTopOfString?.Mean != null && signals.MinimumTensionForTwistOffDetection?.Mean != null)
                {
                    uint code;
                    if (Numeric.GE(signals.TensionTopOfString.Mean, signals.MinimumTensionForTwistOffDetection.Mean))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TwistOffBackOff, code);
                    if (probMicroStates.TwistOffBackOff != null)
                    {
                        probMicroStates.TwistOffBackOff.Probability = signals.TensionTopOfString.ProbabilityLT(signals.MinimumTensionForTwistOffDetection);
                    }
                }
                if (signals.FlowFluidFromOrToFormation?.Mean != null && thresholds.FluidFlowFormationThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.FlowFluidFromOrToFormation.Mean, 0, thresholds.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowFluidFromOrToFormation.Mean, 0, thresholds.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WellIntegrity, code);
                    if (probMicroStates.WellIntegrity != null && probMicroStates.WellIntegrity.Probabilities != null && probMicroStates.WellIntegrity.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.FlowFluidFromOrToFormation.ProbabilityGT(thresholds.FluidFlowFormationThreshold.ScalarValue.Value);
                        double? prob3 = signals.FlowFluidFromOrToFormation.ProbabilityLT(-thresholds.FluidFlowFormationThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.WellIntegrity.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.WellIntegrity.Probabilities[1] = prob2.Value;
                            probMicroStates.WellIntegrity.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.FlowFormationFluidAnnulusOutlet?.Mean != null && thresholds.FluidFlowFormationThreshold?.ScalarValue != null)
                {
                    uint code;

                    if (Numeric.LE(signals.FlowFormationFluidAnnulusOutlet.Mean, 0, thresholds.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationFluidAtAnnulusOutlet, code);
                    if (probMicroStates.FormationFluidAtAnnulusOutlet != null)
                    {
                        probMicroStates.FormationFluidAtAnnulusOutlet.Probability = signals.FlowFormationFluidAnnulusOutlet.ProbabilityGT(thresholds.FluidFlowFormationThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowCavingsFromFormation?.Mean != null && thresholds.FlowCavingsFromFormationThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowCavingsFromFormation.Mean, 0, thresholds.FlowCavingsFromFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationCollapse, code);
                    if (probMicroStates.FormationCollapse != null)
                    {
                        probMicroStates.FormationCollapse.Probability = signals.FlowCavingsFromFormation.ProbabilityGT(thresholds.FlowCavingsFromFormationThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowCavingsAnnulusOutlet?.Mean != null && thresholds.FlowCavingsFromFormationThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowCavingsAnnulusOutlet.Mean, 0, thresholds.FlowCavingsFromFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CavingsAtAnnulusOutlet, code);
                    if (probMicroStates.CavingsAtAnnulusOutlet != null)
                    {
                        probMicroStates.CavingsAtAnnulusOutlet.Probability = signals.FlowCavingsAnnulusOutlet.ProbabilityGT(thresholds.FlowCavingsFromFormationThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowPipeToAnnulus?.Mean != null && thresholds.FlowPipeToAnnulusThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowPipeToAnnulus.Mean, 0, thresholds.FlowPipeToAnnulusThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PipeWashout, code);
                    if (probMicroStates.PipeWashout != null)
                    {
                        probMicroStates.PipeWashout.Probability = signals.FlowPipeToAnnulus.ProbabilityGT(thresholds.FlowPipeToAnnulusThreshold.ScalarValue.Value);
                    }
                }
                if (signals.WhirlRateBottomOfString?.Mean != null && thresholds.WhirlRateBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.WhirlRateBottomOfString.Mean, 0, thresholds.WhirlRateBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateBottomOfString.Mean, 0, thresholds.WhirlRateBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlBottomOfString, code);
                    if (probMicroStates.WhirlBottomOfString != null && probMicroStates.WhirlBottomOfString.Probabilities != null && probMicroStates.WhirlBottomOfString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.WhirlRateBottomOfString.ProbabilityGT(thresholds.WhirlRateBottomOfStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.WhirlRateBottomOfString.ProbabilityLT(-thresholds.WhirlRateBottomOfStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.WhirlBottomOfString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.WhirlBottomOfString.Probabilities[1] = prob2.Value;
                            probMicroStates.WhirlBottomOfString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.WhirlRateHoleOpener?.Mean != null && thresholds.WhirlRateHoleOpenerThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.WhirlRateHoleOpener.Mean, 0, thresholds.WhirlRateHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateHoleOpener.Mean, 0, thresholds.WhirlRateHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlHoleOpener, code);
                    if (probMicroStates.WhirlHoleOpener != null && probMicroStates.WhirlHoleOpener.Probabilities != null && probMicroStates.WhirlHoleOpener.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.WhirlRateHoleOpener.ProbabilityGT(thresholds.WhirlRateHoleOpenerThreshold.ScalarValue.Value);
                        double? prob3 = signals.WhirlRateHoleOpener.ProbabilityLT(-thresholds.WhirlRateHoleOpenerThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.WhirlHoleOpener.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.WhirlHoleOpener.Probabilities[1] = prob2.Value;
                            probMicroStates.WhirlHoleOpener.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.WhirlRateDrillString?.Mean != null && thresholds.WhirlRateDrillStringThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.EQ(signals.WhirlRateDrillString.Mean, 0, thresholds.WhirlRateDrillStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateDrillString.Mean, 0, thresholds.WhirlRateDrillStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlInDrillString, code);
                    if (probMicroStates.WhirlInDrillString != null && probMicroStates.WhirlInDrillString.Probabilities != null && probMicroStates.WhirlInDrillString.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.WhirlRateDrillString.ProbabilityGT(thresholds.WhirlRateDrillStringThreshold.ScalarValue.Value);
                        double? prob3 = signals.WhirlRateDrillString.ProbabilityLT(-thresholds.WhirlRateDrillStringThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.WhirlInDrillString.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.WhirlInDrillString.Probabilities[1] = prob2.Value;
                            probMicroStates.WhirlInDrillString.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.DifferentialPressureFloatValve?.Mean != null && thresholds.MinimumPressureFloatValve?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.DifferentialPressureFloatValve.Mean, thresholds.MinimumPressureFloatValve.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FloatSub, code);
                    if (probMicroStates.FloatSub != null)
                    {
                        probMicroStates.FloatSub.Probability = signals.DifferentialPressureFloatValve.ProbabilityGT(thresholds.MinimumPressureFloatValve.ScalarValue.Value);
                    }
                }
                if (signals.UnderReamerOpen != null && signals.UnderReamerOpen.BooleanValue != null)
                {
                    uint code;
                    if (signals.UnderReamerOpen.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.UnderReamer, code);
                    if (probMicroStates.UnderReamer != null)
                    {
                        probMicroStates.UnderReamer.Probability = signals.UnderReamerOpen.Probability;
                    }
                }
                if (signals.CirculationSubOpen != null && signals.CirculationSubOpen.BooleanValue != null)
                {
                    uint code;
                    if (signals.CirculationSubOpen.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CirculationSub, code);
                    if (probMicroStates.CirculationSub != null)
                    {
                        probMicroStates.CirculationSub.Probability = signals.CirculationSubOpen.Probability;
                    }
                }
                if (signals.PortedFloatOpen != null && signals.PortedFloatOpen.BooleanValue != null)
                {
                    uint code;
                    if (signals.PortedFloatOpen.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PortedFloat, code);
                    if (probMicroStates.PortedFloat != null)
                    {
                        probMicroStates.PortedFloat.Probability = signals.PortedFloatOpen.Probability;
                    }
                }
                if (signals.WhipstockAttached != null && signals.WhipstockAttached.BooleanValue != null)
                {
                    uint code;
                    if (signals.WhipstockAttached.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Whipstock, code);
                    if (probMicroStates.Whipstock != null)
                    {
                        probMicroStates.Whipstock.Probability = signals.WhipstockAttached.Probability;
                    }
                }
                if (signals.PlugAttached != null && signals.PlugAttached.BooleanValue != null)
                {
                    uint code;
                    if (signals.PlugAttached.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Plug, code);
                    if (probMicroStates.Plug != null)
                    {
                        probMicroStates.Plug.Probability = signals.PlugAttached.Probability;
                    }
                }
                if (signals.LinerAttached != null && signals.LinerAttached.BooleanValue != null)
                {
                    uint code;
                    if (signals.LinerAttached.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Liner, code);
                    if (probMicroStates.Liner != null)
                    {
                        probMicroStates.Liner.Probability = signals.LinerAttached.Probability;
                    }
                }
                if (signals.FlowBoosterPump?.Mean != null && thresholds.ZeroFlowBoosterPumpThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowBoosterPump.Mean, 0, thresholds.ZeroFlowBoosterPumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BoosterPumping, code);
                    if (probMicroStates.BoosterPumping != null)
                    {
                        probMicroStates.BoosterPumping.Probability = signals.FlowBoosterPump.ProbabilityGT(thresholds.ZeroFlowBoosterPumpThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowBoosterPump?.Mean != null && thresholds.StableFlowBoosterPumpThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowBoosterPump.Mean, thresholds.StableFlowBoosterPumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBoosterPumping, code);
                    if (probMicroStates.StableBoosterPumping != null)
                    {
                        probMicroStates.StableBoosterPumping.Probability = signals.StandardDeviationFlowBoosterPump.ProbabilityLT(thresholds.StableFlowBoosterPumpThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowBackPressurePump?.Mean != null && thresholds.ZeroFlowBackPressurePumpThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowBackPressurePump.Mean, 0, thresholds.ZeroFlowBackPressurePumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BackPressurePumping, code);
                    if (probMicroStates.BackPressurePumping != null)
                    {
                        probMicroStates.BackPressurePumping.Probability = signals.FlowBackPressurePump.ProbabilityGT(thresholds.ZeroFlowBackPressurePumpThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowBackPressurePump?.Mean != null && thresholds.StableFlowBackPressurePumpThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowBackPressurePump.Mean, thresholds.StableFlowBackPressurePumpThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBackPressurePumping, code);
                    if (probMicroStates.StableBackPressurePumping != null)
                    {
                        probMicroStates.StableBackPressurePumping.Probability = signals.StandardDeviationFlowBackPressurePump.ProbabilityLT(thresholds.StableFlowBackPressurePumpThreshold.ScalarValue);
                    }
                }
                if (signals.OpeningMPDChoke?.Mean != null)
                {
                    uint code;
                    if (Numeric.LE(signals.OpeningMPDChoke.Mean, 0))
                    {
                        code = 1;
                    }
                    else if (Numeric.GE(signals.OpeningMPDChoke.Mean, 1))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MPDChokeOpening, code);
                    if (probMicroStates.MPDChokeOpening != null && probMicroStates.MPDChokeOpening.Probabilities != null && probMicroStates.MPDChokeOpening.Probabilities.Length == 3)
                    {
                        double? prob1 = signals.OpeningMPDChoke.ProbabilityLT(0 + 1e-4);
                        double? prob2 = signals.OpeningMPDChoke.ProbabilityGT(1 - 1e-4);
                        if (prob1 != null && prob2 != null)
                        {
                            probMicroStates.MPDChokeOpening.Probabilities[0] = prob1.Value;
                            probMicroStates.MPDChokeOpening.Probabilities[1] = prob2.Value;
                            probMicroStates.MPDChokeOpening.Probabilities[2] = 1 - prob1.Value - prob2.Value;
                        }
                    }
                }
                if (signals.DifferentialPressureRCD?.Mean != null && thresholds.MinimumDifferentialPressureRCDSealingThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.DifferentialPressureRCD.Mean, 0, thresholds.MinimumDifferentialPressureRCDSealingThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RCDSealing, code);
                    if (probMicroStates.RCDSealing != null)
                    {
                        probMicroStates.RCDSealing.Probability = signals.DifferentialPressureRCD.ProbabilityGT(thresholds.MinimumDifferentialPressureRCDSealingThreshold.ScalarValue.Value);
                    }
                }
                if (signals.IsolationSealActivated != null && signals.IsolationSealActivated.BooleanValue != null)
                {
                    uint code;
                    if (!signals.IsolationSealActivated.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSeal, code);
                    if (probMicroStates.IsolationSeal != null)
                    {
                        probMicroStates.IsolationSeal.Probability = signals.IsolationSealActivated.Probability;
                    }
                }
                if (signals.DifferentialPressureIsolationSeal?.Mean != null && thresholds.MinimumDifferentialPressureSealBalanceThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.DifferentialPressureIsolationSeal.Mean, 0, thresholds.MinimumDifferentialPressureSealBalanceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSealPressureBalance, code);
                    if (probMicroStates.IsolationSealPressureBalance != null)
                    {
                        probMicroStates.IsolationSealPressureBalance.Probability = signals.DifferentialPressureIsolationSeal.ProbabilityGT(thresholds.MinimumDifferentialPressureSealBalanceThreshold.ScalarValue.Value);
                    }
                }
                if (signals.BearingAssemblyLatched != null && signals.BearingAssemblyLatched.BooleanValue != null)
                {
                    uint code;
                    if (!signals.BearingAssemblyLatched.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BearingAssemblyLatched, code);
                    if (probMicroStates.BearingAssemblyLatched != null)
                    {
                        probMicroStates.BearingAssemblyLatched.Probability = signals.BearingAssemblyLatched.Probability;
                    }
                }
                if (signals.ScreenMPDChokePlugged != null && signals.ScreenMPDChokePlugged.BooleanValue != null)
                {
                    uint code;
                    if (signals.ScreenMPDChokePlugged.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ScreenMPDChokePlugged, code);
                    if (probMicroStates.ScreenMPDChokePlugged != null)
                    {
                        probMicroStates.ScreenMPDChokePlugged.Probability = 1.0 - signals.ScreenMPDChokePlugged.Probability;
                    }
                }
                if (signals.MainFlowPathMPDEstablished != null && signals.MainFlowPathMPDEstablished.BooleanValue != null)
                {
                    uint code;
                    if (!signals.MainFlowPathMPDEstablished.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MainFlowPathStable, code);
                    if (probMicroStates.MainFlowPathStable != null)
                    {
                        probMicroStates.MainFlowPathStable.Probability = signals.MainFlowPathMPDEstablished.Probability;
                    }
                }
                if (signals.AlternateFlowPathMPDEstablished != null && signals.AlternateFlowPathMPDEstablished.BooleanValue != null)
                {
                    uint code;
                    if (!signals.AlternateFlowPathMPDEstablished.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AlternateFlowPathStable, code);
                    if (probMicroStates.AlternateFlowPathStable != null)
                    {
                        probMicroStates.AlternateFlowPathStable.Probability = signals.AlternateFlowPathMPDEstablished.Probability;
                    }
                }
                if (signals.FlowFillPumpDGD?.Mean != null && thresholds.ZeroFlowFillPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowFillPumpDGD.Mean, 0, thresholds.ZeroFlowFillPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FillPumpDGD, code);
                    if (probMicroStates.FillPumpDGD != null)
                    {
                        probMicroStates.FillPumpDGD.Probability = signals.FlowFillPumpDGD.ProbabilityGT(thresholds.ZeroFlowFillPumpDGDThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowFillPumpDGD?.Mean != null && thresholds.StableFlowFillPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowFillPumpDGD.Mean, thresholds.StableFlowFillPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFillPumpDGD, code);
                    if (probMicroStates.StableFillPumpDGD != null)
                    {
                        probMicroStates.StableFillPumpDGD.Probability = signals.StandardDeviationFlowFillPumpDGD.ProbabilityLT(thresholds.StableFlowFillPumpDGDThreshold.ScalarValue.Value);
                    }
                }
                if (signals.FlowLiftPumpDGD?.Mean != null && thresholds.ZeroFlowLiftPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.FlowLiftPumpDGD.Mean, 0, thresholds.ZeroFlowLiftPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LiftPumpDGD, code);
                    if (probMicroStates.LiftPumpDGD != null)
                    {
                        probMicroStates.LiftPumpDGD.Probability = signals.FlowLiftPumpDGD.ProbabilityGT(thresholds.ZeroFlowLiftPumpDGDThreshold.ScalarValue.Value);
                    }
                }
                if (signals.StandardDeviationFlowLiftPumpDGD?.Mean != null && thresholds.StableFlowLiftPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.StandardDeviationFlowLiftPumpDGD.Mean, thresholds.StableFlowLiftPumpDGDThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableLiftPumpDGD, code);
                    if (probMicroStates.StableLiftPumpDGD != null)
                    {
                        probMicroStates.StableLiftPumpDGD.Probability = signals.StandardDeviationFlowLiftPumpDGD.ProbabilityLT(thresholds.StableFlowLiftPumpDGDThreshold.ScalarValue);
                    }
                }
                if (signals.UCS?.Mean != null && thresholds.HardStringerThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.UCS.Mean, thresholds.HardStringerThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.InsideHardStringer, code);
                    if (probMicroStates.InsideHardStringer != null)
                    {
                        probMicroStates.InsideHardStringer.Probability = signals.UCS.ProbabilityGT(thresholds.HardStringerThreshold.ScalarValue.Value);
                    }
                }
                if (signals.UCSSlope?.Mean != null && thresholds.ChangeOfFormationUCSSlopeThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(signals.UCSSlope.Mean, -thresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue) &&
                        Numeric.LT(signals.UCSSlope.Mean, thresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else if (Numeric.GE(signals.UCSSlope.Mean, thresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationChange, code);
                    if (probMicroStates.FormationChange != null && probMicroStates.FormationChange.Probabilities != null && probMicroStates.FormationChange.Probabilities.Length == 3)
                    {
                        double? prob2 = signals.UCSSlope.ProbabilityGT(thresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue.Value);
                        double? prob3 = signals.UCSSlope.ProbabilityLT(-thresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue.Value);
                        if (prob2 != null && prob3 != null)
                        {
                            probMicroStates.FormationChange.Probabilities[0] = 1.0 - prob2.Value - prob3.Value;
                            probMicroStates.FormationChange.Probabilities[1] = prob2.Value;
                            probMicroStates.FormationChange.Probabilities[2] = prob3.Value;
                        }
                    }
                }
                if (signals.ToolJoint1Height?.Mean != null && signals.MinDrillHeight?.ScalarValue != null && thresholds.AtDrillHeightThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint1Height.Mean.Value - signals.MinDrillHeight.ScalarValue.Value), thresholds.AtDrillHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtLowestDrillHeight, code);
                    if (probMicroStates.ToolJoint1AtLowestDrillHeight != null)
                    {
                        probMicroStates.ToolJoint1AtLowestDrillHeight.Probability = signals.ToolJoint1Height.ProbabilityLT(signals.MinDrillHeight.ScalarValue.Value + thresholds.AtDrillHeightThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ToolJoint1Height?.Mean != null && signals.StickUpHeight?.ScalarValue != null && thresholds.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint1Height.Mean.Value - signals.StickUpHeight.ScalarValue.Value), thresholds.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtStickUpHeight, code);
                    if (probMicroStates.ToolJoint1AtStickUpHeight != null)
                    {
                        probMicroStates.ToolJoint1AtStickUpHeight.Probability = signals.ToolJoint1Height.ProbabilityLT(signals.StickUpHeight.ScalarValue.Value + thresholds.AtStickUpHeightThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ToolJoint2Height?.Mean != null && signals.StickUpHeight?.ScalarValue != null && thresholds.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint2Height.Mean.Value - signals.StickUpHeight.ScalarValue.Value), thresholds.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint2AtStickUpHeight, code);
                    if (probMicroStates.ToolJoint2AtStickUpHeight != null)
                    {
                        probMicroStates.ToolJoint2AtStickUpHeight.Probability = signals.ToolJoint2Height.ProbabilityLT(signals.StickUpHeight.ScalarValue.Value + thresholds.AtStickUpHeightThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ToolJoint3Height?.Mean != null && signals.StickUpHeight?.ScalarValue != null && thresholds.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint3Height.Mean.Value - signals.StickUpHeight.ScalarValue.Value), thresholds.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint3AtStickUpHeight, code);
                    if (probMicroStates.ToolJoint3AtStickUpHeight != null)
                    {
                        probMicroStates.ToolJoint3AtStickUpHeight.Probability = signals.ToolJoint3Height.ProbabilityLT(signals.StickUpHeight.ScalarValue.Value + thresholds.AtStickUpHeightThreshold.ScalarValue.Value);
                    }
                }
                if (signals.ToolJoint4Height?.Mean != null && signals.StickUpHeight?.ScalarValue != null && thresholds.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint4Height.Mean.Value - signals.StickUpHeight.ScalarValue.Value), thresholds.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint4AtStickUpHeight, code);
                    if (probMicroStates.ToolJoint4AtStickUpHeight != null)
                    {
                        probMicroStates.ToolJoint4AtStickUpHeight.Probability = signals.ToolJoint4Height.ProbabilityLT(signals.StickUpHeight.ScalarValue.Value + thresholds.AtStickUpHeightThreshold.ScalarValue.Value);
                    }
                }
                if (signals.HeaveCompensationInactive != null &&
                    signals.HeaveCompensationInactive.BooleanValue != null &&
                    signals.HeaveCompensationActive != null &&
                    signals.HeaveCompensationActive.BooleanValue != null)
                {
                    uint code = 0;
                    if (signals.HeaveCompensationInactive.BooleanValue.Value && !signals.HeaveCompensationActive.BooleanValue.Value)
                    {
                        code = 1;
                    }
                    else if (signals.HeaveCompensationActive.BooleanValue.Value && !signals.HeaveCompensationInactive.BooleanValue.Value)
                    {
                        code = 2;
                    }
                    else if (!signals.HeaveCompensationActive.BooleanValue.Value && !signals.HeaveCompensationInactive.BooleanValue.Value)
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HeaveCompensation, code);
                    if (probMicroStates.HeaveCompensation != null && probMicroStates.HeaveCompensation.Probabilities != null && probMicroStates.HeaveCompensation.Probabilities.Length == 3 &&
                        signals.HeaveCompensationInactive.Probability != null && signals.HeaveCompensationActive.Probability != null)
                    {
                        probMicroStates.HeaveCompensation.Probabilities[0] = signals.HeaveCompensationInactive.Probability.Value * (1 - signals.HeaveCompensationActive.Probability.Value);
                        probMicroStates.HeaveCompensation.Probabilities[1] = signals.HeaveCompensationActive.Probability.Value * (1 - signals.HeaveCompensationInactive.Probability.Value);
                        probMicroStates.HeaveCompensation.Probabilities[2] = (1 - signals.HeaveCompensationActive.Probability.Value) * (1 - signals.HeaveCompensationInactive.Probability.Value);
                    }
                }
                if (signals.PowerHFTO != null &&
                    signals.PowerHFTO.Mean != null &&
                    thresholds.PowerHFTOThreshold != null &&
                    thresholds.PowerHFTOThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.GT(System.Math.Abs(signals.PowerHFTO.Mean.Value), thresholds.PowerHFTOThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HFTO, code);
                    if (probMicroStates.HFTO != null)
                    {
                        probMicroStates.HFTO.Probability = signals.PowerHFTO.ProbabilityLT(thresholds.PowerHFTOThreshold.ScalarValue.Value);
                    }
                }
                if (signals.PeakToPeakAxialOscillations != null &&
                    signals.PeakToPeakAxialOscillations.Mean != null &&
                    thresholds.PeakToPeakAxialOscillationsThreshold != null &&
                    thresholds.PeakToPeakAxialOscillationsThreshold.ScalarValue != null &&
                    signals.AxialStickDuration != null &&
                    signals.AxialStickDuration.Mean != null &&
                    thresholds.AxialStickDurationThreshold != null &&
                    thresholds.AxialStickDurationThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(System.Math.Abs(signals.PeakToPeakAxialOscillations.Mean.Value), thresholds.PeakToPeakAxialOscillationsThreshold.ScalarValue.Value) &&
                        Numeric.LE(System.Math.Abs(signals.AxialStickDuration.Mean.Value), thresholds.AxialStickDurationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(System.Math.Abs(signals.PeakToPeakAxialOscillations.Mean.Value), thresholds.PeakToPeakAxialOscillationsThreshold.ScalarValue.Value) &&
                             Numeric.LE(System.Math.Abs(signals.AxialStickDuration.Mean.Value), thresholds.AxialStickDurationThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialOscillations, code);
                    if (probMicroStates.AxialOscillations != null && probMicroStates.AxialOscillations.Probabilities != null && probMicroStates.AxialOscillations.Probabilities.Length >= 3)
                    {
                        double? prob1 = signals.PeakToPeakAxialOscillations.ProbabilityLT(thresholds.PeakToPeakAxialOscillationsThreshold.ScalarValue.Value);
                        double? prob2 = signals.AxialStickDuration.ProbabilityLT(thresholds.AxialStickDurationThreshold.ScalarValue.Value);
                        if (prob1 != null && prob2 != null)
                        {
                            double prob3 = prob1.Value * prob2.Value;
                            double prob4 = (1.0-prob1.Value) * prob2.Value;
                            probMicroStates.AxialOscillations.Probabilities[0] = prob3;
                            probMicroStates.AxialOscillations.Probabilities[1] = prob4;
                            probMicroStates.AxialOscillations.Probabilities[2] = 1.0 - probMicroStates.AxialOscillations.Probabilities[0];
                        }
                    }
                }
                if (signals.PeakToPeakTorsionalOscillations != null &&
                    signals.PeakToPeakTorsionalOscillations.Mean != null &&
                    thresholds.PeakToPeakTorsionalOscillationsThreshold != null &&
                    thresholds.PeakToPeakTorsionalOscillationsThreshold.ScalarValue != null &&
                    signals.TorsionalStickDuration != null &&
                    signals.TorsionalStickDuration.Mean != null &&
                    thresholds.TorsionalStickDurationThreshold != null &&
                    thresholds.TorsionalStickDurationThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(System.Math.Abs(signals.PeakToPeakTorsionalOscillations.Mean.Value), thresholds.PeakToPeakTorsionalOscillationsThreshold.ScalarValue.Value) &&
                        Numeric.LE(System.Math.Abs(signals.TorsionalStickDuration.Mean.Value), thresholds.TorsionalStickDurationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(System.Math.Abs(signals.PeakToPeakTorsionalOscillations.Mean.Value), thresholds.PeakToPeakTorsionalOscillationsThreshold.ScalarValue.Value) &&
                             Numeric.LE(System.Math.Abs(signals.TorsionalStickDuration.Mean.Value), thresholds.TorsionalStickDurationThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TorsionalOscillations, code);
                    if (probMicroStates.TorsionalOscillations != null && probMicroStates.TorsionalOscillations.Probabilities != null && probMicroStates.TorsionalOscillations.Probabilities.Length >= 3)
                    {
                        double? prob1 = signals.PeakToPeakTorsionalOscillations.ProbabilityLT(thresholds.PeakToPeakTorsionalOscillationsThreshold.ScalarValue.Value);
                        double? prob2 = signals.TorsionalStickDuration.ProbabilityLT(thresholds.TorsionalStickDurationThreshold.ScalarValue.Value);
                        if (prob1 != null && prob2 != null)
                        {
                            double prob3 = prob1.Value * prob2.Value;
                            double prob4 = (1.0 - prob1.Value) * prob2.Value;
                            probMicroStates.TorsionalOscillations.Probabilities[0] = prob3;
                            probMicroStates.TorsionalOscillations.Probabilities[1] = prob4;
                            probMicroStates.TorsionalOscillations.Probabilities[2] = 1.0 - probMicroStates.TorsionalOscillations.Probabilities[0];
                        }
                    }
                }
                if (signals.LateralShockRateBHA != null &&
                    signals.LateralShockRateBHA.Mean != null &&
                    thresholds.LateralShockRateBHAThreshold != null &&
                    thresholds.LateralShockRateBHAThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.LateralShockRateBHA.Mean.Value, thresholds.LateralShockRateBHAThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LateralShocksInBHA, code);
                    if (probMicroStates.LateralShocksInBHA != null)
                    {
                        probMicroStates.LateralShocksInBHA.Probability = signals.LateralShockRateBHA.ProbabilityGT(thresholds.LateralShockRateBHAThreshold.ScalarValue);
                    }
                }
                if (signals.LateralShockRateDrillString != null &&
                    signals.LateralShockRateDrillString.Mean != null &&
                    thresholds.LateralShockRateDrillStringThreshold != null &&
                    thresholds.LateralShockRateDrillStringThreshold.ScalarValue != null)
                {
                    uint code;
                    if (Numeric.LE(signals.LateralShockRateDrillString.Mean.Value, thresholds.LateralShockRateDrillStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LateralShocksInDrillString, code);
                    if (probMicroStates.LateralShocksInDrillString != null)
                    {
                        probMicroStates.LateralShocksInDrillString.Probability = signals.LateralShockRateDrillString.ProbabilityGT(thresholds.LateralShockRateDrillStringThreshold.ScalarValue);
                    }
                }
                if (signals.CurvilinearAbscissaBottomOfString != null &&
                    signals.CurvilinearAbscissaBottomOfString.Mean != null &&
                    signals.CurvilinearAbscissaBottomOfHole != null &&
                    signals.CurvilinearAbscissaBottomOfHole.Mean != null &&
                    signals.CurvilinearAbscissaHoleOpener != null &&
                    signals.CurvilinearAbscissaHoleOpener.Mean != null &&
                    signals.CurvilinearAbscissaTopOfRatHole != null &&
                    signals.CurvilinearAbscissaTopOfRatHole.Mean != null &&
                    signals.ToolJoint1Height != null &&
                    signals.ToolJoint1Height.Mean != null &&
                    signals.MinDrillHeight != null &&
                    signals.MinDrillHeight.ScalarValue != null) 
                {
                    uint code;
                    if (Numeric.GT(Math.Abs(signals.CurvilinearAbscissaBottomOfString.Mean.Value- signals.CurvilinearAbscissaBottomOfHole.Mean.Value), signals.ToolJoint1Height.Mean.Value - signals.MinDrillHeight.ScalarValue.Value) &&
                        Numeric.GT(Math.Abs(signals.CurvilinearAbscissaHoleOpener.Mean.Value - signals.CurvilinearAbscissaTopOfRatHole.Mean.Value), signals.ToolJoint1Height.Mean.Value - signals.MinDrillHeight.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LastStandToBottomHole, code);
                    if (probMicroStates.LastStandToBottomHole != null &&
                        signals.CurvilinearAbscissaBottomOfString.StandardDeviation != null &&
                        signals.CurvilinearAbscissaBottomOfHole.StandardDeviation != null &&
                        signals.CurvilinearAbscissaHoleOpener.StandardDeviation != null &&
                        signals.CurvilinearAbscissaTopOfRatHole.StandardDeviation != null &&
                        signals.ToolJoint1Height.StandardDeviation != null)
                    {
                        double mean1 = signals.CurvilinearAbscissaBottomOfString.Mean.Value - signals.CurvilinearAbscissaBottomOfHole.Mean.Value;
                        double stdDev1 = 
                            Math.Sqrt(signals.CurvilinearAbscissaBottomOfString.StandardDeviation.Value * signals.CurvilinearAbscissaBottomOfString.StandardDeviation.Value +
                                      signals.CurvilinearAbscissaBottomOfHole.StandardDeviation.Value * signals.CurvilinearAbscissaBottomOfHole.StandardDeviation.Value);
                        double mean2 = signals.ToolJoint1Height.Mean.Value - signals.MinDrillHeight.ScalarValue.Value;
                        double stdDev2 = signals.ToolJoint1Height.StandardDeviation.Value;
                        double mean3 = signals.CurvilinearAbscissaHoleOpener.Mean.Value - signals.CurvilinearAbscissaTopOfRatHole.Mean.Value;
                        double stdDev3 = 
                            Math.Sqrt(signals.CurvilinearAbscissaHoleOpener.StandardDeviation.Value * signals.CurvilinearAbscissaHoleOpener.StandardDeviation.Value +
                                      signals.CurvilinearAbscissaTopOfRatHole.StandardDeviation.Value * signals.CurvilinearAbscissaTopOfRatHole.StandardDeviation.Value);
                        GaussianDrillingProperty dumm1 = new GaussianDrillingProperty() { Mean = mean1, StandardDeviation = stdDev1 };
                        GaussianDrillingProperty dumm2 = new GaussianDrillingProperty() { Mean = mean2, StandardDeviation = stdDev2 };
                        GaussianDrillingProperty dumm3 = new GaussianDrillingProperty() { Mean = mean3, StandardDeviation = stdDev3 };
                        double? prob1 = dumm1.ProbabilityGT(dumm2);
                        double? prob2 = dumm3.ProbabilityGT(dumm2);
                        probMicroStates.LastStandToBottomHole.Probability = 1.0 - prob1 * prob2;
                    }
                }
            }
            _logger.LogInformation("processed data");
            bool changed = false;
            lock (lock_)
            {
                changed |= currentDeterministicMicroStates_.Part1 != microStates.Part1;
                changed |= currentDeterministicMicroStates_.Part2 != microStates.Part2;
                changed |= currentDeterministicMicroStates_.Part3 != microStates.Part3;
                changed |= currentDeterministicMicroStates_.Part4 != microStates.Part4;
                changed |= currentDeterministicMicroStates_.Part5 != microStates.Part5;
                if (changed)
                {
                    currentDeterministicMicroStates_.Part1 = microStates.Part1;
                    currentDeterministicMicroStates_.Part2 = microStates.Part2;
                    currentDeterministicMicroStates_.Part3 = microStates.Part3;
                    currentDeterministicMicroStates_.Part4 = microStates.Part4;
                    currentDeterministicMicroStates_.Part5 = microStates.Part5;
                }
            }
            if (changed)
            {
                _logger.LogInformation("deterministic microstate has changed");
                microStates.TimeStampUTC = DateTime.UtcNow;
                if (DWISClient_ != null && deterministicMicroStatePlaceHolder_ != null)
                {
                    currentDeterministicMicroStates_.SendToDDHub(DWISClient_, deterministicMicroStatePlaceHolder_);
                }
            }
            changed = false;
            lock (lock_)
            {
                changed = !probMicroStates.Equals(currentProbabilisticMicroStates_);
                if (changed)
                {
                    probMicroStates.CopyTo(currentProbabilisticMicroStates_);
                }
            }
            if (changed)
            {
                _logger.LogInformation("probabilistic microstate has changed");
                probMicroStates.TimeStampUTC = DateTime.UtcNow;
                if (DWISClient_ != null)
                {
                    probMicroStates.SendToDDHub(DWISClient_, probabilisticMicroStatePlaceHolders_);
                }
            }
        }

        private void UpdateMicroState(ref MicroStates microState, MicroStates.MicroStateIndex index, uint code)
        {
            // which part
            int part = (int)index / 16;
            int pos = 2 * ((int)index % 16);
            code = code << pos;
            uint mask = 3;
            mask = mask << pos;
            mask = ~mask;
            switch (part)
            {
                case 0:
                    microState.Part1 = (int)((uint)microState.Part1 & mask);
                    microState.Part1 = (int)((uint)microState.Part1 | code);
                    break;
                case 1:
                    microState.Part2 = (int)((uint)microState.Part2 & mask);
                    microState.Part2 = (int)((uint)microState.Part2 | code);
                    break;
                case 2:
                    microState.Part3 = (int)((uint)microState.Part3 & mask);
                    microState.Part3 = (int)((uint)microState.Part3 | code);
                    break;
                case 3:
                    microState.Part4 = (int)((uint)microState.Part4 & mask);
                    microState.Part4 = (int)((uint)microState.Part4 | code);
                    break;
                default:
                    microState.Part5 = (int)((uint)microState.Part5 & mask);
                    microState.Part5 = (int)((uint)microState.Part5 | code);
                    break;
            }

        }
    }
}