using DWIS.Vocabulary.Schemas;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace DWIS.MicroState.Model
{
    [AccessToVariable(CommonProperty.VariableAccessType.Assignable)]
    [SemanticTypeVariable("8796c92c-96a3-4854-a263-3a6aa67344bf")]
    [SemanticFact("8796c92c-96a3-4854-a263-3a6aa67344bf", Nouns.Enum.DynamicDrillingSignal)]
    [SemanticFact("DWIS:MicroStates:Current", Nouns.Enum.ComputedData)]
    [SemanticFact("DWIS:MicroStates:Current", Verbs.Enum.HasDynamicValue, "8796c92c-96a3-4854-a263-3a6aa67344bf")]
    [SemanticFact("DWIS:MicroStates:ProcessState", Nouns.Enum.ProcessState)]
    [SemanticFact("DWIS:MicroStates:ProcessState", Nouns.Enum.DeterministicModel)]
    [SemanticFact("DWIS:MicroStates:Current", Verbs.Enum.IsGeneratedBy, "DWIS:MicroStates:ProcessState")]
    [SemanticFact("DWIS:Provider:DrillingProcessStateInterpreter", Nouns.Enum.DWISDrillingProcessStateInterpreter)]
    [SemanticFact("DWIS:MicroStates:ProcessState", Verbs.Enum.IsProvidedBy, "DWIS:Provider:DrillingProcessStateInterpreter")]
    public struct MicroStates
    {
        /// <summary>
        /// an enumeration of the microstate indices
        /// </summary>
        public enum MicroStateIndex
        {
            AxialVelocityTopOfString = 0,
            StableAxialVelocityTopOfString, // 1
            RotationalVelocityTopOfString, // 2
            StableRotationalVelocityTopOfString, // 3
            FlowAtTopOfString, // 4
            StableFlowAtTopOfString, // 5
            SlipState, // 6
            StableTensionTopOfString, // 7
            PressureTopOfString, // 8
            StablePressureTopOfString, // 9
            TorqueTopOfString, // 10
            StableTorqueTopOfString, // 11
            FlowAtAnnulusOutlet, // 12
            StableFlowAtAnnulusOutlet, // 13
            CuttingsReturnAtAnnulusOutlet, // 14
            OnBottomBottomOfString, // 15
            StableBottomOfStringRockForce, // 16
            OnBottomHoleOpener, // 17
            RotationalVelocityBottomOfString, // 18
            StableRotationalVelocityBottomOfString, // 19
            Drilling, // 20
            HoleOpening, // 21
            AxialVelocityBottomOfString, //22
            StableAxialVelocityBottomOfString, // 23
            FlowBottomOfString, // 24
            StableFlowBottomOfString, // 25
            FlowHoleOpener, // 26
            StableFlowHoleOpener, // 27
            LedgeKeySeat, // 28
            CuttingsBed, // 29
            DifferentialSticking, // 30
            TwistOffBackOff, // 31
            WellIntegrity, // 32
            FormationFluidAtAnnulusOutlet, // 33
            FormationCollapse, // 34
            CavingsAtAnnulusOutlet, // 35
            PipeWashout, // 36
            WhirlBottomOfString, // 37
            WhirlHoleOpener, // 38
            FloatSub, // 39
            UnderReamer, // 40
            CirculationSub, // 41
            PortedFloat, // 42
            Whipstock, // 43
            Plug, // 44
            Liner, // 45
            BoosterPumping, // 46
            StableBoosterPumping, // 47
            BackPressurePumping, // 48
            StableBackPressurePumping, // 49
            MPDChokeOpening, // 50
            RCDSealing, // 51
            IsolationSeal, // 52
            IsolationSealPressureBalance, // 53
            BearingAssemblyLatched, // 54
            ScreenMPDChokePlugged, // 55
            MainFlowPathStable, // 56
            AlternateFlowPathStable, // 57
            FillPumpDGD, // 58
            LiftPumpDGD, // 59
            StableFillPumpDGD, // 60
            StableLiftPumpDGD, // 61
            FormationChange, // 62
            InsideHardStringer, // 63
            ToolJoint1AtLowestDrillHeight, // 64
            ToolJoint1AtStickUpHeight, // 65
            ToolJoint2AtStickUpHeight, // 66
            ToolJoint3AtStickUpHeight, // 67
            ToolJoint4AtStickUpHeight, // 68
            HeaveCompensation, // 69
            LastStandToBottomHole, // 70
            WhirlInDrillString, // 71
            HFTO, // 72
            AxialOscillations, // 73
            TorsionalOscillations, // 74
            LateralShocksInBHA, // 75
            LateralShokcsInDrillString // 76
        }
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
    }
}