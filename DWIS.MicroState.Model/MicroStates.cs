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
        AxialVelocityTopOfString = 0,
        [Label("Stable Axial Velocity Top of String")]
        StableAxialVelocityTopOfString, // 1
        [Label("Rotational Velocity Top of String")]
        RotationalVelocityTopOfString, // 2
        [Label("Stable Rotational Velocity Top of String")]
        StableRotationalVelocityTopOfString, // 3
        [Label("Flow at Top of String")]
        FlowAtTopOfString, // 4
        [Label("Stable Flow at Top of String")]
        StableFlowAtTopOfString, // 5
        [Label("Slip State")]
        SlipState, // 6
        [Label("Stable Tension Top of String")]
        StableTensionTopOfString, // 7
        [Label("Pressure Top of String")]
        PressureTopOfString, // 8
        [Label("Stable Pressure Top of String")]
        StablePressureTopOfString, // 9
        [Label("Torque Top of String")]
        TorqueTopOfString, // 10
        [Label("Stable Torque Top of String")]
        StableTorqueTopOfString, // 11
        [Label("Flow at Annulus Outlet")]
        FlowAtAnnulusOutlet, // 12
        [Label("Stable Flow at Annulus Outlet")]
        StableFlowAtAnnulusOutlet, // 13
        [Label("Cuttings Return at Annulus Outlet")]
        CuttingsReturnAtAnnulusOutlet, // 14
        [Label("On Bottom Bottom of String")]
        OnBottomBottomOfString, // 15
        [Label("Stable Bottom of String Rock Force")]
        StableBottomOfStringRockForce, // 16
        [Label("On Bottom Hole Opener")]
        OnBottomHoleOpener, // 17
        [Label("Rotational Velocity Bottom of String")]
        RotationalVelocityBottomOfString, // 18
        [Label("Stable Rotational Velocity Bottom of String")]
        StableRotationalVelocityBottomOfString, // 19
        [Label("Drilling")]
        Drilling, // 20
        [Label("Hole Opening")]
        HoleOpening, // 21
        [Label("Axial Velocity Bottom of String")]
        AxialVelocityBottomOfString, //22
        [Label("Stable Axial Velocity Bottom of String")]
        StableAxialVelocityBottomOfString, // 23
        [Label("Flow Bottom of String")]
        FlowBottomOfString, // 24
        [Label("Stable Flow Bottom of String")]
        StableFlowBottomOfString, // 25
        [Label("Flow Hole Opener")]
        FlowHoleOpener, // 26
        [Label("Stable Flow Hole Opener")]
        StableFlowHoleOpener, // 27
        [Label("Ledge Key Seat")]
        LedgeKeySeat, // 28
        [Label("Cuttings Bed")]
        CuttingsBed, // 29
        [Label("Differential Sticking")]
        DifferentialSticking, // 30
        [Label("Twist-off Back-off")]
        TwistOffBackOff, // 31
        [Label("Well Integrity")]
        WellIntegrity, // 32
        [Label("Formation Fluid at Annulus Outlet")]
        FormationFluidAtAnnulusOutlet, // 33
        [Label("Formation Collapse")]
        FormationCollapse, // 34
        [Label("Cavings at Annulus Outlet")]
        CavingsAtAnnulusOutlet, // 35
        [Label("Pipe Washout")]
        PipeWashout, // 36
        [Label("Whirl Bottom of String")]
        WhirlBottomOfString, // 37
        [Label("Whirl Hole Opener")]
        WhirlHoleOpener, // 38
        [Label("Float-sub")]
        FloatSub, // 39
        [Label("Under-reamer")]
        UnderReamer, // 40
        [Label("Circulation-sub")]
        CirculationSub, // 41
        [Label("Ported-float")]
        PortedFloat, // 42
        [Label("Whipstock")]
        Whipstock, // 43
        [Label("Plug")]
        Plug, // 44
        [Label("Liner")]
        Liner, // 45
        [Label("Booster Pumping")]
        BoosterPumping, // 46
        [Label("Stable Booster Pumping")]
        StableBoosterPumping, // 47
        [Label("Back Pressure Pumping")]
        BackPressurePumping, // 48
        [Label("Stable Back-pressure Pumping")]
        StableBackPressurePumping, // 49
        [Label("MPD Choke Opening")]
        MPDChokeOpening, // 50
        [Label("RCD Sealing")]
        RCDSealing, // 51
        [Label("Isolation Seal")]
        IsolationSeal, // 52
        [Label("Isolation Seal Pressure Balance")]
        IsolationSealPressureBalance, // 53
        [Label("Bearing Assembly Latched")]
        BearingAssemblyLatched, // 54
        [Label("Screen MPD Choke Plugged")]
        ScreenMPDChokePlugged, // 55
        [Label("Main Flow Path Stable")]
        MainFlowPathStable, // 56
        [Label("Alternate Flow Path Stable")]
        AlternateFlowPathStable, // 57
        [Label("Fill Pump DGD")]
        FillPumpDGD, // 58
        [Label("Lift Pump DGD")]
        LiftPumpDGD, // 59
        [Label("Stable Fill Pump DGD")]
        StableFillPumpDGD, // 60
        [Label("Stable Lift Pump DGD")]
        StableLiftPumpDGD, // 61
        [Label("Formation Change")]
        FormationChange, // 62
        [Label("Inside Hard Stringer")]
        InsideHardStringer, // 63
        [Label("Tool-joint #1 at Lowest Drill Height")]
        ToolJoint1AtLowestDrillHeight, // 64
        [Label("Tool-joint #1 at Stick-up Height")]
        ToolJoint1AtStickUpHeight, // 65
        [Label("Tool-joint #2 at Stick-up Height")]
        ToolJoint2AtStickUpHeight, // 66
        [Label("Tool-joint #3 at Stick-up Height")]
        ToolJoint3AtStickUpHeight, // 67
        [Label("Tool-joint #4 at Stick-up Height")]
        ToolJoint4AtStickUpHeight, // 68
        [Label("Heave Compensation")]
        HeaveCompensation, // 69
        [Label("Last Stand to Bottom Hole")]
        LastStandToBottomHole, // 70
        [Label("Whirl in Drill-string")]
        WhirlInDrillString, // 71
        [Label("HFTO")]
        HFTO, // 72
        [Label("Axial Oscillations")]
        AxialOscillations, // 73
        [Label("Torsional Oscillations")]
        TorsionalOscillations, // 74
        [Label("Lateral Shocks in BHA")]
        LateralShocksInBHA, // 75
        [Label("Lateral Shocks in Drill-string")]
        LateralShocksInDrillString, // 76
        [Label("String Rotation Impeded")]
        StringRotationImpeded, // 77
        [Label("Annulus Flow Impeded")]
        AnnulusFlowImpeded, // 78
        [Label("String Flow Impeded")]
        StringFlowImpeded // 79
    }
    [AccessToVariable(CommonProperty.VariableAccessType.Assignable)]
    [SemanticTypeVariable("ComputedData")]
    [SemanticFact("ComputedData", Nouns.Enum.DynamicDrillingSignal)]
    [SemanticFact("ComputedData#01", Nouns.Enum.ComputedData)]
    [SemanticFact("ComputedData#01", Verbs.Enum.HasDynamicValue, "ComputedData")]
    [SemanticFact("ProcessState", Nouns.Enum.ProcessState)]
    [SemanticFact("ProcessState", Nouns.Enum.DeterministicModel)]
    [SemanticFact("ComputedData#01", Verbs.Enum.IsGeneratedBy, "ProcessState")]
    [SemanticFact("DrillingProcessStateInterpreter", Nouns.Enum.DWISDrillingProcessStateInterpreter)]
    [SemanticFact("ProcessState", Verbs.Enum.IsProvidedBy, "DrillingProcessStateInterpreter")]
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

        public void CopyTo(ref MicroStates dest)
        {
            dest.TimeStampUTC = TimeStampUTC;
            dest.Part1 = Part1;
            dest.Part2 = Part2;
            dest.Part3 = Part3;
            dest.Part4 = Part4;
            dest.Part5 = Part5;
        }

        public bool RegisterToBlackboard(IOPCUADWISClient? DWISClient, QueryResult? placeHolder)
        {
            bool ok = false;
            if (DWISClient != null && placeHolder != null)
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
                        foreach (var kvp in queries)
                        {
                            if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                            {
                                var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                if (result != null && result.Results != null && result.Results.Count > 0)
                                {
                                    res = result;
                                    break;
                                }
                            }
                        }
                        // if we couldn't find any answer then the manifest must be injected
                        if (res == null)
                        {
                            var r = DWISClient.Inject(manifestFile);
                            if (r != null && r.Success)
                            {
                                res = null;
                                foreach (var kvp in queries)
                                {
                                    if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                                    {
                                        var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                        if (result != null && result.Results != null && result.Results.Count > 0)
                                        {
                                            res = result;
                                            break;
                                        }
                                    }
                                }
                                if (res != null)
                                {
                                    placeHolder.VariablesHeader.AddRange(res.VariablesHeader);
                                    placeHolder.Results.AddRange(res.Results);
                                    ok = true;
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
                                DWISClient.UpdateAnyVariables(outputs);
                                ok = true;
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
    }
}
