// See https://aka.ms/new-console-template for more information

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Text;

namespace DWIS.MicroState.MQTTSignalReader
{
    public class SignalReader
    {
        private IManagedMqttClient mqttClient_;

        static async Task Main(string[] args)
        {
            SignalReader playBack = new SignalReader();
            Console.WriteLine("when finished hit return");
            Console.ReadLine();
        }
        public SignalReader()
        {
            ConnectAndSubscribeAsync();
        }

        public async Task ConnectAndSubscribeAsync()
        {
            // Configure MQTT client options
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithTcpServer("172.30.49.215", 1883) // Replace with your MQTT broker details
                        .Build())
                    .Build();

            // Create MQTT client
            mqttClient_ = new MqttFactory().CreateManagedMqttClient();

            // Wire up event handlers
            mqttClient_.ApplicationMessageReceivedAsync += HandleMqttMessageReceived;

            // Connect and subscribe to topic
            await mqttClient_.StartAsync(options);
            await mqttClient_.SubscribeAsync("Insite_3474bc48-5b59-471b-9f8f-71cfc5653869/IOTV2PRIMARY/sensor_TimeSDLFast_a9cfe4af-6e73-4696-82eb-485f76da576b ");

        }

        private async Task HandleMqttMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

            // Deserialize the JSON payload
            var message = JsonConvert.DeserializeObject<Payload>(payload);

            // Process the message as needed
            Console.WriteLine($"Received MQTT message: {message.info.timestamp}, {message.info.sequenceNo}");
        }
    }
}

