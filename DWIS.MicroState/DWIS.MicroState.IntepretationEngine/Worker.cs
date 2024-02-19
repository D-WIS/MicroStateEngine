using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.General.Common;
using DWIS.Client.ReferenceImplementation;
using DWIS.API.DTO;
using DWIS.SPARQL.Utils;
using DWIS.Vocabulary.Schemas;

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
                            microStateThresholds.StableAxialVelocityTopOfStringThreshold = 0.5 / 3600.0;
                            microStateThresholds.StableRotationalVelocityTopOfStringThreshold = 0.5 / 60.0;
                            microStateThresholds.StableFlowTopOfStringThreshold = 10.0 / 60000.0;
                            microStateThresholds.StableTensionTopOfStringThreshold = 1000.0;
                            microStateThresholds.AtmosphericPressureThreshold = OSDC.DotnetLibraries.General.Common.Constants.EarthStandardAtmosphericPressure / 10.0;
                            microStateThresholds.StablePressureTopOfStringThreshold = 0.1 * 1e5;
                            microStateThresholds.StableTorqueTopOfStringThreshold = 10.0;
                            microStateThresholds.StableFlowAnnulusOutletThreshold = 1.0 / 60000.0;
                            microStateThresholds.StableBottomOfStringRockForceThreshold = 1000.0;
                            microStateThresholds.StableRotationalVelocityBottomOfStringThreshold = 1.0 / 60.0;
                            microStateThresholds.StableAxialVelocityBottomOfStringThreshold = 0.1 / 3600.0;
                            microStateThresholds.StableFlowBottomOfStringThreshold = 1.0 / 60000.0;
                            microStateThresholds.StableFlowHoleOpenerThreshold = 1.0 / 60000.0;
                            microStateThresholds.MinimumTensionTopOfString = 10000.0;
                            microStateThresholds.MinimumPressureFloatValve = 1e5;
                            microStateThresholds.StableFlowBoosterPumpThreshold = 10.0 / 60000.0;
                            microStateThresholds.StableFlowBackPressurePumpThreshold = 10.0 / 60000.0;
                            microStateThresholds.MinimumDifferentialPressureRCDSealingThreshold = 1e5;
                            microStateThresholds.MinimumDifferentialPressureSealBalanceThreshold = 1e5;
                            microStateThresholds.StableFlowFillPumpDGDThreshold = 10.0 / 60000.0;
                            microStateThresholds.StableFlowLiftPumpDGDThreshold = 10.0 / 60000.0;
                            microStateThresholds.StableCuttingsFlowThreshold = 1.0 / 60000.0;
                            microStateThresholds.HardStringerThreshold = 60e6;
                            microStateThresholds.ChangeOfFormationUCSSlopeThreshold = 2.5e6;
                            microStateThresholds.ForceOnLedgeThreshold = 10000;
                            microStateThresholds.ForceOnCuttingsBedThreshold = 10000;
                            microStateThresholds.ForceDifferentialStickingThreshold = 10000;
                            microStateThresholds.FluidFlowFormationThreshold = 10.0 / 60000.0;
                            microStateThresholds.WhirlRateThreshold = 0.5;
                            microStateThresholds.FlowPipeToAnnulusThreshold = 0.5 / 60000.0;
                            microStateThresholds.FlowCavingsFromFormationThreshold = 0.5 / 60000.0;
                            microStateThresholds.AtStickUpHeightThreshold = 0.1;
                            microStateThresholds.AtDrillHeightThreshold = 0.1;

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
                if (signals.AxialVelocityTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.AxialVelocityTopOfString, 0, thresholds_.StableAxialVelocityTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityTopOfString, 0, thresholds_.StableAxialVelocityTopOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityTopOfString, code);
                }
                if (signals.StandardDeviationAxialVelocityTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityTopOfString, thresholds_.StableAxialVelocityTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityTopOfString, code);
                }
                if (signals.RotationalVelocityTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.RotationalVelocityTopOfString, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationalVelocityTopOfString, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityTopOfString, code);
                }
                if (signals.StandardDeviationRotationalVelocityTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityTopOfString, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityTopOfString, code);
                }
                if (signals.FlowTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowTopOfString, 0, thresholds_.StableFlowTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtTopOfString, code);
                }
                if (signals.StandardDeviationFlowTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowTopOfString, thresholds_.StableFlowTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtTopOfString, code);
                }
                if (signals.TensionTopOfString != null &&
                    signals.ForceBottomTopDrive != null &&
                    signals.ForceElevator != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.TensionTopOfString, signals.ForceBottomTopDrive, thresholds_.StableTensionTopOfStringThreshold) || Numeric.EQ(signals.TensionTopOfString, signals.ForceElevator, thresholds_.StableTensionTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (!Numeric.EQ(signals.TensionTopOfString, 0, thresholds_.StableTensionTopOfStringThreshold) && Numeric.EQ(signals.ForceBottomTopDrive, 0, thresholds_.StableTensionTopOfStringThreshold) && Numeric.EQ(signals.ForceElevator, 0, thresholds_.StableTensionTopOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.SlipState, code);
                }
                if (signals.StandardDeviationTensionTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationTensionTopOfString, thresholds_.StableTensionTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTensionTopOfString, code);
                }
                if (signals.PressureTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.LE(signals.PressureTopOfString, Constants.EarthStandardAtmosphericPressure, thresholds_.StablePressureTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PressureTopOfString, code);
                }
                if (signals.StandardDeviationPressureTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationPressureTopOfString, thresholds_.StablePressureTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StablePressureTopOfString, code);
                }
                if (signals.TorqueTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.TorqueTopOfString, 0, thresholds_.StableTorqueTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TorqueTopOfString, code);
                }
                if (signals.StandardDeviationTorqueTopOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationTorqueTopOfString, thresholds_.StableTorqueTopOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTorqueTopOfString, code);
                }
                if (signals.FlowAnnulusOutlet != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowAnnulusOutlet, 0, thresholds_.StableFlowAnnulusOutletThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtAnnulusOutlet, code);
                }
                if (signals.StandardDeviationFlowAnnulusOutlet != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowAnnulusOutlet, thresholds_.StableFlowAnnulusOutletThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtAnnulusOutlet, code);
                }
                if (signals.FlowCuttingsAnnulusOutlet != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsAnnulusOutlet, 0, thresholds_.StableCuttingsFlowThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsReturnAtAnnulusOutlet, code);
                }
                if (signals.ForceBottomOfStringOnRock != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceBottomOfStringOnRock, 0, thresholds_.StableBottomOfStringRockForceThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomBottomOfString, code);
                }
                if (signals.StandardDeviationForceBottomOfStringOnRock != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationForceBottomOfStringOnRock, thresholds_.StableBottomOfStringRockForceThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBottomOfStringRockForce, code);
                }
                if (signals.ForceHoleOpenerOnRock != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceHoleOpenerOnRock, 0, thresholds_.StableBottomOfStringRockForceThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomHoleOpener, code);
                }
                if (signals.RotationaVelocityBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.RotationaVelocityBottomOfString, 0, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.RotationaVelocityBottomOfString, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityBottomOfString, code);
                }
                if (signals.StandardDeviationRotationalVelocityBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationRotationalVelocityBottomOfString, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityBottomOfString, code);
                }
                if (signals.FlowCuttingsBottomHole != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsBottomHole, 0, thresholds_.StableCuttingsFlowThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Drilling, code);
                }
                if (signals.FlowCuttingsTopOfRateHole != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCuttingsTopOfRateHole, 0, thresholds_.StableCuttingsFlowThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HoleOpening, code);
                }
                if (signals.AxialVelocityBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.AxialVelocityBottomOfString, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.AxialVelocityBottomOfString, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityBottomOfString, code);
                }
                if (signals.StandardDeviationAxialVelocityBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationAxialVelocityBottomOfString, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityBottomOfString, code);
                }
                if (signals.FlowBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBottomOfString, 0, thresholds_.StableFlowBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowBottomOfString, 0, thresholds_.StableFlowBottomOfStringThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowBottomOfString, code);
                }
                if (signals.StableFlowBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StableFlowBottomOfString, thresholds_.StableFlowBottomOfStringThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowBottomOfString, code);
                }
                if (signals.FlowHoleOpener != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowHoleOpener, 0, thresholds_.StableFlowHoleOpenerThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowHoleOpener, 0, thresholds_.StableFlowHoleOpenerThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowHoleOpener, code);
                }
                if (signals.StableFlowHoleOpener != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StableFlowHoleOpener, thresholds_.StableFlowHoleOpenerThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowHoleOpener, code);
                }
                if (signals.ForceOnLedge != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceOnLedge, 0, thresholds_.ForceOnLedgeThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnLedge, 0, thresholds_.ForceOnLedgeThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LedgeKeySeat, code);
                }
                if (signals.ForceOnCuttingsBed != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceOnCuttingsBed, 0, thresholds_.ForceOnCuttingsBedThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.ForceOnCuttingsBed, 0, thresholds_.ForceOnCuttingsBedThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsBed, code);
                }
                if (signals.ForceDifferentialSticking != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.ForceDifferentialSticking, 0, thresholds_.ForceDifferentialStickingThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.DifferentialSticking, code);
                }
                if (signals.TensionTopOfString != null && signals.MinimumTensionForTwistOffDetection != null)
                {
                    uint code = 0;
                    if (Numeric.GE(signals.TensionTopOfString, signals.MinimumTensionForTwistOffDetection))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TwistOffBackOff, code);
                }
                if (signals.FlowFluidFromOrToFormation != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowFluidFromOrToFormation, 0, thresholds_.FluidFlowFormationThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.FlowFluidFromOrToFormation, 0, thresholds_.FluidFlowFormationThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WellIntegrity, code);
                }
                if (signals.FlowFormationFluidAnnulusOutlet != null)
                {
                    uint code = 0;

                    if (Numeric.EQ(signals.FlowFormationFluidAnnulusOutlet, 0, thresholds_.FluidFlowFormationThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationFluidAtAnnulusOutlet, code);
                }
                if (signals.FlowCavingsFromFormation != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCavingsFromFormation, 0, thresholds_.FlowCavingsFromFormationThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationCollapse, code);
                }
                if (signals.FlowCavingsAnnulusOutlet != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowCavingsAnnulusOutlet, 0, thresholds_.FlowCavingsFromFormationThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CavingsAtAnnulusOutlet, code);
                }
                if (signals.FlowPipeToAnnulus != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowPipeToAnnulus, 0, thresholds_.FlowPipeToAnnulusThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PipeWashout, code);
                }
                if (signals.WhirlRateBottomOfString != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.WhirlRateBottomOfString, 0, thresholds_.WhirlRateThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateBottomOfString, 0, thresholds_.WhirlRateThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlBottomOfString, code);
                }
                if (signals.WhirlRateHoleOpener != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.WhirlRateHoleOpener, 0, thresholds_.WhirlRateThreshold))
                    {
                        code = 1;
                    }
                    else if (Numeric.GT(signals.WhirlRateHoleOpener, 0, thresholds_.WhirlRateThreshold))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlHoleOpener, code);
                }
                if (signals.DifferentialPressureFloatValve != null)
                {
                    uint code = 0;
                    if (Numeric.LE(signals.DifferentialPressureFloatValve, thresholds_.MinimumPressureFloatValve))
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
                if (signals.FlowBoosterPump != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBoosterPump, 0, thresholds_.StableFlowBoosterPumpThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BoosterPumping, code);
                }
                if (signals.StandardDeviationFlowBoosterPump != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowBoosterPump, thresholds_.StableFlowBoosterPumpThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBoosterPumping, code);
                }
                if (signals.FlowBackPressurePump != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowBackPressurePump, 0, thresholds_.StableFlowBackPressurePumpThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BackPressurePumping, code);
                }
                if (signals.StandardDeviationFlowBackPressurePump != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowBackPressurePump, thresholds_.StableFlowBackPressurePumpThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBackPressurePumping, code);
                }
                if (signals.OpeningMPDChoke != null)
                {
                    uint code = 0;
                    if (Numeric.LE(signals.OpeningMPDChoke, 0))
                    {
                        code = 1;
                    }
                    else if (Numeric.GE(signals.OpeningMPDChoke, 1))
                    {
                        code = 2;
                    }
                    else
                    {
                        code = 3;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MPDChokeOpening, code);
                }
                if (signals.DifferentialPressureRCD != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.DifferentialPressureRCD, 0, thresholds_.MinimumDifferentialPressureRCDSealingThreshold))
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
                if (signals.DifferentialPressureIsolationSeal != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.DifferentialPressureIsolationSeal, 0, thresholds_.MinimumDifferentialPressureSealBalanceThreshold))
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
                if (signals.FlowFillPumpDGD != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowFillPumpDGD, 0, thresholds_.StableFlowFillPumpDGDThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FillPumpDGD, code);
                }
                if (signals.StandardDeviationFlowFillPumpDGD != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowFillPumpDGD, thresholds_.StableFlowFillPumpDGDThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFillPumpDGD, code);
                }
                if (signals.FlowLiftPumpDGD != null)
                {
                    uint code = 0;
                    if (Numeric.EQ(signals.FlowLiftPumpDGD, 0, thresholds_.StableFlowLiftPumpDGDThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LiftPumpDGD, code);
                }
                if (signals.StandardDeviationFlowLiftPumpDGD != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.StandardDeviationFlowLiftPumpDGD, thresholds_.StableFlowFillPumpDGDThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableLiftPumpDGD, code);
                }
                if (signals.UCS != null)
                {
                    uint code = 0;
                    if (Numeric.LT(signals.UCS, thresholds_.HardStringerThreshold))
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
                if (signals.UCSSlope != null)
                {
                    uint code = 0;
                    if (Numeric.GT(signals.UCSSlope, -thresholds_.ChangeOfFormationUCSSlopeThreshold) &&
                        Numeric.LT(signals.UCSSlope, thresholds_.ChangeOfFormationUCSSlopeThreshold))
                    {
                        code = 1;
                        changeOfFormation = 1;
                    }
                    else if (Numeric.GE(signals.UCSSlope, thresholds_.ChangeOfFormationUCSSlopeThreshold))
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
                if (signals.ToolJoint1Height != null && signals.MinDrillHeight != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs((double)signals.ToolJoint1Height - (double)signals.MinDrillHeight), thresholds_.AtDrillHeightThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtLowestDrillHeight, code);
                }
                if (signals.ToolJoint1Height != null && signals.StickUpHeight != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs((double)signals.ToolJoint1Height - (double)signals.StickUpHeight), thresholds_.AtStickUpHeightThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint1AtStickUpHeight, code);
                }
                if (signals.ToolJoint2Height != null && signals.StickUpHeight != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs((double)signals.ToolJoint2Height - (double)signals.StickUpHeight), thresholds_.AtStickUpHeightThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint2AtStickUpHeight, code);
                }
                if (signals.ToolJoint3Height != null && signals.StickUpHeight != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs((double)signals.ToolJoint3Height - (double)signals.StickUpHeight), thresholds_.AtStickUpHeightThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint3AtStickUpHeight, code);
                }
                if (signals.ToolJoint4Height != null && signals.StickUpHeight != null)
                {
                    uint code = 0;
                    if (Numeric.GT(System.Math.Abs((double)signals.ToolJoint4Height - (double)signals.StickUpHeight), thresholds_.AtStickUpHeightThreshold))
                    {
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ToolJoint4AtStickUpHeight, code);
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