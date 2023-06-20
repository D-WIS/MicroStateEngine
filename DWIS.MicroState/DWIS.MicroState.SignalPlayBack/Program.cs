// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System.Text;
using System.Timers;
using DWIS.MicroState.MQTT;

namespace DWIS.MicroState.SignalPlayBack
{
    public class SignalPlayBack
    {
        private IMqttClient mqttClient_;

        static async Task Main(string[] args)
        {
            SignalPlayBack playBack = new SignalPlayBack();
            Console.WriteLine("when finished hit return");
            Console.ReadLine();
            if (playBack.mqttClient_ != null)
            {
                await playBack.mqttClient_.DisconnectAsync();
            }
        }
        public SignalPlayBack()
        {
            ConnectAndSubscribeAsync();
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += RefreshSignals;
            timer.AutoReset = true;
            timer.Start();
        }

        public async Task ConnectAndSubscribeAsync()
        {
            // Configure MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 707) // Replace with your MQTT broker details
                .Build();

            // Create MQTT client
            mqttClient_ = new MqttFactory().CreateMqttClient();

            // Connect to the broker
            await mqttClient_.ConnectAsync(options);

        }

        private async void RefreshSignals(object? sender, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            // Create and serialize the message payload
            double[] scalars = new double[100];
            for (int i = 0; i < scalars.Length; i++)
            {
                scalars[i] = rnd.NextDouble();
            }
            string scalarPayload = JsonConvert.SerializeObject(scalars);
            // Create MQTT message and publish
            var mqttScalarMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topics.SignalSourceScalars)
                .WithPayload(scalarPayload)
                .WithRetainFlag()
                .Build();
            await mqttClient_.PublishAsync(mqttScalarMessage);

            bool[] booleans = new bool[100];
            for (int i = 0; i < booleans.Length; i++)
            {
                booleans[i] = rnd.Next(2) != 0;
            }

            string booleanPayload = JsonConvert.SerializeObject(booleans);
            // Create MQTT message and publish
            var mqttBooleanMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topics.SignalSourceBooleans)
                .WithPayload(booleanPayload)
                .WithRetainFlag()
                .Build();
            await mqttClient_.PublishAsync(mqttBooleanMessage);

        }
    }
}