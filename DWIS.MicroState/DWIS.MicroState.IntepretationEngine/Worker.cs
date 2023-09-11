using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.General.Common;
using System.Text;
using System.Timers;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;

namespace DWIS.MicroState.IntepretationEngine
{
    public class Worker : BackgroundService
    {
        private Configuration Configuration { get; set; } = new Configuration();
        private readonly ILogger<Worker> _logger;
        private IManagedMqttClient mqttReceiverClient_;
        public IMqttClient mqttClient_;
        private Thresholds thresholds_ = null;
        private Signals signals_ = null;
        private Dictionary<Guid, SignalMapping> signalDictionary_ = new Dictionary<Guid, SignalMapping>();
        private List<string> subscribedSignalTopic_ = new List<string>();
        private MicroStates currentMicroStates_ = new MicroStates();
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            await ConnectAndSubscribeReceiverAsync();
            await ConnectAndSubscribeEmitterAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                await RefreshSignals();
                await Task.Delay(1000, stoppingToken);
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
                        Configuration config = JsonConvert.DeserializeObject<Configuration>(jsonContent);
                        if (config != null)
                        {
                            Configuration = config;
                        }
                    }
                }
            }
            thresholds_ = new Thresholds();
            thresholds_.TimeStampUTC = DateTime.UtcNow;
            thresholds_.StableAxialVelocityTopOfStringThreshold = 0.5 / 3600.0;
            thresholds_.StableRotationalVelocityTopOfStringThreshold = 0.5 / 60.0;
            thresholds_.StableFlowTopOfStringThreshold = 10.0 / 60000.0;
            thresholds_.StableTensionTopOfStringThreshold = 1000.0;
            thresholds_.AtmosphericPressureThreshold = Constants.EarthStandardAtmosphericPressure / 10.0;
            thresholds_.StablePressureTopOfStringThreshold = 0.1 * 1e5;
            thresholds_.StableTorqueTopOfStringThreshold = 10.0;
            thresholds_.StableFlowAnnulusOutletThreshold = 1.0 / 60000.0;
            thresholds_.StableBottomOfStringRockForceThreshold = 1000.0;
            thresholds_.StableRotationalVelocityBottomOfStringThreshold = 1.0 / 60.0;
            thresholds_.StableAxialVelocityBottomOfStringThreshold = 0.1 / 3600.0;
            thresholds_.StableFlowBottomOfStringThreshold = 1.0 / 60000.0;
            thresholds_.StableFlowHoleOpenerThreshold = 1.0 / 60000.0;
            thresholds_.MinimumTensionTopOfString = 10000.0;
            thresholds_.MinimumPressureFloatValve = 1e5;
            thresholds_.StableFlowBoosterPumpThreshold = 10.0 / 60000.0;
            thresholds_.StableFlowBackPressurePumpThreshold = 10.0 / 60000.0;
            thresholds_.MinimumDifferentialPressureRCDSealingThreshold = 1e5;
            thresholds_.MinimumDifferentialPressureSealBalanceThreshold = 1e5;
            thresholds_.StableFlowFillPumpDGDThreshold = 10.0 / 60000.0;
            thresholds_.StableFlowLiftPumpDGDThreshold = 10.0 / 60000.0;
            thresholds_.StableCuttingsFlowThreshold = 1.0 / 60000.0;
            thresholds_.HardStringerThreshold = 40e6;
            thresholds_.ChangeOfFormationUCSSlopeThreshold = 2.5e6;
            thresholds_.ForceOnLedgeThreshold = 10000;
            thresholds_.ForceOnCuttingsBedThreshold = 10000;
            thresholds_.ForceDifferentialStickingThreshold = 10000;
            thresholds_.FluidFlowFormationThreshold = 10.0 / 60000.0;
            thresholds_.WhirlRateThreshold = 0.5;
            thresholds_.FlowPipeToAnnulusThreshold = 0.5 / 60000.0;
            thresholds_.FlowCavingsFromFormationThreshold = 0.5 / 60000.0;

            signals_ = new Signals();
            signals_.TimeStampUTC = DateTime.UtcNow;
            signals_.MaximumBindingRefreshInterval = TimeSpan.FromSeconds(5);

            signals_.AxialVelocityTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "AxialVelocityTopOfString", ID = new Guid("bc991ccf-6d8a-4bdd-a78a-63204c53b6a9") };
            signals_.StandardDeviationAxialVelocityTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationAxialVelocityTopOfString", ID = new Guid("49c15ed5-cfff-40d3-be6f-1f82d1e8b09d") };
            signals_.RotationalVelocityTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "RotationalVelocityTopOfString", ID = new Guid("1f67db23-4916-4f3f-8046-3c28c3076819") };
            signals_.StandardDeviationRotationalVelocityTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationRotationalVelocityTopOfString", ID = new Guid("01cea621-e634-43b0-ab9d-3402c2899591") };
            signals_.FlowTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowTopOfString", ID = new Guid("37ef27af-9f47-4c82-bc0d-268c6c38b194") };
            signals_.StandardDeviationFlowTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowTopOfString", ID = new Guid("cf64f77c-bb3a-4700-b55d-cdebe65bf4a6") };
            signals_.TensionTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "TensionTopOfString", ID = new Guid("7d65e3ea-ae9e-4be5-9bfc-59db904adcf6") };
            signals_.ForceBottomTopDrive = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceBottomTopDrive", ID = new Guid("96373b14-240d-410a-8d6e-e38a7f59f41d") };
            signals_.ForceElevator = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceElevator", ID = new Guid("2258c3ab-40a2-487c-bef3-cb625a5d9ecd") };
            signals_.StandardDeviationTensionTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationTensionTopOfString", ID = new Guid("2f7dc613-3fd6-4ea5-b27a-b3d97cc8b253") };
            signals_.PressureTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "PressureTopOfString", ID = new Guid("97d8f4c0-e513-479e-8ef1-b14e98e2d867") };
            signals_.StandardDeviationPressureTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationPressureTopOfString", ID = new Guid("c7001988-6064-458e-b0ad-1ab9bb431b3c") };
            signals_.TorqueTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "TorqueTopOfString", ID = new Guid("53d1b1bf-79f4-4bb0-810b-0c288d7f98ac") };
            signals_.StandardDeviationTorqueTopOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationTorqueTopOfString", ID = new Guid("af282cad-71f0-47b1-9cf7-59f6a48906e0") };
            signals_.FlowAnnulusOutlet = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowAnnulusOutlet", ID = new Guid("a8e9be02-210d-4b19-8d20-3bf1cc8cace0") };
            signals_.StandardDeviationFlowAnnulusOutlet = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowAnnulusOutlet", ID = new Guid("d83b6cef-7123-4821-90c4-f6536f04cd1a") };
            signals_.FlowCuttingsAnnulusOutlet = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowCuttingsAnnulusOutlet", ID = new Guid("bfe7efc9-5657-47ee-82f3-5a7a085ec71f") };
            signals_.ForceBottomOfStringOnRock = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceBottomOfStringOnRock", ID = new Guid("40cf5e40-875d-4fd4-b82c-879b8add2441") };
            signals_.StandardDeviationForceBottomOfStringOnRock = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationForceBottomOfStringOnRock", ID = new Guid("f144c38d-ab45-4b7f-bedf-07a7c43ce920") };
            signals_.ForceHoleOpenerOnRock = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceHoleOpenerOnRock", ID = new Guid("c151e2ba-ad2a-436d-884b-d2f406932b62") };
            signals_.RotationaVelocityBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "RotationaVelocityBottomOfString", ID = new Guid("4f8de13c-8a16-4880-b3d9-2a6935b5f0ac") };
            signals_.StandardDeviationRotationalVelocityBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationRotationalVelocityBottomOfString", ID = new Guid("371dee90-5552-4aed-b406-5f61f61a904b") };
            signals_.FlowCuttingsBottomHole = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowCuttingsBottomHole", ID = new Guid("6729ff39-d8a3-409b-90e9-e9aaaa13f2ef") };
            signals_.FlowCuttingsTopOfRateHole = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowCuttingsTopOfRateHole", ID = new Guid("dc866c9b-c2ba-49bb-916e-79d1add3f642") };
            signals_.AxialVelocityBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "AxialVelocityBottomOfString", ID = new Guid("dd9bd12d-199f-4236-9380-6a4ae38608c5") };
            signals_.StandardDeviationAxialVelocityBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationAxialVelocityBottomOfString", ID = new Guid("353fdb50-6c85-4839-b104-5bd984169ad7") };
            signals_.FlowBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowBottomOfString", ID = new Guid("f884e097-4442-4eaa-a0e8-0a1af4bcc396") };
            signals_.StableFlowBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StableFlowBottomOfString", ID = new Guid("be0b9e6c-d60f-42a8-adfb-8ee6a76c605d") };
            signals_.FlowHoleOpener = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowHoleOpener", ID = new Guid("b9c9280a-7821-4d91-a8cd-1eaa1a950dc9") };
            signals_.StableFlowHoleOpener = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StableFlowHoleOpener", ID = new Guid("33eb2fa7-74fe-4e7f-ac61-a16c26a215cd") };
            signals_.ForceOnLedge = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceOnLedge", ID = new Guid("8ac10ef4-19d1-463c-86ca-dc1f3ded6481") };
            signals_.ForceOnCuttingsBed = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceOnCuttingsBed", ID = new Guid("340f7e4b-2bfd-4c21-bfa6-78b92b831a1a") };
            signals_.ForceDifferentialSticking = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "ForceDifferentialSticking", ID = new Guid("9253aa58-dbf9-4a9e-9ffc-014b70dcd610") };
            signals_.FlowFluidFromOrToFormation = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowFluidFromOrToFormation", ID = new Guid("17cb9c25-2d19-4649-b198-50684d020e1d") };
            signals_.FlowFormationFluidAnnulusOutlet = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowFormationFluidAnnulusOutlet", ID = new Guid("f7dc08f0-8001-45ec-b66b-60189029591d") };
            signals_.FlowCavingsFromFormation = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowCavingsFromFormation", ID = new Guid("9f4062ed-e666-4a5b-bab9-d0b849845a77") };
            signals_.FlowCavingsAnnulusOutlet = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowCavingsAnnulusOutlet", ID = new Guid("429b2472-9326-4c46-a684-4cd119469f3b") };
            signals_.FlowPipeToAnnulus = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowPipeToAnnulus", ID = new Guid("b6ca4593-63d9-499c-8ada-eff1bb34cb31") };
            signals_.WhirlRateBottomOfString = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "WhirlRateBottomOfString", ID = new Guid("e3085773-270f-4e85-9c04-982792d3bd89") };
            signals_.WhirlRateHoleOpener = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "WhirlRateHoleOpener", ID = new Guid("b8c56e5e-6632-4971-81d6-b25300ede7ee") };
            signals_.DifferentialPressureFloatValve = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "DifferentialPressureFloatValve", ID = new Guid("2fb2534a-db02-4aaa-921f-1a01b81f2aa8") };
            signals_.FlowBoosterPump = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowBoosterPump", ID = new Guid("5e1bf167-9fc5-47b1-9dec-86b5bc436ea0") };
            signals_.StandardDeviationFlowBoosterPump = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowBoosterPump", ID = new Guid("b994fee4-c975-451b-86f3-44185fbd5369") };
            signals_.FlowBackPressurePump = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowBackPressurePump", ID = new Guid("154ea99a-99c2-4999-bfb9-2826b5d7616e") };
            signals_.StandardDeviationFlowBackPressurePump = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowBackPressurePump", ID = new Guid("6d883888-1861-402b-9fe8-8495e22df5ba") };
            signals_.OpeningMPDChoke = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "OpeningMPDChoke", ID = new Guid("eac2b4ee-1512-427c-a43d-579e31744184") };
            signals_.DifferentialPressureRCD = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "DifferentialPressureRCD", ID = new Guid("7f34c854-8628-4361-a5fe-88e536736f5d") };
            signals_.DifferentialPressureIsolationSeal = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "DifferentialPressureIsolationSeal", ID = new Guid("4c68812a-bee0-4a35-af79-aa2f91cda308") };
            signals_.FlowFillPumpDGD = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowFillPumpDGD", ID = new Guid("1581a580-9466-404d-ab55-990a2ecd8175") };
            signals_.FlowLiftPumpDGD = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "FlowLiftPumpDGD", ID = new Guid("c74ae1e3-5b23-44ef-8e1d-4480f79890c8") };
            signals_.StandardDeviationFlowFillPumpDGD = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowFillPumpDGD", ID = new Guid("4fbf8d28-d610-48f1-af0a-84b88f378d0e") };
            signals_.StandardDeviationFlowLiftPumpDGD = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "StandardDeviationFlowLiftPumpDGD", ID = new Guid("40436227-ef9c-479d-9b05-4701c6e9ebdf") };
            signals_.UCS = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "UCS", ID = new Guid("fe37d619-0b88-4f58-8e18-0f886f3f9a8a") };
            signals_.UCSSlope = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "UCSSlope", ID = new Guid("6af57812-af41-44e0-9381-d7e39d6f3208") };
            signals_.MinimumTensionForTwistOffDetection = new RigOS.Capabilities.ModelShared.ReadableReferenceOfScalarValue() { Name = "MinimumTensionForTwistOffDetection", ID = new Guid("45bac978-015c-4428-8be9-4054f3bef765") };

            signals_.UnderReamerOpen = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "UnderReamerOpen", ID = new Guid("58424947-2eb6-4223-9316-3e42188f9267") };
            signals_.CirculationSubOpen = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "CirculationSubOpen", ID = new Guid("87023537-d902-419c-a585-48a0d1e52dcc") };
            signals_.PortedFloatOpen = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "PortedFloatOpen", ID = new Guid("5a33bad6-9585-416d-947b-0c9be9fb03ad") };
            signals_.WhipstockAttached = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "WhipstockAttached", ID = new Guid("38147bf4-3564-4107-a03f-db7908754231") };
            signals_.PlugAttached = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "PlugAttached", ID = new Guid("2dba71cd-9640-4d12-98ba-198b2fb1590a") };
            signals_.LinerAttached = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "LinerAttached", ID = new Guid("dc17aff7-18cf-4f57-90ab-ccb082bc019f") };
            signals_.IsolationSealActivated = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "IsolationSealActivated", ID = new Guid("56e4c80a-0221-446d-bf90-5782be100b75") };
            signals_.BearingAssemblyLatched = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "BearingAssemblyLatched", ID = new Guid("9419a1d0-3568-4f39-9ed0-13d7ee765736") };
            signals_.ScreenMPDChokePlugged = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "ScreenMPDChokePlugged", ID = new Guid("1a41e95d-8e8c-4c89-a19c-05fd6751a202") };
            signals_.MainFlowPathMPDEstablished = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "MainFlowPathMPDEstablished", ID = new Guid("d3233523-983b-45c2-b74f-bb4a8f2de861") };
            signals_.AlternateFlowPathMPDEstablished = new RigOS.Capabilities.ModelShared.ReadableReferenceOfBooleanValue() { Name = "AlternateFlowPathMPDEstablished", ID = new Guid("1035b36e-a2db-490f-9429-e0c730d9517c") };
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

            // Create MQTT message and publish
            string thresholdsPayload = JsonConvert.SerializeObject(thresholds_);
            var thresholdsMQTTMessage = new MqttApplicationMessageBuilder()
                .WithTopic(DWIS.MicroState.MQTTTopics.Topics.Thresholds)
                .WithPayload(thresholdsPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag(true)
                .Build();
            await mqttClient_.PublishAsync(thresholdsMQTTMessage);

            string signalsPayload = JsonConvert.SerializeObject(signals_);
            var signalsMQTTMessage = new MqttApplicationMessageBuilder()
                .WithTopic(DWIS.MicroState.MQTTTopics.Topics.SignalsInputs)
                .WithPayload(signalsPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag(true)
                .Build();
            await mqttClient_.PublishAsync(signalsMQTTMessage);

        }

        private async Task ConnectAndSubscribeReceiverAsync()
        {
            // Configure MQTT client options
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithTcpServer(Configuration.MQTTServerName, Configuration.MQTTServerPort) // Replace with your MQTT broker details
                    .Build())
                .Build();

            // Create MQTT client
            mqttReceiverClient_ = new MqttFactory().CreateManagedMqttClient();

            // Wire up event handlers
            mqttReceiverClient_.ApplicationMessageReceivedAsync += HandleMqttMessageReceived;

            // Connect and subscribe to topic
            await mqttReceiverClient_.StartAsync(options);
            await mqttReceiverClient_.SubscribeAsync(DWIS.MicroState.MQTTTopics.Topics.SignalsInputs);
            await mqttReceiverClient_.SubscribeAsync(DWIS.MicroState.MQTTTopics.Topics.ThresholdsRequests);

            // Additional setup or logic can be added here
        }

        private async Task HandleMqttMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            if (eventArgs != null)
            {
                if (eventArgs.ApplicationMessage != null)
                {
                    string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                    if (eventArgs.ApplicationMessage.Topic == DWIS.MicroState.MQTTTopics.Topics.SignalsInputs)
                    {
                        // Deserialize the JSON payload
                        var message = JsonConvert.DeserializeObject<Signals>(payload);

                    }
                    else if (eventArgs.ApplicationMessage.Topic == DWIS.MicroState.MQTTTopics.Topics.ThresholdsRequests)
                    {
                        // Deserialize the JSON payload
                        var message = JsonConvert.DeserializeObject<Thresholds>(payload);
                        if (message != null && thresholds_ != null)
                        {
                            message.CopyTo(thresholds_);
                            thresholds_.TimeStampUTC = DateTime.UtcNow;

                            string thresholdsPayload = JsonConvert.SerializeObject(thresholds_);

                            // Create MQTT message and publish
                            var mqttMessage = new MqttApplicationMessageBuilder()
                                .WithTopic(DWIS.MicroState.MQTTTopics.Topics.Thresholds)
                                .WithPayload(thresholdsPayload)
                                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                                .WithRetainFlag(true)
                                .Build();
                            await mqttClient_.PublishAsync(mqttMessage);

                        }
                    }
                    else if (eventArgs.ApplicationMessage.Topic == DWIS.MicroState.MQTTTopics.Topics.SignalSourceScalars)
                    {
                        var message = JsonConvert.DeserializeObject<double?[]>(payload);
                        if (message != null)
                        {
                            await ProcessSignals(eventArgs.ApplicationMessage.Topic, message);
                        }
                    }
                    else if (eventArgs.ApplicationMessage.Topic == DWIS.MicroState.MQTTTopics.Topics.SignalSourceBooleans)
                    {
                        var message = JsonConvert.DeserializeObject<bool?[]>(payload);
                        if (message != null)
                        {
                            await ProcessSignals(eventArgs.ApplicationMessage.Topic, message);
                        }
                    }
                }
            }
        }

        private async Task RefreshSignals()
        {
            // start refresh
            if (!subscribedSignalTopic_.Contains(DWIS.MicroState.MQTTTopics.Topics.SignalSourceScalars) && mqttReceiverClient_ != null)
            {
                await mqttReceiverClient_.SubscribeAsync(DWIS.MicroState.MQTTTopics.Topics.SignalSourceScalars);
            }
            if (!subscribedSignalTopic_.Contains(DWIS.MicroState.MQTTTopics.Topics.SignalSourceBooleans) && mqttReceiverClient_ != null)
            {
                await mqttReceiverClient_.SubscribeAsync(DWIS.MicroState.MQTTTopics.Topics.SignalSourceBooleans);
            }
            if (signals_ != null)
            {
                List<Guid> scalarIDs = signals_.GetScalarIDs();
                List<Guid> booleanIDs = signals_.GetBooleanIDs();
                lock (lock_)
                {
                    signalDictionary_.Clear();
                    for (int i = 0; i < scalarIDs.Count; i++)
                    {
                        signalDictionary_.Add(scalarIDs[i], new SignalMapping(DWIS.MicroState.MQTTTopics.Topics.SignalSourceScalars, i));
                    }
                    for (int i = 0; i < booleanIDs.Count; i++)
                    {
                        signalDictionary_.Add(booleanIDs[i], new SignalMapping(DWIS.MicroState.MQTTTopics.Topics.SignalSourceBooleans, i));
                    }
                }

            }
            // end refresh
            signals_.TimeStampUTC = DateTime.UtcNow;
            string signalsPayload = JsonConvert.SerializeObject(signals_);
            var signalsMQTTMessage = new MqttApplicationMessageBuilder()
                .WithTopic(DWIS.MicroState.MQTTTopics.Topics.SignalsInputs)
                .WithPayload(signalsPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag(true)
                .Build();
            await mqttClient_.PublishAsync(signalsMQTTMessage);
        }

        private async Task ProcessSignals(string topic, double?[] signals)
        {
            if (!string.IsNullOrEmpty(topic) && signals != null)
            {
                lock (lock_)
                {
                    if (signalDictionary_ != null)
                    {
                        foreach (Guid ID in signalDictionary_.Keys)
                        {
                            SignalMapping mapping = signalDictionary_[ID];
                            if (mapping != null && mapping.MQTTTopic == topic && mapping.Index >= 0 && mapping.Index < signals.Length)
                            {
                                mapping.Value = signals[mapping.Index];
                            }
                        }
                    }
                }
                bool changed = ProcessSignals();
                if (changed)
                {
                    MicroStates microStates = new MicroStates();
                    lock (lock_)
                    {
                        currentMicroStates_.CopyTo(ref microStates);
                    }
                    microStates.TimeStampUTC = DateTime.UtcNow;
                    string microStatePayload = JsonConvert.SerializeObject(microStates);
                    var microStateMQTTMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(DWIS.MicroState.MQTTTopics.Topics.CurrentMicroStates)
                        .WithPayload(microStatePayload)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                        .WithRetainFlag(true)
                        .Build();
                    await mqttClient_.PublishAsync(microStateMQTTMessage);
                }
            }
        }

        private async Task ProcessSignals(string topic, bool?[] signals)
        {
            if (!string.IsNullOrEmpty(topic) && signals != null)
            {
                lock (lock_)
                {
                    if (signalDictionary_ != null)
                    {
                        foreach (Guid ID in signalDictionary_.Keys)
                        {
                            SignalMapping mapping = signalDictionary_[ID];
                            if (mapping != null && mapping.MQTTTopic == topic && mapping.Index >= 0 && mapping.Index < signals.Length)
                            {
                                mapping.Value = signals[mapping.Index];
                            }
                        }
                    }
                }
                bool changed = ProcessSignals();
                if (changed)
                {
                    MicroStates microStates = new MicroStates();
                    lock (lock_)
                    {
                        currentMicroStates_.CopyTo(ref microStates);
                    }
                    microStates.TimeStampUTC = DateTime.UtcNow;
                    string microStatePayload = JsonConvert.SerializeObject(microStates);
                    var microStateMQTTMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(DWIS.MicroState.MQTTTopics.Topics.CurrentMicroStates)
                        .WithPayload(microStatePayload)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                        .WithRetainFlag(true)
                        .Build();
                    await mqttClient_.PublishAsync(microStateMQTTMessage);
                }
            }
        }
        private bool ProcessSignals()
        {
            MicroStates microStates = new MicroStates();
            if (signals_ != null)
            {
                if (signals_.AxialVelocityTopOfString != null)
                {
                    Guid ID = signals_.AxialVelocityTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableAxialVelocityTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.StableAxialVelocityTopOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityTopOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationAxialVelocityTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationAxialVelocityTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableAxialVelocityTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityTopOfString, code);
                        }
                    }
                }
                if (signals_.RotationalVelocityTopOfString != null)
                {
                    Guid ID = signals_.RotationalVelocityTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityTopOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationRotationalVelocityTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationRotationalVelocityTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableRotationalVelocityTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityTopOfString, code);
                        }
                    }
                }
                if (signals_.FlowTopOfString != null)
                {
                    Guid ID = signals_.FlowTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtTopOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationFlowTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationFlowTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtTopOfString, code);
                        }
                    }
                }
                if (signals_.TensionTopOfString != null &&
                    signals_.ForceBottomTopDrive != null &&
                    signals_.ForceElevator != null)
                {
                    Guid ID1 = signals_.TensionTopOfString.ID;
                    Guid ID2 = signals_.ForceBottomTopDrive.ID;
                    Guid ID3 = signals_.ForceElevator.ID;
                    if (signalDictionary_.ContainsKey(ID1) && signalDictionary_.ContainsKey(ID2) && signalDictionary_.ContainsKey(ID3))
                    {
                        SignalMapping mapping1 = signalDictionary_[ID1];
                        SignalMapping mapping2 = signalDictionary_[ID2];
                        SignalMapping mapping3 = signalDictionary_[ID3];
                        if (mapping1 != null && mapping1.Value != null && mapping1.Value is double &&
                            mapping2 != null && mapping2.Value != null && mapping2.Value is double &&
                            mapping3 != null && mapping3.Value != null && mapping3.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping1.Value) && Numeric.IsDefined((double)mapping2.Value) && Numeric.IsDefined((double)mapping3.Value))
                            {
                                if (Numeric.EQ((double)mapping1.Value, (double)mapping2.Value, thresholds_.StableTensionTopOfStringThreshold) || Numeric.EQ((double)mapping1.Value, (double)mapping3.Value, thresholds_.StableTensionTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (!Numeric.EQ((double)mapping1.Value, 0, thresholds_.StableTensionTopOfStringThreshold) && Numeric.EQ((double)mapping2.Value, 0, thresholds_.StableTensionTopOfStringThreshold) && Numeric.EQ((double)mapping3.Value, 0, thresholds_.StableTensionTopOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.SlipState, code);
                        }
                    }
                }
                if (signals_.StandardDeviationTensionTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationTensionTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableTensionTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTensionTopOfString, code);
                        }
                    }
                }
                if (signals_.PressureTopOfString != null)
                {
                    Guid ID = signals_.PressureTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.LE((double)mapping.Value, Constants.EarthStandardAtmosphericPressure, thresholds_.StablePressureTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PressureTopOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationPressureTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationPressureTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StablePressureTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StablePressureTopOfString, code);
                        }
                    }
                }
                if (signals_.TorqueTopOfString != null)
                {
                    Guid ID = signals_.TorqueTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableTorqueTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TorqueTopOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationTorqueTopOfString != null)
                {
                    Guid ID = signals_.StandardDeviationTorqueTopOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableTorqueTopOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableTorqueTopOfString, code);
                        }
                    }
                }
                if (signals_.FlowAnnulusOutlet != null)
                {
                    Guid ID = signals_.FlowAnnulusOutlet.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowAnnulusOutletThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowAtAnnulusOutlet, code);
                        }
                    }
                }
                if (signals_.StandardDeviationFlowAnnulusOutlet != null)
                {
                    Guid ID = signals_.StandardDeviationFlowAnnulusOutlet.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowAnnulusOutletThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowAtAnnulusOutlet, code);
                        }
                    }
                }
                if (signals_.FlowCuttingsAnnulusOutlet != null)
                {
                    Guid ID = signals_.FlowCuttingsAnnulusOutlet.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableCuttingsFlowThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsReturnAtAnnulusOutlet, code);
                        }
                    }
                }
                if (signals_.ForceBottomOfStringOnRock != null)
                {
                    Guid ID = signals_.ForceBottomOfStringOnRock.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableBottomOfStringRockForceThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomBottomOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationForceBottomOfStringOnRock != null)
                {
                    Guid ID = signals_.StandardDeviationForceBottomOfStringOnRock.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableBottomOfStringRockForceThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBottomOfStringRockForce, code);
                        }
                    }
                }
                if (signals_.ForceHoleOpenerOnRock != null)
                {
                    Guid ID = signals_.ForceHoleOpenerOnRock.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableBottomOfStringRockForceThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.OnBottomHoleOpener, code);
                        }
                    }
                }
                if (signals_.RotationaVelocityBottomOfString != null)
                {
                    Guid ID = signals_.RotationaVelocityBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RotationalVelocityBottomOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationRotationalVelocityBottomOfString != null)
                {
                    Guid ID = signals_.StandardDeviationRotationalVelocityBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableRotationalVelocityBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableRotationalVelocityBottomOfString, code);
                        }
                    }
                }
                if (signals_.FlowCuttingsBottomHole != null)
                {
                    Guid ID = signals_.FlowCuttingsBottomHole.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableCuttingsFlowThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Drilling, code);
                        }
                    }
                }
                if (signals_.FlowCuttingsTopOfRateHole != null)
                {
                    Guid ID = signals_.FlowCuttingsTopOfRateHole.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableCuttingsFlowThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.HoleOpening, code);
                        }
                    }
                }
                if (signals_.AxialVelocityBottomOfString != null)
                {
                    Guid ID = signals_.AxialVelocityBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AxialVelocityBottomOfString, code);
                        }
                    }
                }
                if (signals_.StandardDeviationAxialVelocityBottomOfString != null)
                {
                    Guid ID = signals_.StandardDeviationAxialVelocityBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableAxialVelocityBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableAxialVelocityBottomOfString, code);
                        }
                    }
                }
                if (signals_.FlowBottomOfString != null)
                {
                    Guid ID = signals_.FlowBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.StableFlowBottomOfStringThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowBottomOfString, code);
                        }
                    }
                }
                if (signals_.StableFlowBottomOfString != null)
                {
                    Guid ID = signals_.StableFlowBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowBottomOfStringThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowBottomOfString, code);
                        }
                    }
                }
                if (signals_.FlowHoleOpener != null)
                {
                    Guid ID = signals_.FlowHoleOpener.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowHoleOpenerThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.StableFlowHoleOpenerThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FlowHoleOpener, code);
                        }
                    }
                }
                if (signals_.StableFlowHoleOpener != null)
                {
                    Guid ID = signals_.StableFlowHoleOpener.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowHoleOpenerThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFlowHoleOpener, code);
                        }

                    }
                }
                if (signals_.ForceOnLedge != null)
                {
                    Guid ID = signals_.ForceOnLedge.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0,thresholds_.ForceOnLedgeThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.ForceOnLedgeThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LedgeKeySeat, code);
                        }
                    }
                }
                if (signals_.ForceOnCuttingsBed != null)
                {
                    Guid ID = signals_.ForceOnCuttingsBed.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.ForceOnCuttingsBedThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.ForceOnCuttingsBedThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CuttingsBed, code);
                        }
                    }
                }
                if (signals_.ForceDifferentialSticking != null)
                {
                    Guid ID = signals_.ForceDifferentialSticking.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.ForceDifferentialStickingThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.DifferentialSticking, code);
                        }
                    }
                }
                if (signals_.TensionTopOfString != null && signals_.MinimumTensionForTwistOffDetection != null)
                {
                    Guid ID1 = signals_.TensionTopOfString.ID;
                    Guid ID2 = signals_.MinimumTensionForTwistOffDetection.ID;
                    if (signalDictionary_.ContainsKey(ID1) && signalDictionary_.ContainsKey(ID2))
                    {
                        SignalMapping mapping1 = signalDictionary_[ID1];
                        SignalMapping mapping2 = signalDictionary_[ID2];
                        if (mapping1 != null && mapping1.Value != null && mapping1.Value is double &&
                            mapping2 != null && mapping2.Value != null && mapping2.Value is double)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping1.Value) && Numeric.IsDefined((double)mapping2.Value))
                            {
                                if (Numeric.GE((double)mapping1.Value, (double)mapping2.Value))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.TwistOffBackOff, code);
                        }
                    }
                }
                if (signals_.FlowFluidFromOrToFormation != null)
                {
                    Guid ID = signals_.FlowFluidFromOrToFormation.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.FluidFlowFormationThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.FluidFlowFormationThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WellIntegrity, code);
                        }
                    }
                }
                if (signals_.FlowFormationFluidAnnulusOutlet != null)
                {
                    Guid ID = signals_.FlowFormationFluidAnnulusOutlet.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.FluidFlowFormationThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationFluidAtAnnulusOutlet, code);
                        }
                    }
                }
                if (signals_.FlowCavingsFromFormation != null)
                {
                    Guid ID = signals_.FlowCavingsFromFormation.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.FlowCavingsFromFormationThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationCollapse, code);
                        }
                    }
                }
                if (signals_.FlowCavingsAnnulusOutlet != null)
                {
                    Guid ID = signals_.FlowCavingsAnnulusOutlet.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.FlowCavingsFromFormationThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CavingsAtAnnulusOutlet, code);
                        }
                    }
                }
                if (signals_.FlowPipeToAnnulus != null)
                {
                    Guid ID = signals_.FlowPipeToAnnulus.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.FlowPipeToAnnulusThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PipeWashout, code);
                        }
                    }
                }
                if (signals_.WhirlRateBottomOfString != null)
                {
                    Guid ID = signals_.WhirlRateBottomOfString.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.WhirlRateThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.WhirlRateThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlBottomOfString, code);
                        }
                    }
                }
                if (signals_.WhirlRateHoleOpener != null)
                {
                    Guid ID = signals_.WhirlRateHoleOpener.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.WhirlRateThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GT((double)mapping.Value, 0, thresholds_.WhirlRateThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.WhirlHoleOpener, code);
                        }
                    }
                }
                if (signals_.DifferentialPressureFloatValve != null)
                {
                    Guid ID = signals_.DifferentialPressureFloatValve.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.LT((double)mapping.Value, thresholds_.MinimumPressureFloatValve))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FloatSub, code);
                        }

                    }
                }
                if (signals_.UnderReamerOpen != null)
                {
                    Guid ID = signals_.UnderReamerOpen.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.UnderReamer, code);
                        }

                    }
                }
                if (signals_.CirculationSubOpen != null)
                {
                    Guid ID = signals_.CirculationSubOpen.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.CirculationSub, code);
                        }

                    }
                }
                if (signals_.PortedFloatOpen != null)
                {
                    Guid ID = signals_.PortedFloatOpen.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.PortedFloat, code);
                        }

                    }
                }
                if (signals_.WhipstockAttached != null)
                {
                    Guid ID = signals_.WhipstockAttached.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Whipstock, code);
                        }

                    }
                }
                if (signals_.PlugAttached != null)
                {
                    Guid ID = signals_.PlugAttached.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Plug, code);
                        }

                    }
                }
                if (signals_.LinerAttached != null)
                {
                    Guid ID = signals_.LinerAttached.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.Liner, code);
                        }

                    }
                }
                if (signals_.FlowBoosterPump != null)
                {
                    Guid ID = signals_.FlowBoosterPump.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowBoosterPumpThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BoosterPumping, code);
                        }

                    }
                }
                if (signals_.StandardDeviationFlowBoosterPump != null)
                {
                    Guid ID = signals_.StandardDeviationFlowBoosterPump.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowBoosterPumpThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBoosterPumping, code);
                        }

                    }
                }
                if (signals_.FlowBackPressurePump != null)
                {
                    Guid ID = signals_.FlowBackPressurePump.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowBackPressurePumpThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BackPressurePumping, code);
                        }

                    }
                }
                if (signals_.StandardDeviationFlowBackPressurePump != null)
                {
                    Guid ID = signals_.StandardDeviationFlowBackPressurePump.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowBackPressurePumpThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableBackPressurePumping, code);
                        }

                    }
                }
                if (signals_.OpeningMPDChoke != null)
                {
                    Guid ID = signals_.OpeningMPDChoke.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.LE((double)mapping.Value, 0))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GE((double)mapping.Value, 1))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MPDChokeOpening, code);
                        }

                    }
                }
                if (signals_.DifferentialPressureRCD != null)
                {
                    Guid ID = signals_.DifferentialPressureRCD.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.MinimumDifferentialPressureRCDSealingThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.RCDSealing, code);
                        }

                    }
                }
                if (signals_.IsolationSealActivated != null)
                {
                    Guid ID = signals_.IsolationSealActivated.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if (!(bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSeal, code);
                        }

                    }
                }
                if (signals_.DifferentialPressureIsolationSeal != null)
                {
                    Guid ID = signals_.DifferentialPressureIsolationSeal.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.MinimumDifferentialPressureSealBalanceThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.IsolationSealPressureBalance, code);
                        }

                    }
                }
                if (signals_.BearingAssemblyLatched != null)
                {
                    Guid ID = signals_.BearingAssemblyLatched.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if (!(bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.BearingAssemblyLatched, code);
                        }

                    }
                }
                if (signals_.ScreenMPDChokePlugged != null)
                {
                    Guid ID = signals_.ScreenMPDChokePlugged.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if ((bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.ScreenMPDChokePlugged, code);
                        }
                    }
                }
                if (signals_.MainFlowPathMPDEstablished != null)
                {
                    Guid ID = signals_.MainFlowPathMPDEstablished.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if (!(bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.MainFlowPathStable, code);
                        }
                    }
                }
                if (signals_.AlternateFlowPathMPDEstablished != null)
                {
                    Guid ID = signals_.AlternateFlowPathMPDEstablished.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is bool)
                        {
                            uint code = 0;
                            if (!(bool)mapping.Value)
                            {
                                code = 1;
                            }
                            else
                            {
                                code = 2;
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.AlternateFlowPathStable, code);
                        }
                    }
                }
                if (signals_.FlowFillPumpDGD != null)
                {
                    Guid ID = signals_.FlowFillPumpDGD.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowFillPumpDGDThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FillPumpDGD, code);
                        }

                    }
                }
                if (signals_.StandardDeviationFlowFillPumpDGD != null)
                {
                    Guid ID = signals_.StandardDeviationFlowFillPumpDGD.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowFillPumpDGDThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableFillPumpDGD, code);
                        }

                    }
                }
                if (signals_.FlowLiftPumpDGD != null)
                {
                    Guid ID = signals_.FlowLiftPumpDGD.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.EQ((double)mapping.Value, 0, thresholds_.StableFlowLiftPumpDGDThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.LiftPumpDGD, code);
                        }

                    }
                }
                if (signals_.StandardDeviationFlowLiftPumpDGD != null)
                {
                    Guid ID = signals_.StandardDeviationFlowLiftPumpDGD.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, thresholds_.StableFlowFillPumpDGDThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.StableLiftPumpDGD, code);
                        }
                    }
                }
                if (signals_.UCS != null)
                {
                    Guid ID = signals_.UCS.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.LT((double)mapping.Value, thresholds_.HardStringerThreshold))
                                {
                                    code = 1;
                                }
                                else
                                {
                                    code = 2;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.InsideHardStringer, code);
                        }

                    }
                }
                if (signals_.UCSSlope != null)
                {
                    Guid ID = signals_.UCSSlope.ID;
                    if (signalDictionary_.ContainsKey(ID))
                    {
                        SignalMapping mapping = signalDictionary_[ID];
                        if (mapping != null && mapping.Value != null && mapping.Value is double && thresholds_ != null)
                        {
                            uint code = 0;
                            if (Numeric.IsDefined((double)mapping.Value))
                            {
                                if (Numeric.GT((double)mapping.Value, -thresholds_.ChangeOfFormationUCSSlopeThreshold) &&
                                    Numeric.LT((double)mapping.Value, thresholds_.ChangeOfFormationUCSSlopeThreshold))
                                {
                                    code = 1;
                                }
                                else if (Numeric.GE((double)mapping.Value, thresholds_.ChangeOfFormationUCSSlopeThreshold))
                                {
                                    code = 2;
                                }
                                else
                                {
                                    code = 3;
                                }
                            }
                            UpdateMicroState(ref microStates, MicroStates.MicroStateIndex.FormationChange, code);
                        }

                    }
                }
            }
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

            return changed;
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