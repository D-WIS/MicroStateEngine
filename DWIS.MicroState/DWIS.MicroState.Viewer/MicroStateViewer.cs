using MQTTnet.Extensions.ManagedClient;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System.Text;
using DWIS.MicroState.MQTT;
using DWIS.MicroState.ModelShared;

namespace DWIS.MicroState.Viewer
{
    public partial class MicroStateViewer : Form
    {
        private IManagedMqttClient mqttReceiverClient_;
        private MicroStates currentMicroStates_;
        private object lock_ = new object();

        public MicroStateViewer()
        {
            InitializeComponent();
            ConnectAndSubscribeReceiverAsync();
        }

        public async Task ConnectAndSubscribeReceiverAsync()
        {
            // Configure MQTT client options
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithTcpServer("localhost", 707) // Replace with your MQTT broker details
                    .Build())
                .Build();

            // Create MQTT client
            mqttReceiverClient_ = new MqttFactory().CreateManagedMqttClient();

            // Wire up event handlers
            mqttReceiverClient_.ApplicationMessageReceivedAsync += HandleMqttMessageReceived;

            // Connect and subscribe to topic
            await mqttReceiverClient_.StartAsync(options);
            await mqttReceiverClient_.SubscribeAsync(Topics.CurrentMicroStates);

            // Additional setup or logic can be added here
            RefreshConnected();
        }

        public void RefreshConnected()
        {
            if (InvokeRequired)
            {
                connectedButton.Invoke(new Action(RefreshConnected_));
            }
            else
            {
                RefreshConnected_();
            }
        }

        public void RefreshConnected_()
        {
            connectedButton.BackColor = Color.LightGreen;
        }

        private async Task HandleMqttMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            if (eventArgs != null)
            {
                if (eventArgs.ApplicationMessage != null)
                {

                    string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                    if (eventArgs.ApplicationMessage.Topic == Topics.CurrentMicroStates)
                    {
                        // Deserialize the JSON payload
                        var currentMicroStates = JsonConvert.DeserializeObject<MicroStates>(payload);
                        lock (lock_)
                        {
                            currentMicroStates_ = currentMicroStates;
                        }
                        RefreshDisplay();
                    }
                }
            }
        }
        private void RefreshDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshDisplay_));
            }
            else
            {
                RefreshDisplay_();
            }
        }
        private void RefreshDisplay_()
        {
            MicroStates currentMicroStates = new MicroStates();
            lock (lock_)
            {
                currentMicroStates.Part1 = currentMicroStates_.Part1;
                currentMicroStates.Part2 = currentMicroStates_.Part2;
                currentMicroStates.Part3 = currentMicroStates_.Part3;
                currentMicroStates.Part4 = currentMicroStates_.Part4;
                currentMicroStates.Part5 = currentMicroStates_.Part5;
            }
            List<Button> buttons = FindButtons(this);
            if (buttons != null)
            {
                foreach (var choice in Enum.GetValues(typeof(MicroStateIndex))) 
                {
                    string choiceName = Enum.GetName(typeof(MicroStateIndex), choice);
                    if (!string.IsNullOrEmpty(choiceName))
                    {
                        // find the corresponding button
                        foreach (var button in buttons)
                        {
                            if (!string.IsNullOrEmpty(button.Name) && button.Name.Contains(choiceName))
                            {
                                // decode the value
                                uint val = 0;
                                // which part
                                int part = (int)choice / 16;
                                int pos = 2 * ((int)choice % 16);
                                switch (part)
                                {
                                    case 0:
                                        val = (uint)currentMicroStates.Part1;
                                        break;
                                    case 1:
                                        val = (uint)currentMicroStates.Part2;
                                        break;
                                    case 2:
                                        val = (uint)currentMicroStates.Part3;
                                        break;
                                    case 3:
                                        val = (uint)currentMicroStates.Part4;
                                        break;
                                    default:
                                        val = (uint)currentMicroStates.Part5;
                                        break;
                                }
                                val = val >> pos;
                                val &= 0x00000003;
                                // change the color of the button
                                switch (val)
                                {
                                    case 1:
                                        button.BackColor = Color.Red;
                                        break;
                                    case 2:
                                        button.BackColor = Color.Green;
                                        break;
                                    case 3:
                                        button.BackColor = Color.Blue;
                                        break;
                                    default:
                                        button.BackColor = Color.Gray;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<Button> FindButtons(Control container)
        {
            List<Button> buttons = new List<Button>();

            // Iterate through the controls collection
            foreach (Control control in container.Controls)
            {
                // Check if the control is a button
                if (control is Button button)
                {
                    buttons.Add(button);
                }

                // If the control has child controls, recursively search for buttons
                if (control.HasChildren)
                {
                    buttons.AddRange(FindButtons(control));
                }
            }

            return buttons;
        }
    }


}
