using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.General.Common;
using DWIS.Client.ReferenceImplementation;
using DWIS.API.DTO;
using DWIS.SPARQL.Utils;
using DWIS.Vocabulary.Schemas;
using System.ComponentModel;

namespace DWIS.MicroState.IntepretationEngine
{
    public class Worker : BackgroundService
    {
        private static readonly string microStatesThresholdsQueryName_ = "DWIS:MicroStates:ThresholdsQuery";
        private static readonly string microStatesInputSignalsQueryName_ = "DWIS:MicroStates:InputSignalsQuery";
        private Configuration Configuration { get; set; } = new Configuration();
        private readonly ILogger<Worker> _logger;
        public IMqttClient mqttClient_;
        private Thresholds? thresholds_ = null;
        private List<string> subscribedSignalTopic_ = new List<string>();
        private MicroStates currentMicroStates_ = new MicroStates();

        private IOPCUADWISClient? DDHubClient_ = null;
        private QueryResult? microStateInDDHub_ = null;
        private AcquiredSignals? microStateThresholdsSignals_ = null;
        private AcquiredSignals? microStateInputSignals_ = null;

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

        private async Task ConnectToDDHub()
        {
            try
            {
                if (Configuration != null && !string.IsNullOrEmpty(Configuration.OPCUAURL))
                {
                    DefaultDWISClientConfiguration defaultDWISClientConfiguration = new DefaultDWISClientConfiguration();
                    defaultDWISClientConfiguration.UseWebAPI = false;
                    defaultDWISClientConfiguration.ServerAddress = Configuration.OPCUAURL; // "opc.tcp://localhost:48030";
                    DDHubClient_ = null; // new DWISClient(defaultDWISClientConfiguration, new UAApplicationConfiguration(), null, null, new DWIS.OPCUA.UALicenseManager.LicenseManager());
                }
            }
            catch (Exception e)
            {

            }
        }

