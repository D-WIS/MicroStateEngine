using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.Vocabulary.Schemas;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;
using System.Text.Json;

namespace DWIS.MicroState.Model
{
    /// <summary>
    /// an enumeration of the microstate indices
    /// </summary>
    public enum MicroStateIndex
    {
        [Label("Axial Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        AxialVelocityTopOfString = 0,
        [Label("Stable Axial Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        StableAxialVelocityTopOfString, // 1
        [Label("Rotational Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        RotationalVelocityTopOfString, // 2
        [Label("Stable Rotational Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        StableRotationalVelocityTopOfString, // 3
        [Label("Flow at Top of String")]
        [GroupName("Top Side Boundaries")]
        FlowAtTopOfString, // 4
        [Label("Stable Flow at Top of String")]
        [GroupName("Top Side Boundaries")]
        StableFlowAtTopOfString, // 5
        [Label("Slip State")]
        [GroupName("Top Side Boundaries")]
        SlipState, // 6
        [Label("Stable Tension Top of String")]
        [GroupName("Top Side Boundaries")]
        StableTensionTopOfString, // 7
        [Label("Pressure Top of String")]
        [GroupName("Top Side Boundaries")]
        PressureTopOfString, // 8
        [Label("Stable Pressure Top of String")]
        [GroupName("Top Side Boundaries")]
        StablePressureTopOfString, // 9
        [Label("Torque Top of String")]
        [GroupName("Top Side Boundaries")]
        TorqueTopOfString, // 10
        [Label("Stable Torque Top of String")]
        [GroupName("Top Side Boundaries")]
        StableTorqueTopOfString, // 11
        [Label("Flow at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        FlowAtAnnulusOutlet, // 12
        [Label("Stable Flow at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        StableFlowAtAnnulusOutlet, // 13
        [Label("Cuttings Return at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        CuttingsReturnAtAnnulusOutlet, // 14
        [Label("On Bottom Bottom of String")]
        [GroupName("Downhole Boundaries")]
        OnBottomBottomOfString, // 15
        [Label("Stable Bottom of String Rock Force")]
        [GroupName("Downhole Boundaries")]
        StableBottomOfStringRockForce, // 16
        [Label("On Bottom Hole Opener")]
        [GroupName("Downhole Boundaries")]
        OnBottomHoleOpener, // 17
        [Label("Rotational Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        RotationalVelocityBottomOfString, // 18
        [Label("Stable Rotational Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        StableRotationalVelocityBottomOfString, // 19
        [Label("Drilling")]
        [GroupName("Downhole Boundaries")]
        Drilling, // 20
        [Label("Hole Opening")]
        [GroupName("Downhole Boundaries")]
        HoleOpening, // 21
        [Label("Axial Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        AxialVelocityBottomOfString, //22
        [Label("Stable Axial Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        StableAxialVelocityBottomOfString, // 23
        [Label("Flow Bottom of String")]
        [GroupName("Downhole Boundaries")]
        FlowBottomOfString, // 24
        [Label("Stable Flow Bottom of String")]
        [GroupName("Downhole Boundaries")]
        StableFlowBottomOfString, // 25
        [Label("Flow Hole Opener")]
        [GroupName("Downhole Boundaries")]
        FlowHoleOpener, // 26
        [Label("Stable Flow Hole Opener")]
        [GroupName("Downhole Boundaries")]
        StableFlowHoleOpener, // 27
        [Label("Ledge Key Seat")]
        [GroupName("Abnormal Downhole Boundaries")]
        LedgeKeySeat, // 28
        [Label("Cuttings Bed")]
        [GroupName("Abnormal Downhole Boundaries")]
        CuttingsBed, // 29
        [Label("Differential Sticking")]
        [GroupName("Abnormal Downhole Boundaries")]
        DifferentialSticking, // 30
        [Label("Twist-off Back-off")]
        [GroupName("Abnormal Downhole Boundaries")]
        TwistOffBackOff, // 31
        [Label("Well Integrity")]
        [GroupName("Abnormal Downhole Boundaries")]
        WellIntegrity, // 32
        [Label("Formation Fluid at Annulus Outlet")]
        [GroupName("Abnormal Downhole Boundaries")]
        FormationFluidAtAnnulusOutlet, // 33
        [Label("Formation Collapse")]
        [GroupName("Abnormal Downhole Boundaries")]
        FormationCollapse, // 34
        [Label("Cavings at Annulus Outlet")]
        [GroupName("Abnormal Downhole Boundaries")]
        CavingsAtAnnulusOutlet, // 35
        [Label("Pipe Washout")]
        [GroupName("Abnormal Downhole Boundaries")]
        PipeWashout, // 36
        [Label("Whirl Bottom of String")]
        [GroupName("Abnormal Downhole Boundaries")]
        WhirlBottomOfString, // 37
        [Label("Whirl Hole Opener")]
        [GroupName("Abnormal Downhole Boundaries")]
        WhirlHoleOpener, // 38
        [Label("Float-sub")]
        [GroupName("Downhole Element Change Boundaries")]
        FloatSub, // 39
        [Label("Under-reamer")]
        [GroupName("Downhole Element Change Boundaries")]
        UnderReamer, // 40
        [Label("Circulation-sub")]
        [GroupName("Downhole Element Change Boundaries")]
        CirculationSub, // 41
        [Label("Ported-float")]
        [GroupName("Downhole Element Change Boundaries")]
        PortedFloat, // 42
        [Label("Whipstock")]
        [GroupName("Downhole Element Change Boundaries")]
        Whipstock, // 43
        [Label("Plug")]
        [GroupName("Downhole Element Change Boundaries")]
        Plug, // 44
        [Label("Liner")]
        [GroupName("Downhole Element Change Boundaries")]
        Liner, // 45
        [Label("Booster Pumping")]
        [GroupName("Floating Rig Boundaries")]
        BoosterPumping, // 46
        [Label("Stable Booster Pumping")]
        [GroupName("Floating Rig Boundaries")]
        StableBoosterPumping, // 47
        [Label("Back Pressure Pumping")]
        [GroupName("Back-pressure MPD Boundaries")]
        BackPressurePumping, // 48
        [Label("Stable Back-pressure Pumping")]
        [GroupName("Back-pressure MPD Boundaries")]
        StableBackPressurePumping, // 49
        [Label("MPD Choke Opening")]
        [GroupName("Back-pressure MPD Boundaries")]
        MPDChokeOpening, // 50
        [Label("RCD Sealing")]
        [GroupName("Back-pressure MPD Boundaries")]
        RCDSealing, // 51
        [Label("Isolation Seal")]
        [GroupName("Back-pressure MPD Boundaries")]
        IsolationSeal, // 52
        [Label("Isolation Seal Pressure Balance")]
        [GroupName("Back-pressure MPD Boundaries")]
        IsolationSealPressureBalance, // 53
        [Label("Bearing Assembly Latched")]
        [GroupName("Back-pressure MPD Boundaries")]
        BearingAssemblyLatched, // 54
        [Label("Screen MPD Choke Plugged")]
        [GroupName("Back-pressure MPD Boundaries")]
        ScreenMPDChokePlugged, // 55
        [Label("Main Flow Path Stable")]
        [GroupName("Back-pressure MPD Boundaries")]
        MainFlowPathStable, // 56
        [Label("Alternate Flow Path Stable")]
        [GroupName("Back-pressure MPD Boundaries")]
        AlternateFlowPathStable, // 57
        [Label("Fill Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        FillPumpDGD, // 58
        [Label("Lift Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        LiftPumpDGD, // 59
        [Label("Stable Fill Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        StableFillPumpDGD, // 60
        [Label("Stable Lift Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        StableLiftPumpDGD, // 61
        [Label("Formation Change")]
        [GroupName("Downhole Boundaries")]
        FormationChange, // 62
        [Label("Inside Hard Stringer")]
        [GroupName("Downhole Boundaries")]
        InsideHardStringer, // 63
        [Label("Tool-joint #1 at Lowest Drill Height")]
        [GroupName("Top Side Boundaries")]
        ToolJoint1AtLowestDrillHeight, // 64
        [Label("Tool-joint #1 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        ToolJoint1AtStickUpHeight, // 65
        [Label("Tool-joint #2 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        ToolJoint2AtStickUpHeight, // 66
        [Label("Tool-joint #3 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        ToolJoint3AtStickUpHeight, // 67
        [Label("Tool-joint #4 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        ToolJoint4AtStickUpHeight, // 68
        [Label("Heave Compensation")]
        [GroupName("Floating Rig Boundaries")]
        HeaveCompensation, // 69
        [Label("Last Stand to Bottom Hole")]
        [GroupName("Downhole Boundaries")]
        LastStandToBottomHole, // 70
        [Label("Whirl in Drill-string")]
        [GroupName("Abnormal Downhole Boundaries")]
        WhirlInDrillString, // 71
        [Label("HFTO")]
        [GroupName("Abnormal Downhole Boundaries")]
        HFTO, // 72
        [Label("Axial Oscillations")]
        [GroupName("Abnormal Downhole Boundaries")]
        AxialOscillations, // 73
        [Label("Torsional Oscillations")]
        [GroupName("Abnormal Downhole Boundaries")]
        TorsionalOscillations, // 74
        [Label("Lateral Shocks in BHA")]
        [GroupName("Abnormal Downhole Boundaries")]
        LateralShocksInBHA, // 75
        [Label("Lateral Shocks in Drill-string")]
        [GroupName("Abnormal Downhole Boundaries")]
        LateralShocksInDrillString, // 76
        [Label("String Rotation Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        StringRotationImpeded, // 77
        [Label("Annulus Flow Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        AnnulusFlowImpeded, // 78
        [Label("String Flow Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        StringFlowImpeded // 79
    }
    [AccessToVariable(CommonProperty.VariableAccessType.Assignable)]
    [SemanticTypeVariable("DeterministicState")]
    [SemanticFact("DeterministicState", Nouns.Enum.DynamicDrillingSignal)]
    [SemanticFact("DeterministicState#01", Nouns.Enum.ComputedData)]
    [SemanticFact("DeterministicState#01", Nouns.Enum.JSonDataType)]
    [SemanticFact("DeterministicState#01", Verbs.Enum.HasDynamicValue, "DeterministicState")]
    [SemanticFact("DeterministicProcessState", Nouns.Enum.ProcessState)]
    [SemanticFact("DeterministicProcessState", Nouns.Enum.DeterministicModel)]
    [SemanticFact("DeterministicState#01", Verbs.Enum.IsGeneratedBy, "DeterministicProcessState")]
    [SemanticFact("ProcessStateInterpreter#01", Nouns.Enum.DWISDrillingProcessStateInterpreter)]
    [SemanticFact("DeterministicState#01", Verbs.Enum.IsProvidedBy, "ProcessStateInterpreter#01")]
    public struct MicroStates
    {
        private static string prefix_ = "DWIS:MicroState:DeterministicMicroStates:";
        private static string companyName_ = "DWIS";

        /// <summary>
        /// the time stamp in UTC when the state has been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }
        /// <summary>
        /// Part1: microstates from 0 to 15
        /// </summary>
        public int Part1 { get; set; }
        /// <summary>
        /// Part2: microstates from 16 to 31
        /// </summary>
        public int Part2 { get; set; }
        /// <summary>
        /// Part3: microstates from 32 to 47
        /// </summary>
        public int Part3 { get; set; }
        /// <summary>
        /// Part4: microstates from 48 to 63
        /// </summary>
        public int Part4 { get; set; }
        /// <summary>
        /// Part5: microstates from 64 to 79
        /// </summary>
        public int Part5 { get; set; }

        public bool Equals(MicroStates? other, bool useTimeStamp = true)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return 
                    (!useTimeStamp || this.TimeStampUTC.Equals(other.Value.TimeStampUTC)) &&
                    this.Part1 == other.Value.Part1 &&
                    this.Part2 == other.Value.Part2 &&
                    this.Part3 == other.Value.Part3 &&
                    this.Part4 == other.Value.Part4 &&
                    this.Part5 == other.Value.Part5;
            }
        }
        public void CopyTo(ref MicroStates dest)
        {
            dest.TimeStampUTC = TimeStampUTC;
            dest.Part1 = Part1;
            dest.Part2 = Part2;
            dest.Part3 = Part3;
            dest.Part4 = Part4;
            dest.Part5 = Part5;
        }

        public bool RegisterToBlackboard(IOPCUADWISClient? DWISClient, ref QueryResult? placeHolder)
        {
            bool ok = false;
            if (DWISClient != null)
            {
                Type type = GetType();
                Assembly assembly = type.Assembly;

                string? manifestName = type.FullName;
                if (!string.IsNullOrEmpty(manifestName))
                {
                    ManifestFile? manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, type.FullName, manifestName, companyName_, prefix_);
                    Dictionary<string, QuerySpecification>? queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName);
                    if (queries != null && queries.Count > 0 && manifestFile != null)
                    {
                        QueryResult? res = null;
                        List<List<string>> vars = new List<List<string>>();
                        foreach (var kvp in queries)
                        {
                            if (kvp.Value.Variables != null)
                            {
                                vars.Add(kvp.Value.Variables);
                            }
                            if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                            {
                                if (kvp.Value.Variables != null)
                                {
                                    vars.Add(kvp.Value.Variables);
                                }
                                var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                if (result != null && result.Results != null && result.Results.Count > 0)
                                {
                                    res = result;
                                    break;
                                }
                            }
                        }
                        List<string>? variables = Utils.CommonVariables(vars);
                        // if we couldn't find any answer then the manifest must be injected
                        if (res == null)
                        {
                            var r = DWISClient.Inject(manifestFile);
                            if (r != null && r.Success)
                            {
                                if (r.ProvidedVariables != null && r.ProvidedVariables.Count > 0)
                                {
                                    placeHolder = new QueryResult();
                                    QueryResultRow row = new QueryResultRow();
                                    List<NodeIdentifier> items = new List<NodeIdentifier>();
                                    placeHolder.VariablesHeader = variables;
                                    row.Items = items;
                                    foreach (var kvp in r.ProvidedVariables)
                                    {
                                        DWISClient.GetNameSpace(kvp.InjectedID.NameSpaceIndex, out string ns);
                                        items.Add(new NodeIdentifier() { ID = kvp.InjectedID.ID, NameSpace = ns });
                                    }
                                    placeHolder.Add(row);
                                }
                            }
                        }
                        else
                        {
                            // a manifest has already been injected.
                            placeHolder = res;
                            ok = true;
                        }
                    }
                }
            }
            return ok;
        }

        public bool SendToBlackboard(IOPCUADWISClient? DWISClient, QueryResult? placeHolder)
        {
            if (DWISClient != null && placeHolder != null)
            {
                bool ok = false;
                if (placeHolder != null && placeHolder.Count > 0 && placeHolder[0].Count > 0)
                {
                    string json = JsonSerializer.Serialize(this);
                    if (!string.IsNullOrEmpty(json))
                    {
                        {
                            NodeIdentifier id = placeHolder[0][0];
                            if (id != null && !string.IsNullOrEmpty(id.ID) && !string.IsNullOrEmpty(id.NameSpace))
                            {
                                // OPC-UA code to set the value at the node id = ID
                                (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                                outputs[0].nameSpace = id.NameSpace;
                                outputs[0].id = id.ID;
                                outputs[0].value = json;
                                outputs[0].sourceTimestamp = DateTime.UtcNow;
                                ok = DWISClient.UpdateAnyVariables(outputs);
                            }
                        }
                    }
                }
                return ok;
            }
            else
            {
                return false;
            }
        }

        public void UpdateMicroState(MicroStateIndex index, uint code)
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
                    Part1 = (int)((uint)Part1 & mask);
                    Part1 = (int)((uint)Part1 | code);
                    break;
                case 1:
                    Part2 = (int)((uint)Part2 & mask);
                    Part2 = (int)((uint)Part2 | code);
                    break;
                case 2:
                    Part3 = (int)((uint)Part3 & mask);
                    Part3 = (int)((uint)Part3 | code);
                    break;
                case 3:
                    Part4 = (int)((uint)Part4 & mask);
                    Part4 = (int)((uint)Part4 | code);
                    break;
                default:
                    Part5 = (int)((uint)Part5 & mask);
                    Part5 = (int)((uint)Part5 | code);
                    break;
            }
        }

        public byte GetValue(MicroStateIndex index)
        {
            uint val = 0;
            // which part
            int part = (int)index / 16;
            int pos = 2 * ((int)index % 16);
            switch (part)
            {
                case 0:
                    val = (uint)Part1;
                    break;
                case 1:
                    val = (uint)Part2;
                    break;
                case 2:
                    val = (uint)Part3;
                    break;
                case 3:
                    val = (uint)Part4;
                    break;
                default:
                    val = (uint)Part5;
                    break;
            }
            val = val >> pos;
            val &= 0x00000003;
            return (byte)val;
        }
    }
}
