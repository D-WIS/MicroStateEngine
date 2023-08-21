using DWIS.MicroState.MQTT;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using OSDC.DotnetLibraries.General.Common;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;

namespace DWIS.MicroState.SignalGenerator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Configuration Configuration { get; set; } = new Configuration();
        private static int TCPPort_ = 12345;
        private IManagedMqttClient mqttReceiverClient_;
        public IMqttClient mqttClient_;
        private List<string> subscribedSignalTopic_ = new List<string>();
        private object lock_ = new object();
        private double?[] scalarSignals_ = new double?[60];
        private bool?[] boolSignals_ = new bool?[20];


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            Initialize();
        }

        private async Task Initialize()
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
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectAndSubscribeEmitterAsync();
            if (mqttClient_ != null)
            {
                TcpListener listener = new TcpListener(TCPPort_);
                try
                {
                    // Start listening for incoming connections
                    listener.Start();
                    _logger.LogInformation("Server listnening on port " + TCPPort_);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        // Accept a client connection
                        TcpClient client = listener.AcceptTcpClient();

                        // Handle the client connection asynchronously
                        await HandleClientAsync(client);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            }
            if (mqttClient_ != null)
            {
                await mqttClient_.DisconnectAsync();
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];

                // Read the data from the client
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                // Convert the received bytes to a string
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                for (int i = 0; i < scalarSignals_.Length; i++)
                {
                    scalarSignals_[i] = Numeric.UNDEF_DOUBLE;
                }
                if (!string.IsNullOrEmpty(receivedMessage))
                {
                    string[] tokens = receivedMessage.Split('\t');
                    if (tokens != null && tokens.Length > 0)
                    {
                        double? flowrateIn;
                        if (tokens.Length >= 1 && TryParse(tokens[0], out flowrateIn))
                        {
                            scalarSignals_[4] = flowrateIn;
                        }
                        double? flowrateInStdDev;
                        if (tokens.Length >= 2 && TryParse(tokens[1], out flowrateInStdDev))
                        {
                            scalarSignals_[5] = flowrateInStdDev;
                        }
                        double? flowrateOut;
                        if (tokens.Length >= 3 && TryParse(tokens[2], out flowrateOut))
                        {
                            scalarSignals_[14] = flowrateOut;
                        }
                        double? flowrateOutStdDev;
                        if (tokens.Length >= 4 && TryParse(tokens[3], out flowrateOutStdDev))
                        {
                            scalarSignals_[15] = flowrateOutStdDev;
                        }
                        double? flowrateBit;
                        if (tokens.Length >= 5 && TryParse(tokens[4], out flowrateBit))
                        {
                            scalarSignals_[26] = flowrateBit;
                        }
                        double? flowrateBitStdDev;
                        if (tokens.Length >= 6 && TryParse(tokens[5], out flowrateBitStdDev))
                        {
                            scalarSignals_[27] = flowrateBitStdDev;
                        }
                        double? flowrateHoleOpener;
                        if (tokens.Length >= 7 && TryParse(tokens[6], out flowrateHoleOpener))
                        {
                            scalarSignals_[28] = flowrateHoleOpener;
                        }
                        double? flowrateHoleOpenerStdDev;
                        if (tokens.Length >= 8 && TryParse(tokens[7], out flowrateHoleOpenerStdDev))
                        {
                            scalarSignals_[29] = flowrateHoleOpenerStdDev;
                        }
                        double? topOfStringAxialVelocity;
                        if (tokens.Length >= 9 && TryParse(tokens[8], out topOfStringAxialVelocity))
                        {
                            scalarSignals_[0] = topOfStringAxialVelocity;
                        }
                        double? topOfStringAxialVelocityStdDev;
                        if (tokens.Length >= 10 && TryParse(tokens[9], out topOfStringAxialVelocityStdDev))
                        {
                            scalarSignals_[1] = topOfStringAxialVelocityStdDev;
                        }
                        double? topOfStringRotationalVelocity;
                        if (tokens.Length >= 11 && TryParse(tokens[10], out topOfStringRotationalVelocity))
                        {
                            scalarSignals_[2] = topOfStringRotationalVelocity;
                        }
                        double? topOfStringRotationalVelocityStdDev;
                        if (tokens.Length >= 12 && TryParse(tokens[11], out topOfStringRotationalVelocityStdDev))
                        {
                            scalarSignals_[3] = topOfStringRotationalVelocityStdDev;
                        }
                        double? tensionTopOfString;
                        if (tokens.Length >= 13 && TryParse(tokens[12], out tensionTopOfString))
                        {
                            scalarSignals_[6] = tensionTopOfString;
                        }
                        double? forceBottomTopDrive;
                        if (tokens.Length >= 14 && TryParse(tokens[13], out forceBottomTopDrive))
                        {
                            scalarSignals_[7] = forceBottomTopDrive;
                        }
                        double? forceElevator;
                        if (tokens.Length >= 15 && TryParse(tokens[14], out forceElevator))
                        {
                            scalarSignals_[8] = forceElevator;
                        }
                        double? tensionTopOfStringStdDev;
                        if (tokens.Length >= 16 && TryParse(tokens[15], out tensionTopOfStringStdDev))
                        {
                            scalarSignals_[9] = tensionTopOfStringStdDev;
                        }
                        double? pressureTopOfString;
                        if (tokens.Length >= 17 && TryParse(tokens[16], out pressureTopOfString))
                        {
                            scalarSignals_[10] = pressureTopOfString;
                        }
                        double? pressureTopOfStringStdDev;
                        if (tokens.Length >= 18 && TryParse(tokens[17], out pressureTopOfStringStdDev))
                        {
                            scalarSignals_[11] = pressureTopOfStringStdDev;
                        }
                        double? torqueTopOfString;
                        if (tokens.Length >= 19 && TryParse(tokens[18], out torqueTopOfString))
                        {
                            scalarSignals_[12] = torqueTopOfString;
                        }
                        double? torqueTopOfStringStdDev;
                        if (tokens.Length >= 20 && TryParse(tokens[19], out torqueTopOfStringStdDev))
                        {
                            scalarSignals_[13] = torqueTopOfStringStdDev;
                        }
                        double? bottomOfStringAxialVelocity;
                        if (tokens.Length >= 21 && TryParse(tokens[20], out bottomOfStringAxialVelocity))
                        {
                            scalarSignals_[24] = bottomOfStringAxialVelocity;
                        }
                        double? bottomOfStringAxialVelocityStdDev;
                        if (tokens.Length >= 22 && TryParse(tokens[21], out bottomOfStringAxialVelocityStdDev))
                        {
                            scalarSignals_[25] = bottomOfStringAxialVelocityStdDev;
                        }
                        double? bottomOfStringRotationalVelocity;
                        if (tokens.Length >= 23 && TryParse(tokens[22], out bottomOfStringRotationalVelocity))
                        {
                            scalarSignals_[20] = bottomOfStringRotationalVelocity;
                        }
                        double? bottomOfStringRotationalVelocityStdDev;
                        if (tokens.Length >= 24 && TryParse(tokens[23], out bottomOfStringRotationalVelocityStdDev))
                        {
                            scalarSignals_[21] = bottomOfStringRotationalVelocityStdDev;
                        }
                        double? forceBottomOfString;
                        if (tokens.Length >= 25 && TryParse(tokens[24], out forceBottomOfString))
                        {
                            scalarSignals_[17] = forceBottomOfString;
                        }
                        double? forceBottomOfStringStdDev;
                        if (tokens.Length >= 26 && TryParse(tokens[25], out forceBottomOfStringStdDev))
                        {
                            scalarSignals_[18] = forceBottomOfStringStdDev;
                        }
                        double? forceHoleOpener;
                        if (tokens.Length >= 27 && TryParse(tokens[26], out forceHoleOpener))
                        {
                            scalarSignals_[19] = forceHoleOpener;
                        }
                        double? cuttingsFlowAtBit;
                        if (tokens.Length >= 29 && TryParse(tokens[28], out cuttingsFlowAtBit))
                        {
                            scalarSignals_[22] = cuttingsFlowAtBit;
                        }
                        double? cuttingsFlowAtHoleOpener;
                        if (tokens.Length >= 30 && TryParse(tokens[29], out cuttingsFlowAtHoleOpener))
                        {
                            scalarSignals_[23] = cuttingsFlowAtHoleOpener;
                        }
                        double? cuttingsFlowAtAnnulusOutlet;
                        if (tokens.Length >= 31 && TryParse(tokens[30], out cuttingsFlowAtAnnulusOutlet))
                        {
                            scalarSignals_[16] = cuttingsFlowAtAnnulusOutlet;
                        }
                        double? UCSAvg;
                        if (tokens.Length >= 32 && TryParse(tokens[31], out UCSAvg))
                        {
                            scalarSignals_[52] = UCSAvg;
                        }
                        double? UCSSlope;
                        if (tokens.Length >= 33 && TryParse(tokens[32], out UCSSlope))
                        {
                            scalarSignals_[53] = UCSSlope;
                        }
                        double? forceOnLedge;
                        if (tokens.Length >= 34 && TryParse(tokens[33], out forceOnLedge))
                        {
                            scalarSignals_[30] = forceOnLedge;
                        }
                        double? forceOnCuttingsBed;
                        if (tokens.Length >= 35 && TryParse(tokens[34], out forceOnCuttingsBed))
                        {
                            scalarSignals_[31] = forceOnCuttingsBed;
                        }
                        double? forceDifferentialSticking;
                        if (tokens.Length >= 36 && TryParse(tokens[35], out forceDifferentialSticking))
                        {
                            scalarSignals_[32] = forceDifferentialSticking;
                        }
                        double? flowFluidFromOrToFormation;
                        if (tokens.Length >= 37 && TryParse(tokens[36], out flowFluidFromOrToFormation))
                        {
                            scalarSignals_[33] = flowFluidFromOrToFormation;
                        }
                        double? flowFormationFluidAnnulusOutlet;
                        if (tokens.Length >= 38 && TryParse(tokens[37], out flowFormationFluidAnnulusOutlet))
                        {
                            scalarSignals_[34] = flowFormationFluidAnnulusOutlet;
                        }
                        double? flowCavingsFromFormation;
                        if (tokens.Length >= 39 && TryParse(tokens[38], out flowCavingsFromFormation))
                        {
                            scalarSignals_[35] = flowCavingsFromFormation;
                        }
                        double? flowCavingsAnnulusOutlet;
                        if (tokens.Length >= 40 && TryParse(tokens[39], out flowCavingsAnnulusOutlet))
                        {
                            scalarSignals_[36] = flowCavingsAnnulusOutlet;
                        }
                        double? flowPipeToAnnulus;
                        if (tokens.Length >= 41 && TryParse(tokens[40], out flowPipeToAnnulus))
                        {
                            scalarSignals_[37] = flowPipeToAnnulus;
                        }
                        double? whirlRateBottomOfString;
                        if (tokens.Length >= 42 && TryParse(tokens[41], out whirlRateBottomOfString))
                        {
                            scalarSignals_[38] = whirlRateBottomOfString;
                        }
                        double? whirlRateHoleOpener;
                        if (tokens.Length >= 43 && TryParse(tokens[42], out whirlRateHoleOpener))
                        {
                            scalarSignals_[39] = whirlRateHoleOpener;
                        }
                        double? differentialPressureFloatValve;
                        if (tokens.Length >= 44 && TryParse(tokens[43], out differentialPressureFloatValve))
                        {
                            scalarSignals_[40] = differentialPressureFloatValve;
                        }
                        double? flowBoosterPump;
                        if (tokens.Length >= 45 && TryParse(tokens[44], out flowBoosterPump))
                        {
                            scalarSignals_[41] = flowBoosterPump;
                        }
                        double? standardDeviationFlowBoosterPump;
                        if (tokens.Length >= 46 && TryParse(tokens[45], out standardDeviationFlowBoosterPump))
                        {
                            scalarSignals_[42] = standardDeviationFlowBoosterPump;
                        }
                        double? flowBackPressurePump;
                        if (tokens.Length >= 47 && TryParse(tokens[46], out flowBackPressurePump))
                        {
                            scalarSignals_[43] = flowBackPressurePump;
                        }
                        double? standardDeviationFlowBackPressurePump;
                        if (tokens.Length >= 48 && TryParse(tokens[47], out standardDeviationFlowBackPressurePump))
                        {
                            scalarSignals_[44] = standardDeviationFlowBackPressurePump;
                        }
                        double? openingMPDChoke;
                        if (tokens.Length >= 49 && TryParse(tokens[48], out openingMPDChoke))
                        {
                            scalarSignals_[45] = openingMPDChoke;
                        }
                        double? differentialPressureRCD;
                        if (tokens.Length >= 50 && TryParse(tokens[49], out differentialPressureRCD))
                        {
                            scalarSignals_[46] = differentialPressureRCD;
                        }
                        double? differentialPressureIsolationSeal;
                        if (tokens.Length >= 51 && TryParse(tokens[50], out differentialPressureIsolationSeal))
                        {
                            scalarSignals_[47] = differentialPressureIsolationSeal;
                        }
                        double? flowFillPumpDGD;
                        if (tokens.Length >= 52 && TryParse(tokens[51], out flowFillPumpDGD))
                        {
                            scalarSignals_[48] = flowFillPumpDGD;
                        }
                        double? flowLiftPumpDGD;
                        if (tokens.Length >= 53 && TryParse(tokens[52], out flowLiftPumpDGD))
                        {
                            scalarSignals_[49] = flowLiftPumpDGD;
                        }
                        double? standardDeviationFlowFillPumpDGD;
                        if (tokens.Length >= 54 && TryParse(tokens[53], out standardDeviationFlowFillPumpDGD))
                        {
                            scalarSignals_[50] = standardDeviationFlowFillPumpDGD;
                        }
                        double? standardDeviationFlowLiftPumpDGD;
                        if (tokens.Length >= 55 && TryParse(tokens[54], out standardDeviationFlowLiftPumpDGD))
                        {
                            scalarSignals_[51] = standardDeviationFlowLiftPumpDGD;
                        }
                        double? minimumTensionForTwistOffDetection;
                        if (tokens.Length >= 56 && TryParse(tokens[55], out minimumTensionForTwistOffDetection))
                        {
                            scalarSignals_[54] = minimumTensionForTwistOffDetection;
                        }

                        bool? CirculationSubOpen;
                        if (tokens.Length >= 57 && TryParse(tokens[56], out CirculationSubOpen))
                        {
                            boolSignals_[0] = CirculationSubOpen;
                        }
                        bool? PortedFloatOpen;
                        if (tokens.Length >= 58 && TryParse(tokens[57], out PortedFloatOpen))
                        {
                            boolSignals_[1] = PortedFloatOpen;
                        }
                        bool? WhipstockAttached;
                        if (tokens.Length >= 59 && TryParse(tokens[58], out WhipstockAttached))
                        {
                            boolSignals_[2] = WhipstockAttached;
                        }
                        bool? PlugAttached;
                        if (tokens.Length >= 60 && TryParse(tokens[59], out PlugAttached))
                        {
                            boolSignals_[3] = PlugAttached;
                        }
                        bool? LinerAttached;
                        if (tokens.Length >= 61 && TryParse(tokens[60], out LinerAttached))
                        {
                            boolSignals_[4] = LinerAttached;
                        }
                        bool? IsolationSealActivated;
                        if (tokens.Length >= 62 && TryParse(tokens[61], out IsolationSealActivated))
                        {
                            boolSignals_[5] = IsolationSealActivated;
                        }
                        bool? BearingAssemblyLatched;
                        if (tokens.Length >= 63 && TryParse(tokens[62], out BearingAssemblyLatched))
                        {
                            boolSignals_[6] = BearingAssemblyLatched;
                        }
                        bool? ScreenMPDChokePlugged;
                        if (tokens.Length >= 64 && TryParse(tokens[63], out ScreenMPDChokePlugged))
                        {
                            boolSignals_[7] = ScreenMPDChokePlugged;
                        }
                        bool? MainFlowPathMPDEstablished;
                        if (tokens.Length >= 65 && TryParse(tokens[64], out MainFlowPathMPDEstablished))
                        {
                            boolSignals_[8] = MainFlowPathMPDEstablished;
                        }
                        bool? AlternateFlowPathMPDEstablished;
                        if (tokens.Length >= 66 && TryParse(tokens[65], out AlternateFlowPathMPDEstablished))
                        {
                            boolSignals_[9] = AlternateFlowPathMPDEstablished;
                        }
                    }
                }

                // process the received message here


                // Close the client connection
                client.Close();

                // Create MQTT message and publish
                string scalarSignalPayload = JsonConvert.SerializeObject(scalarSignals_);
                var scalarSignalsMQTTMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(Topics.SignalSourceScalars)
                    .WithPayload(scalarSignalPayload)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                    .WithRetainFlag(true)
                    .Build();
                await mqttClient_.PublishAsync(scalarSignalsMQTTMessage);

                string boolSignalPayload = JsonConvert.SerializeObject(boolSignals_);
                var boolSignalsMQTTMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(Topics.SignalSourceBooleans)
                    .WithPayload(boolSignalPayload)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                    .WithRetainFlag(true)
                    .Build();
                await mqttClient_.PublishAsync(boolSignalsMQTTMessage);

            }
        }

        private bool TryParse(string svalue, out double? dvalue)
        {
            if (string.IsNullOrEmpty(svalue))
            {
                dvalue = null;
                return true;
            }
            else if (svalue.ToLowerInvariant() == "null")
            {
                dvalue = null;
                return true;
            }
            else
            {
                double d;
                if (Numeric.TryParse(svalue, out d))
                {
                    dvalue = d;
                    return true;
                }
                else
                {
                    dvalue = null;
                    return false;
                }

            }
        }
        private bool TryParse(string svalue, out bool? bvalue)
        {
            if (string.IsNullOrEmpty(svalue))
            {
                bvalue = null;
                return true;
            }
            else if (svalue.ToLowerInvariant() == "null")
            {
                bvalue = null;
                return true;
            }
            else
            {
                bool b;
                if (Numeric.TryParse(svalue, out b))
                {
                    bvalue = b;
                    return true;
                }
                else
                {
                    bvalue = null;
                    return false;
                }

            }
        }

        public async Task ConnectAndSubscribeEmitterAsync()
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
            string scalarSignalPayload = JsonConvert.SerializeObject(scalarSignals_);
            var thresholdsMQTTMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topics.SignalSourceScalars)
                .WithPayload(scalarSignalPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag(true)
                .Build();
            await mqttClient_.PublishAsync(thresholdsMQTTMessage);

            string boolSignalPayload = JsonConvert.SerializeObject(boolSignals_);
            var signalsMQTTMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topics.SignalSourceBooleans)
                .WithPayload(boolSignalPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag(true)
                .Build();
            await mqttClient_.PublishAsync(signalsMQTTMessage);

        }
    }
}