        private void DefineMicroStateSemantic()
        {
            if (DDHubClient_ != null && DDHubClient_.Connected)
            {
                string processState = "?processState";
                string dwis = "?dwis";
                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.AddSelectedVariable(QueryBuilder.SIGNAL_VARIABLE);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, Verbs.IsGeneratedBy, processState);
                queryBuilder.AddPatternItem(processState, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.ProcessState);
                queryBuilder.AddPatternItem(processState, Verbs.IsProvidedBy, dwis);
                queryBuilder.AddPatternItem(dwis, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.DWISInternalService);
                string query = queryBuilder.Build();
                var result = DDHubClient_.GetQueryResult(query);
                if (result == null || result.Results == null || result.Results.Count <= 0)
                {
                    ManifestFile manifestFile = new ManifestFile()
                    {
                        InjectedNodes = new List<InjectedNode>(),
                        InjectedReferences = new List<InjectedReference>(),
                        InjectedVariables = new List<InjectedVariable>(),
                        InjectionInformation = new InjectionInformation()
                        {
                            EndPointURL = "",
                            InjectedNodesNamespaceAlias = "nodes",
                            InjectedVariablesNamespaceAlias = "variables",
                            ProvidedVariablesNamespaceAlias = "providedNodes",
                            ServerName = "sourceserver"
                        },
                        ProvidedVariables = new List<ProvidedVariable>(),
                        ManifestName = "MicroStateSignalDeclarations",
                        Provider = new InjectionProvider()
                        {
                            Company = "NORCE",
                            Name = "MicroStatesSignalDeclarations"
                        }
                    };

                    string ddhubURL = "http://ddhub.no/";
                    string microStateSignal = "DWIS:MicroStates:Current";
                    string opcUAMicroStateSignal = "8796c92c-96a3-4854-a263-3a6aa67344bf";
                    string processStateNode = "DWIS:MicroStates:ProcessState";
                    string DWISInternalServiceNode = "DWIS:Provider:DWISInternalService";

                    ProvidedVariable providedVariable = new ProvidedVariable() { DataType = "string", VariableID = opcUAMicroStateSignal };
                    manifestFile.ProvidedVariables.Add(providedVariable);
                    InjectedNode injectedNode = new InjectedNode()
                    {
                        BrowseName = microStateSignal,
                        DisplayName = microStateSignal,
                        UniqueName = microStateSignal,
                        TypeDictionaryURI = Nouns.DrillingDataPoint
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    injectedNode = new InjectedNode()
                    {
                        BrowseName = processStateNode,
                        DisplayName = processStateNode,
                        UniqueName = processStateNode,
                        TypeDictionaryURI = Nouns.ProcessState,
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    injectedNode = new InjectedNode()
                    {
                        BrowseName = DWISInternalServiceNode,
                        DisplayName = DWISInternalServiceNode,
                        UniqueName = DWISInternalServiceNode,
                        TypeDictionaryURI = Nouns.DWISInternalService,
                        Fields = new List<Field>() {
                                              new Field() { FieldName = DWIS.Vocabulary.Schemas.Attributes.DataProvider_ProviderName, FieldValue = "DWIS" }
                                           }
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = microStateSignal },
                        VerbURI = ddhubURL + Verbs.HasDynamicValue,
                        Object = new NodeIdentifier() { NameSpace = "providedNodes", ID = opcUAMicroStateSignal }

                    }
                    );
                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = microStateSignal },
                        VerbURI = ddhubURL + Verbs.IsGeneratedBy,
                        Object = new NodeIdentifier() { NameSpace = "nodes", ID = processStateNode }
                    }
                    );
                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = processStateNode },
                        VerbURI = ddhubURL + Verbs.IsProvidedBy,
                        Object = new NodeIdentifier() { NameSpace = "nodes", ID = DWISInternalServiceNode }
                    }
                    );

                    var res = DDHubClient_.Inject(manifestFile);
                    if (res == null || !res.Success)
                    {
                        return;

                    }
                }
                result = DDHubClient_.GetQueryResult(query);
                if (result != null && result.Results != null && result.Results.Count > 0)
                {
                    microStateInDDHub_ = DDHubClient_.GetQueryResult(query);
                }
            }
        }
        private void AcquireMicroStatesThresholds()
        {
            if (DDHubClient_ != null && DDHubClient_.Connected)
            {
                string processState = "?processState";
                string dwis = "?dwis";
                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.AddSelectedVariable(QueryBuilder.SIGNAL_VARIABLE);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, Verbs.IsComputationInput, processState);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.Limit);
                queryBuilder.AddPatternItem(processState, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.ProcessState);
                queryBuilder.AddPatternItem(processState, Verbs.IsProvidedBy, dwis);
                queryBuilder.AddPatternItem(dwis, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.DWISInternalService);
                string query = queryBuilder.Build();

                var result = DDHubClient_.GetQueryResult(query);
                if (result == null || result.Results == null || result.Results.Count <= 0)
                {
                    ManifestFile manifestFile = new ManifestFile()
                    {
                        InjectedNodes = new List<InjectedNode>(),
                        InjectedReferences = new List<InjectedReference>(),
                        InjectedVariables = new List<InjectedVariable>(),
                        InjectionInformation = new InjectionInformation()
                        {
                            EndPointURL = "",
                            InjectedNodesNamespaceAlias = "nodes",
                            InjectedVariablesNamespaceAlias = "variables",
                            ProvidedVariablesNamespaceAlias = "providedNodes",
                            ServerName = "sourceserver"
                        },
                        ProvidedVariables = new List<ProvidedVariable>(),
                        ManifestName = "MicroStateThresholdSignalDeclarations",
                        Provider = new InjectionProvider()
                        {
                            Company = "NORCE",
                            Name = "MicroStateThresholdSignalDeclarations"
                        }
                    };

                    string ddhubURL = "http://ddhub.no/";
                    string microStatesThresholdsSignal = "DWIS:MicroStates:Thresholds";
                    string opcUAMicroStatesThresholdsSignal = "25b3ebfd-b306-487d-9685-2da384ce2bd3";
                    string processStateNode = "DWIS:MicroStates:ProcessState";
                    string DWISInternalServiceNode = "DWIS:Provider:DWISInternalService";

                    ProvidedVariable providedVariable = new ProvidedVariable() { DataType = "string", VariableID = opcUAMicroStatesThresholdsSignal };
                    manifestFile.ProvidedVariables.Add(providedVariable);
                    InjectedNode injectedNode = new InjectedNode()
                    {
                        BrowseName = microStatesThresholdsSignal,
                        DisplayName = microStatesThresholdsSignal,
                        UniqueName = microStatesThresholdsSignal,
                        TypeDictionaryURI = Nouns.Limit
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    injectedNode = new InjectedNode()
                    {
                        BrowseName = processStateNode,
                        DisplayName = processStateNode,
                        UniqueName = processStateNode,
                        TypeDictionaryURI = Nouns.ProcessState,
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    injectedNode = new InjectedNode()
                    {
                        BrowseName = DWISInternalServiceNode,
                        DisplayName = DWISInternalServiceNode,
                        UniqueName = DWISInternalServiceNode,
                        TypeDictionaryURI = Nouns.DWISInternalService,
                        Fields = new List<Field>() {
                                             new Field() { FieldName = DWIS.Vocabulary.Schemas.Attributes.DataProvider_ProviderName, FieldValue = "DWIS" }
                                           }
                    };
                    manifestFile.InjectedNodes.Add(injectedNode);

                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = microStatesThresholdsSignal },
                        VerbURI = ddhubURL + Verbs.HasDynamicValue,
                        Object = new NodeIdentifier() { NameSpace = "providedNodes", ID = opcUAMicroStatesThresholdsSignal }

                    }
                    );
                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = microStatesThresholdsSignal },
                        VerbURI = ddhubURL + Verbs.IsComputationInput,
                        Object = new NodeIdentifier() { NameSpace = "nodes", ID = processStateNode }
                    }
                    );
                    manifestFile.InjectedReferences.Add(new InjectedReference()
                    {
                        Subject = new NodeIdentifier() { NameSpace = "nodes", ID = processStateNode },
                        VerbURI = ddhubURL + Verbs.IsProvidedBy,
                        Object = new NodeIdentifier() { NameSpace = "nodes", ID = DWISInternalServiceNode }
                    }
                    );

                    var res = DDHubClient_.Inject(manifestFile);
                    if (res == null || !res.Success)
                    {
                        _logger.LogError("Failed to inject manifest");
                    }
                    result = DDHubClient_.GetQueryResult(query);
                    if (result != null && result.Results != null && result.Results.Count > 0)
                    {
                        QueryResultRow row = result.Results[0];
                        if (row != null && row.Count > 0)
                        {
                            Thresholds microStateThresholds = new Thresholds();
                            microStateThresholds.TimeStampUTC = DateTime.UtcNow;
                            microStateThresholds.StableAxialVelocityTopOfStringThreshold.ScalarValue = 0.5 / 3600.0;
                            microStateThresholds.StableRotationalVelocityTopOfStringThreshold.ScalarValue = 0.5 / 60.0;
                            microStateThresholds.StableFlowTopOfStringThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.StableTensionTopOfStringThreshold.ScalarValue = 1000.0;
                            microStateThresholds.AtmosphericPressureThreshold.ScalarValue = OSDC.DotnetLibraries.General.Common.Constants.EarthStandardAtmosphericPressure / 10.0;
                            microStateThresholds.StablePressureTopOfStringThreshold.ScalarValue = 0.1 * 1e5;
                            microStateThresholds.StableTorqueTopOfStringThreshold.ScalarValue = 10.0;
                            microStateThresholds.StableFlowAnnulusOutletThreshold.ScalarValue = 1.0 / 60000.0;
                            microStateThresholds.StableBottomOfStringRockForceThreshold.ScalarValue = 1000.0;
                            microStateThresholds.StableRotationalVelocityBottomOfStringThreshold.ScalarValue = 1.0 / 60.0;
                            microStateThresholds.StableAxialVelocityBottomOfStringThreshold.ScalarValue = 0.1 / 3600.0;
                            microStateThresholds.StableFlowBottomOfStringThreshold.ScalarValue = 1.0 / 60000.0;
                            microStateThresholds.StableFlowHoleOpenerThreshold.ScalarValue = 1.0 / 60000.0;
                            microStateThresholds.MinimumTensionTopOfString.ScalarValue = 10000.0;
                            microStateThresholds.MinimumPressureFloatValve.ScalarValue = 1e5;
                            microStateThresholds.StableFlowBoosterPumpThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.StableFlowBackPressurePumpThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.MinimumDifferentialPressureRCDSealingThreshold.ScalarValue = 1e5;
                            microStateThresholds.MinimumDifferentialPressureSealBalanceThreshold.ScalarValue = 1e5;
                            microStateThresholds.StableFlowFillPumpDGDThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.StableFlowLiftPumpDGDThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.StableCuttingsFlowThreshold.ScalarValue = 1.0 / 60000.0;
                            microStateThresholds.HardStringerThreshold.ScalarValue = 60e6;
                            microStateThresholds.ChangeOfFormationUCSSlopeThreshold.ScalarValue = 2.5e6;
                            microStateThresholds.ForceOnLedgeThreshold.ScalarValue = 10000;
                            microStateThresholds.ForceOnCuttingsBedThreshold.ScalarValue = 10000;
                            microStateThresholds.ForceDifferentialStickingThreshold.ScalarValue = 10000;
                            microStateThresholds.FluidFlowFormationThreshold.ScalarValue = 10.0 / 60000.0;
                            microStateThresholds.WhirlRateThreshold.ScalarValue = 0.5;
                            microStateThresholds.FlowPipeToAnnulusThreshold.ScalarValue = 0.5 / 60000.0;
                            microStateThresholds.FlowCavingsFromFormationThreshold.ScalarValue = 0.5 / 60000.0;
                            microStateThresholds.AtStickUpHeightThreshold.ScalarValue = 0.1;
                            microStateThresholds.AtDrillHeightThreshold.ScalarValue = 0.1;

                            // serialize to Json and push to DDHub
                            try
                            {
                                string json = JsonConvert.SerializeObject(microStateThresholds);
                                if (!string.IsNullOrEmpty(json))
                                {
                                    (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                                    outputs[0].nameSpace = row[0].NameSpace;
                                    outputs[0].id = row[0].ID;
                                    outputs[0].value = json;
                                    outputs[0].sourceTimestamp = DateTime.UtcNow;
                                    DDHubClient_.UpdateAnyVariables(outputs);
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
                result = DDHubClient_.GetQueryResult(query);
                if (result != null && result.Results != null && result.Results.Count > 0)
                {
                    microStateThresholdsSignals_ = AcquiredSignals.CreateWithSubscription(new string[] { query }, new string[] { microStatesThresholdsQueryName_ }, 0, DDHubClient_);
                }
            }
        }

        private void AcquireSignalInputs()
        {
            if (DDHubClient_ != null && DDHubClient_.Connected)
            {
                // build query
                string computationUnit = "?computationUnit";
                string processState = "?processState";
                string dwis = "?dwis";
                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.AddSelectedVariable(QueryBuilder.SIGNAL_VARIABLE);
                queryBuilder.AddSelectedVariable(computationUnit);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, Verbs.IsComputedBy, computationUnit);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.ComputedData);
                queryBuilder.AddPatternItem(computationUnit, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.ComputationUnit);
                queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, Verbs.IsComputationInput, processState);
                queryBuilder.AddPatternItem(processState, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.ProcessState);
                queryBuilder.AddPatternItem(processState, Verbs.IsProvidedBy, dwis);
                queryBuilder.AddPatternItem(computationUnit, Verbs.IsProvidedBy, dwis);
                queryBuilder.AddPatternItem(dwis, QueryBuilder.RDFTYPE, "ddhub:" + Nouns.DWISInternalService);
                string query = queryBuilder.Build();

                var result = DDHubClient_.GetQueryResult(query);
                if (result != null && result.Results != null && result.Results.Count > 0)
                {
                    microStateInputSignals_ = AcquiredSignals.CreateWithSubscription(new string[] { query }, new string[] { microStatesInputSignalsQueryName_ }, 0, DDHubClient_);
                }
            }
        }
        private void RefreshThresholds()
        {
            if (microStateThresholdsSignals_ != null && microStateThresholdsSignals_.ContainsKey(microStatesThresholdsQueryName_))
            {
                List<AcquiredSignal> signals = microStateThresholdsSignals_[microStatesThresholdsQueryName_];
                if (signals != null && signals.Count > 0)
                {
                    AcquiredSignal signal = signals[0];
                    if (signal != null)
                    {
                        string? json = signal.GetValue<string>();
                        if (!string.IsNullOrEmpty(json))
                        {
                            try
                            {
                                Thresholds? data = JsonConvert.DeserializeObject<Thresholds>(json);
                                if (data != null)
                                {
                                    lock (lock_)
                                    {
                                        if (thresholds_ == null)
                                        {
                                            thresholds_ = new Thresholds();
                                        }
                                        data.CopyTo(thresholds_);
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectToDDHub();
            DefineMicroStateSemantic();
            AcquireMicroStatesThresholds();
            AcquireSignalInputs();
            await ConnectAndSubscribeEmitterAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime d1 = DateTime.UtcNow;
                RefreshThresholds();
                await RefreshSignals();
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
            await mqttClient_.DisconnectAsync();
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

                        }
                    }
                }
            }
            _logger.LogInformation("Configuration MQTT Server: " + Configuration.MQTTServerName);
            _logger.LogInformation("Configuration MQTT Port: " + Configuration.MQTTServerPort);
            _logger.LogInformation("Configuration Loop Duration: " + Configuration.LoopDuration.ToString());
            _logger.LogInformation("Configuration OPCUAURAL: " + Configuration.OPCUAURL);
            string hostName = System.Net.Dns.GetHostName();
            if (!string.IsNullOrEmpty(hostName))
            {
                var ip = System.Net.Dns.GetHostByName(hostName);
                if (ip != null && ip.AddressList != null && ip.AddressList.Length > 0)
                {
                    _logger.LogInformation("My IP Address: " + ip.AddressList[0].ToString());
                }
            }
        }
        private async Task ConnectAndSubscribeEmitterAsync()
        {
            // Configure MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(Configuration.MQTTServerName, Configuration.MQTTServerPort) // Replace with your MQTT broker details
                .Build();

            // Create MQTT client
            mqttClient_ = new MqttFactory().CreateMqttClient();

            // Connect to the broker
            await mqttClient_.ConnectAsync(options);
        }

        private async Task RefreshSignals()
        {
            MicroStates microStates = new MicroStates();
            SignalGroup signals = null;
            if (microStateInputSignals_ != null && microStateInputSignals_.ContainsKey(microStatesInputSignalsQueryName_))
            {
                List<AcquiredSignal> sigs = microStateInputSignals_[microStatesInputSignalsQueryName_];
                if (sigs != null && sigs.Count > 0)
                {
                    AcquiredSignal signal = sigs[0];
                    if (signal != null)
                    {
                        string? json = signal.GetValue<string>();
                        if (!string.IsNullOrEmpty(json))
                        {
                            try
                            {
                                SignalGroup? data = JsonConvert.DeserializeObject<SignalGroup>(json);
                                if (data != null)
                                {
                                    signals = data;
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e.ToString());
                            }
                        }
                    }
                }
            }
            bool? insideHardStringer = null;
            int? changeOfFormation = null;
            if (signals != null && thresholds_ != null)
            {
                if (signals.AxialVelocityTopOfString?.Mean != null &&
                    thresholds_.StableAxialVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.AxialVelocityTopOfString.Mean, 0, thresholds_.StableAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityTopOfString.Mean, 0, thresholds_.StableAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityTopOfString, code);
                }
                if (signals.StandardDeviationAxialVelocityTopOfString?.Mean != null &&
                    thresholds_.StableAxialVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityTopOfString.Mean, thresholds_.StableAxialVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityTopOfString, code);
                }
                if (signals.RotationalVelocityTopOfString?.Mean != null &&
                    thresholds_.StableRotationalVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.RotationalVelocityTopOfString.Mean, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationalVelocityTopOfString.Mean, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityTopOfString, code);
                }
                if (signals.StandardDeviationRotationalVelocityTopOfString?.Mean != null &&
                    thresholds_.StableRotationalVelocityTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityTopOfString.Mean, thresholds_.StableRotationalVelocityTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityTopOfString, code);
                }
                if (signals.FlowTopOfString?.Mean != null &&
                    thresholds_.StableFlowTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowTopOfString.Mean, 0, thresholds_.StableFlowTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtTopOfString, code);
                }
                if (signals.StandardDeviationFlowTopOfString?.Mean != null &&
                    thresholds_.StableFlowTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowTopOfString.Mean, thresholds_.StableFlowTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtTopOfString, code);
                }
                if (signals.TensionTopOfString?.Mean != null &&
                    signals.ForceBottomTopDrive?.Mean != null &&
                    signals.ForceElevator?.Mean != null &&
                    thresholds_.StableTensionTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.TensionTopOfString.Mean, signals.ForceBottomTopDrive.Mean, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value) || 
                        Numeric.EQ(signals.TensionTopOfString.Mean, signals.ForceElevator.Mean, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (!Numeric.EQ(signals.TensionTopOfString.Mean, 0, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value) && 
                             Numeric.EQ(signals.ForceBottomTopDrive.Mean, 0, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value) &&
                             Numeric.EQ(signals.ForceElevator.Mean, 0, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.SlipState, code);
                }
                if (signals.StandardDeviationTensionTopOfString?.Mean != null &&
                    thresholds_.StableTensionTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationTensionTopOfString.Mean, thresholds_.StableTensionTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTensionTopOfString, code);
                }
                if (signals.PressureTopOfString?.Mean != null &&
                    thresholds_.StablePressureTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.LE(signals.PressureTopOfString.Mean, Constants.EarthStandardAtmosphericPressure, thresholds_.StablePressureTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PressureTopOfString, code);
                }
                if (signals.StandardDeviationPressureTopOfString?.Mean != null &&
                    thresholds_.StablePressureTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationPressureTopOfString.Mean, thresholds_.StablePressureTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StablePressureTopOfString, code);
                }
                if (signals.TorqueTopOfString?.Mean != null &&
                    thresholds_.StableTorqueTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.TorqueTopOfString.Mean, 0, thresholds_.StableTorqueTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TorqueTopOfString, code);
                }
                if (signals.StandardDeviationTorqueTopOfString?.Mean != null &&
                    thresholds_.StableTorqueTopOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationTorqueTopOfString.Mean, thresholds_.StableTorqueTopOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTorqueTopOfString, code);
                }
                if (signals.FlowAnnulusOutlet?.Mean != null &&
                    thresholds_.StableFlowAnnulusOutletThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowAnnulusOutlet.Mean, 0, thresholds_.StableFlowAnnulusOutletThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtAnnulusOutlet, code);
                }
                if (signals.StandardDeviationFlowAnnulusOutlet?.Mean != null &&
                    thresholds_.StableFlowAnnulusOutletThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowAnnulusOutlet?.Mean, thresholds_.StableFlowAnnulusOutletThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtAnnulusOutlet, code);
                }
                if (signals.FlowCuttingsAnnulusOutlet?.Mean != null &&
                    thresholds_.StableCuttingsFlowThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsAnnulusOutlet.Mean, 0, thresholds_.StableCuttingsFlowThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsReturnAtAnnulusOutlet, code);
                }
                if (signals.ForceBottomOfStringOnRock?.Mean != null &&
                    thresholds_.StableBottomOfStringRockForceThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceBottomOfStringOnRock?.Mean, 0, thresholds_.StableBottomOfStringRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomBottomOfString, code);
                }
                if (signals.StandardDeviationForceBottomOfStringOnRock?.Mean != null &&
                    thresholds_.StableBottomOfStringRockForceThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationForceBottomOfStringOnRock?.Mean, thresholds_.StableBottomOfStringRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBottomOfStringRockForce, code);
                }
                if (signals.ForceHoleOpenerOnRock?.Mean != null &&
                    thresholds_.StableBottomOfStringRockForceThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceHoleOpenerOnRock?.Mean, 0, thresholds_.StableBottomOfStringRockForceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomHoleOpener, code);
                }
                if (signals.RotationaVelocityBottomOfString?.Mean != null &&
                    thresholds_.StableRotationalVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.RotationaVelocityBottomOfString.Mean, 0, thresholds_.StableRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationaVelocityBottomOfString.Mean, thresholds_.StableRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityBottomOfString, code);
                }
                if (signals.StandardDeviationRotationalVelocityBottomOfString?.Mean != null &&
                    thresholds_.StableRotationalVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityBottomOfString.Mean, thresholds_.StableRotationalVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityBottomOfString, code);
                }
                if (signals.FlowCuttingsBottomHole?.Mean != null &&
                    thresholds_.StableCuttingsFlowThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsBottomHole.Mean, 0, thresholds_.StableCuttingsFlowThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Drilling, code);
                }
                if (signals.FlowCuttingsTopOfRateHole?.Mean != null && thresholds_.StableCuttingsFlowThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsTopOfRateHole.Mean, 0, thresholds_.StableCuttingsFlowThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HoleOpening, code);
                }
                if (signals.AxialVelocityBottomOfString?.Mean != null && thresholds_.StableAxialVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.AxialVelocityBottomOfString.Mean, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityBottomOfString.Mean, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityBottomOfString, code);
                }
                if (signals.StandardDeviationAxialVelocityBottomOfString?.Mean != null && thresholds_.StableAxialVelocityBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityBottomOfString.Mean, thresholds_.StableAxialVelocityBottomOfStringThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityBottomOfString, code);
                }
                if (signals.FlowBottomOfString?.Mean != null && thresholds_.StableFlowBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBottomOfString.Mean, 0, thresholds_.StableFlowBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowBottomOfString.Mean, 0, thresholds_.StableFlowBottomOfStringThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowBottomOfString, code);
                }
                if (signals.StableFlowBottomOfString?.Mean != null && thresholds_.StableFlowBottomOfStringThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StableFlowBottomOfString.Mean, thresholds_.StableFlowBottomOfStringThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowBottomOfString, code);
                }
                if (signals.FlowHoleOpener?.Mean != null && thresholds_.StableFlowHoleOpenerThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowHoleOpener.Mean, 0, thresholds_.StableFlowHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowHoleOpener.Mean, 0, thresholds_.StableFlowHoleOpenerThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowHoleOpener, code);
                }
                if (signals.StableFlowHoleOpener?.Mean != null && thresholds_.StableFlowHoleOpenerThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StableFlowHoleOpener.Mean, thresholds_.StableFlowHoleOpenerThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowHoleOpener, code);
                }
                if (signals.ForceOnLedge?.Mean != null && thresholds_.ForceOnLedgeThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceOnLedge.Mean, 0, thresholds_.ForceOnLedgeThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnLedge.Mean, 0, thresholds_.ForceOnLedgeThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LedgeKeySeat, code);
                }
                if (signals.ForceOnCuttingsBed?.Mean != null && thresholds_.ForceOnCuttingsBedThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceOnCuttingsBed.Mean, 0, thresholds_.ForceOnCuttingsBedThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnCuttingsBed.Mean, 0, thresholds_.ForceOnCuttingsBedThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsBed, code);
                }
                if (signals.ForceDifferentialSticking?.Mean != null && thresholds_.ForceDifferentialStickingThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceDifferentialSticking.Mean, 0, thresholds_.ForceDifferentialStickingThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.DifferentialSticking, code);
                }
                if (signals.TensionTopOfString?.Mean != null && signals.MinimumTensionForTwistOffDetection?.Mean != null)
                {
                    uint code = 0;
                    if (Numeric.GE(signals.TensionTopOfString.Mean, signals.MinimumTensionForTwistOffDetection.Mean))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TwistOffBackOff, code);
                }
                if (signals.FlowFluidFromOrToFormation?.Mean != null && thresholds_.FluidFlowFormationThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowFluidFromOrToFormation.Mean, 0, thresholds_.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowFluidFromOrToFormation.Mean, 0, thresholds_.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WellIntegrity, code);
                }
                if (signals.FlowFormationFluidAnnulusOutlet?.Mean != null && thresholds_.FluidFlowFormationThreshold?.ScalarValue != null)
                {
                    uint code = 0;

                    if (Numeric.EQ(signals.FlowFormationFluidAnnulusOutlet.Mean, 0, thresholds_.FluidFlowFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationFluidAtAnnulusOutlet, code);
                }
                if (signals.FlowCavingsFromFormation?.Mean != null && thresholds_.FlowCavingsFromFormationThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCavingsFromFormation.Mean, 0, thresholds_.FlowCavingsFromFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationCollapse, code);
                }
                if (signals.FlowCavingsAnnulusOutlet?.Mean != null && thresholds_.FlowCavingsFromFormationThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCavingsAnnulusOutlet.Mean, 0, thresholds_.FlowCavingsFromFormationThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CavingsAtAnnulusOutlet, code);
                }
                if (signals.FlowPipeToAnnulus?.Mean != null && thresholds_.FlowPipeToAnnulusThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowPipeToAnnulus.Mean, 0, thresholds_.FlowPipeToAnnulusThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PipeWashout, code);
                }
                if (signals.WhirlRateBottomOfString?.Mean != null && thresholds_.WhirlRateThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.WhirlRateBottomOfString.Mean, 0, thresholds_.WhirlRateThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateBottomOfString.Mean, 0, thresholds_.WhirlRateThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlBottomOfString, code);
                }
                if (signals.WhirlRateHoleOpener?.Mean != null && thresholds_.WhirlRateThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.WhirlRateHoleOpener.Mean, 0, thresholds_.WhirlRateThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateHoleOpener.Mean, 0, thresholds_.WhirlRateThreshold.ScalarValue.Value))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlHoleOpener, code);
                }
                if (signals.DifferentialPressureFloatValve?.Mean != null && thresholds_.MinimumPressureFloatValve?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.LE(signals.DifferentialPressureFloatValve.Mean, thresholds_.MinimumPressureFloatValve.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FloatSub, code);
                }
                if (signals.UnderReamerOpen != null)
                {
                    uint code = 0;
                    if ((bool)signals.UnderReamerOpen)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.UnderReamer, code);
                }
                if (signals.CirculationSubOpen != null)
                {
                    uint code = 0;
                    if ((bool)signals.CirculationSubOpen)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CirculationSub, code);
                }
                if (signals.PortedFloatOpen != null)
                {
                    uint code = 0;
                    if ((bool)signals.PortedFloatOpen)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PortedFloat, code);
                }
                if (signals.WhipstockAttached != null)
                {
                    uint code = 0;
                    if ((bool)signals.WhipstockAttached)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Whipstock, code);
                }
                if (signals.PlugAttached != null)
                {
                    uint code = 0;
                    if ((bool)signals.PlugAttached)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Plug, code);
                }
                if (signals.LinerAttached != null)
                {
                    uint code = 0;
                    if ((bool)signals.LinerAttached)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Liner, code);
                }
                if (signals.FlowBoosterPump?.Mean != null && thresholds_.StableFlowBoosterPumpThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBoosterPump.Mean, 0, thresholds_.StableFlowBoosterPumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BoosterPumping, code);
                }
                if (signals.StandardDeviationFlowBoosterPump?.Mean != null && thresholds_.StableFlowBoosterPumpThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowBoosterPump.Mean, thresholds_.StableFlowBoosterPumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBoosterPumping, code);
                }
                if (signals.FlowBackPressurePump?.Mean != null && thresholds_.StableFlowBackPressurePumpThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBackPressurePump.Mean, 0, thresholds_.StableFlowBackPressurePumpThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BackPressurePumping, code);
                }
                if (signals.StandardDeviationFlowBackPressurePump?.Mean != null && thresholds_.StableFlowBackPressurePumpThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowBackPressurePump.Mean, thresholds_.StableFlowBackPressurePumpThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBackPressurePumping, code);
                }
                if (signals.OpeningMPDChoke?.Mean != null)
                {
                    uint code = 0;
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
                }
                if (signals.DifferentialPressureRCD?.Mean != null && thresholds_.MinimumDifferentialPressureRCDSealingThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.DifferentialPressureRCD.Mean, 0, thresholds_.MinimumDifferentialPressureRCDSealingThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RCDSealing, code);
                }
                if (signals.IsolationSealActivated != null)
                {
                    uint code = 0;
                    if (!(bool)signals.IsolationSealActivated)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSeal, code);
                }
                if (signals.DifferentialPressureIsolationSeal?.Mean != null && thresholds_.MinimumDifferentialPressureSealBalanceThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.DifferentialPressureIsolationSeal.Mean, 0, thresholds_.MinimumDifferentialPressureSealBalanceThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSealPressureBalance, code);
                }
                if (signals.BearingAssemblyLatched != null)
                {
                    uint code = 0;
                    if (!(bool)signals.BearingAssemblyLatched)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BearingAssemblyLatched, code);
                }
                if (signals.ScreenMPDChokePlugged != null)
                {
                    uint code = 0;
                    if ((bool)signals.ScreenMPDChokePlugged)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ScreenMPDChokePlugged, code);
                }
                if (signals.MainFlowPathMPDEstablished != null)
                {
                    uint code = 0;
                    if (!(bool)signals.MainFlowPathMPDEstablished)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MainFlowPathStable, code);
                }
                if (signals.AlternateFlowPathMPDEstablished != null)
                {
                    uint code = 0;
                    if (!(bool)signals.AlternateFlowPathMPDEstablished)
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AlternateFlowPathStable, code);
                }
                if (signals.FlowFillPumpDGD?.Mean != null && thresholds_.StableFlowFillPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowFillPumpDGD.Mean, 0, thresholds_.StableFlowFillPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FillPumpDGD, code);
                }
                if (signals.StandardDeviationFlowFillPumpDGD?.Mean != null && thresholds_.StableFlowFillPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowFillPumpDGD.Mean, thresholds_.StableFlowFillPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFillPumpDGD, code);
                }
                if (signals.FlowLiftPumpDGD?.Mean != null && thresholds_.StableFlowLiftPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowLiftPumpDGD.Mean, 0, thresholds_.StableFlowLiftPumpDGDThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LiftPumpDGD, code);
                }
                if (signals.StandardDeviationFlowLiftPumpDGD?.Mean != null && thresholds_.StableFlowFillPumpDGDThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowLiftPumpDGD.Mean, thresholds_.StableFlowFillPumpDGDThreshold.ScalarValue))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableLiftPumpDGD, code);
                }
                if (signals.UCS?.Mean != null && thresholds_.HardStringerThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.LT(signals.UCS.Mean, thresholds_.HardStringerThreshold.ScalarValue))
                    {
                        code = 1;
                        insideHardStringer = false;
                    }
                    else
                    {
                        code = 2;
                        insideHardStringer = true;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.InsideHardStringer, code);
                }
                if (signals.UCSSlope?.Mean != null && thresholds_.ChangeOfFormationUCSSlopeThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.UCSSlope.Mean, -thresholds_.ChangeOfFormationUCSSlopeThreshold.ScalarValue) &&
                        Numeric.LT(signals.UCSSlope.Mean, thresholds_.ChangeOfFormationUCSSlopeThreshold.ScalarValue))
                    {
                        code = 1;
                        changeOfFormation = 1;
                    }
                    else if (Numeric.GE(signals.UCSSlope.Mean, thresholds_.ChangeOfFormationUCSSlopeThreshold.ScalarValue))
                    {
                        code = 2;
                        changeOfFormation = 2;
                    }
                    else
                    {
                        code = 3;
                        changeOfFormation = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationChange, code);
                }
                if (signals.ToolJoint1Height?.Mean != null && signals.MinDrillHeight?.Mean != null && thresholds_.AtDrillHeightThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint1Height.Mean.Value - signals.MinDrillHeight.Mean.Value), thresholds_.AtDrillHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtLowestDrillHeight, code);
                }
                if (signals.ToolJoint1Height?.Mean != null && signals.StickUpHeight?.Mean != null && thresholds_.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint1Height.Mean.Value - signals.StickUpHeight.Mean.Value), thresholds_.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtStickUpHeight, code);
                }
                if (signals.ToolJoint2Height?.Mean != null && signals.StickUpHeight?.Mean != null && thresholds_.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint2Height.Mean.Value - signals.StickUpHeight.Mean.Value), thresholds_.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint2AtStickUpHeight, code);
                }
                if (signals.ToolJoint3Height?.Mean != null && signals.StickUpHeight?.Mean != null && thresholds_.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint3Height.Mean.Value - signals.StickUpHeight.Mean.Value), thresholds_.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint3AtStickUpHeight, code);
                }
                if (signals.ToolJoint4Height?.Mean != null && signals.StickUpHeight?.Mean != null && thresholds_.AtStickUpHeightThreshold?.ScalarValue != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs(signals.ToolJoint4Height.Mean.Value - signals.StickUpHeight.Mean.Value), thresholds_.AtStickUpHeightThreshold.ScalarValue.Value))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint4AtStickUpHeight, code);
                }
                if (signals.HeaveCompensationInactive != null && signals.HeaveCompensationActive != null)
                {
                    uint code = 0;
                    if (signals.HeaveCompensationInactive.Value && !signals.HeaveCompensationActive.Value)
                    {
                        code = 1;
                    }
                    else if (signals.HeaveCompensationActive.Value && !signals.HeaveCompensationInactive.Value)
                    {
                        code = 2;
                    }
                    else if (!signals.HeaveCompensationActive.Value && !signals.HeaveCompensationInactive.Value)
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HeaveCompensation, code);
                }
            }
            _logger.LogInformation("processed data");
            bool changed = false;
            lock (lock_)
            {
                changed |= currentMicroStates_.Part1 != microStates.Part1;
                changed |= currentMicroStates_.Part2 != microStates.Part2;
                changed |= currentMicroStates_.Part3 != microStates.Part3;
                changed |= currentMicroStates_.Part4 != microStates.Part4;
                changed |= currentMicroStates_.Part5 != microStates.Part5;
                if (changed)
                {
                    currentMicroStates_.Part1 = microStates.Part1;
                    currentMicroStates_.Part2 = microStates.Part2;
                    currentMicroStates_.Part3 = microStates.Part3;
                    currentMicroStates_.Part4 = microStates.Part4;
                    currentMicroStates_.Part5 = microStates.Part5;
                }
            }
            if (changed)
            {
                _logger.LogInformation("microstate has changed");
                microStates.TimeStampUTC = DateTime.UtcNow;
                string microStatePayload = JsonConvert.SerializeObject(microStates);
                if (microStateInDDHub_ != null && microStateInDDHub_.Count > 0)
                {
                    if (microStateInDDHub_[0].Count > 0)
                    {
                        NodeIdentifier id = microStateInDDHub_[0][0];
                        if (id != null && !string.IsNullOrEmpty(id.ID) && !string.IsNullOrEmpty(id.NameSpace))
                        {
                            // OPC-UA code to set the value at the node id = ID
                            (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                            outputs[0].nameSpace = id.NameSpace;
                            outputs[0].id = id.ID;
                            outputs[0].value = microStatePayload;
                            outputs[0].sourceTimestamp = DateTime.UtcNow;
                            DDHubClient_.UpdateAnyVariables(outputs);
                        }
                    }
                }
                var microStateMQTTMessage = new MqttApplicationMessageBuilder()
                  .WithTopic(DWIS.MicroState.MQTTTopics.Topics.CurrentMicroStates)
                  .WithPayload(microStatePayload)
                  .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                  .WithRetainFlag(true)
                  .Build();
                await mqttClient_.PublishAsync(microStateMQTTMessage);
                FormationStrength strength = new FormationStrength() { InsideHardStringer = insideHardStringer, ChangeOfFormation = changeOfFormation };
                string formationStrengthPayload = JsonConvert.SerializeObject(strength);
                var formationStrengthMQTTMessage = new MqttApplicationMessageBuilder()
                  .WithTopic(DWIS.MicroState.MQTTTopics.Topics.FormationStrengthStates)
                  .WithPayload(formationStrengthPayload)
                  .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                  .WithRetainFlag(true)
                  .Build();
                await mqttClient_.PublishAsync(formationStrengthMQTTMessage);
            }
        }

        private void UpdateMicroState(ref MicroStates microState, MicroStates.MicroStateIndex index, uint code)
        {
            uint val = 0;
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