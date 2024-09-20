using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using DWIS.MicroState.ModelShared;
using DWIS.Client.ReferenceImplementation;
using DWIS.SPARQL.Utils;
using DWIS.Vocabulary.Schemas;

namespace DWIS.MicroState.Viewer
{
    public partial class MicroStateViewer : Form
    {
        private static readonly string microStatesQueryName_ = "DWIS:eRAP:MicroStatesQuery";
        private IManagedMqttClient mqttReceiverClient_;
        private MicroStates currentMicroStates_;
        private IOPCUADWISClient? DDHubClient_ = null;
        private AcquiredSignals? microStateSignals_ = null;
        private object lock_ = new object();

        public MicroStateViewer()
        {
            InitializeComponent();
            ConnectToDDHub();
            AcquireMicroStates();
            Thread thread = new Thread(Loop);
            thread.Start();
        }
        private void ConnectToDDHub()
        {
            try
            {
                DefaultDWISClientConfiguration defaultDWISClientConfiguration = new DefaultDWISClientConfiguration();
                defaultDWISClientConfiguration.UseWebAPI = false;
                defaultDWISClientConfiguration.ServerAddress = "opc.tcp://localhost:48030";
                DDHubClient_ = null; // new DWISClient(defaultDWISClientConfiguration, new UAApplicationConfiguration(), null, null, new DWIS.OPCUA.UALicenseManager.LicenseManager());
            }
            catch (Exception e)
            {

            }
        }

        private void AcquireMicroStates()
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
                if (result != null && result.Results != null && result.Results.Count > 0)
                {
                    microStateSignals_ = AcquiredSignals.CreateWithSubscription(new string[] { query }, new string[] { microStatesQueryName_ }, 0, DDHubClient_);
                }
                RefreshConnected();
            }
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

        private void Loop()
        {
            while (true)
            {
                DateTime d1 = DateTime.UtcNow;
                ManageChanges();
                DateTime d2 = DateTime.UtcNow;
                TimeSpan elapsed = d2 - d1;
                if (elapsed < TimeSpan.FromSeconds(1))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1) - elapsed);
                }
            }
        }
        private void ManageChanges()
        {
            if (microStateSignals_ != null && microStateSignals_.ContainsKey(microStatesQueryName_))
            {
                var result = microStateSignals_[microStatesQueryName_];
                if (result != null && result.Count > 0)
                {
                    AcquiredSignal signal = result[0];
                    if (signal != null)
                    {
                        string? json = signal.GetValue<string>();
                        if (!string.IsNullOrEmpty(json))
                        {
                            try
                            {
                                var currentMicroStates = JsonConvert.DeserializeObject<MicroStates>(json);
                                if (currentMicroStates != null)
                                {
                                    lock (lock_)
                                    {
                                        currentMicroStates_ = currentMicroStates;
                                    }
                                }
                                RefreshDisplay();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
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
                /*
                foreach (var choice in Enum.GetValues(typeof(MicroStateIndex)))
                {
                    string? choiceName = Enum.GetName(typeof(MicroStateIndex), choice);
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
                */
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
