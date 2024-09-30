//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.9.0.0 (Newtonsoft.Json v9.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace DWIS.MicroState.ModelShared
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class MicroStates
    {
        [Newtonsoft.Json.JsonProperty("TimeStampUTC", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TimeStampUTC { get; set; }

        [Newtonsoft.Json.JsonProperty("Part1", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part1 { get; set; }

        [Newtonsoft.Json.JsonProperty("Part2", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part2 { get; set; }

        [Newtonsoft.Json.JsonProperty("Part3", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part3 { get; set; }

        [Newtonsoft.Json.JsonProperty("Part4", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part4 { get; set; }

        [Newtonsoft.Json.JsonProperty("Part5", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Part5 { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ProbabilisticMicroStates
    {
        [Newtonsoft.Json.JsonProperty("TimeStampUTC", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TimeStampUTC { get; set; }

        [Newtonsoft.Json.JsonProperty("AxialVelocityTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty AxialVelocityTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableAxialVelocityTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableAxialVelocityTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("RotationalVelocityTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty RotationalVelocityTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableRotationalVelocityTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableRotationalVelocityTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("FlowAtTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FlowAtTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableFlowAtTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableFlowAtTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("SlipState", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty SlipState { get; set; }

        [Newtonsoft.Json.JsonProperty("StableTensionTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableTensionTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("PressureTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty PressureTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StablePressureTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StablePressureTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("TorqueTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty TorqueTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableTorqueTopOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableTorqueTopOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("FlowAtAnnulusOutlet", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FlowAtAnnulusOutlet { get; set; }

        [Newtonsoft.Json.JsonProperty("StableFlowAtAnnulusOutlet", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableFlowAtAnnulusOutlet { get; set; }

        [Newtonsoft.Json.JsonProperty("CuttingsReturnAtAnnulusOutlet", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty CuttingsReturnAtAnnulusOutlet { get; set; }

        [Newtonsoft.Json.JsonProperty("OnBottomBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty OnBottomBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableBottomOfStringRockForce", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableBottomOfStringRockForce { get; set; }

        [Newtonsoft.Json.JsonProperty("OnBottomHoleOpener", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty OnBottomHoleOpener { get; set; }

        [Newtonsoft.Json.JsonProperty("RotationalVelocityBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty RotationalVelocityBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableRotationalVelocityBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableRotationalVelocityBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("Drilling", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty Drilling { get; set; }

        [Newtonsoft.Json.JsonProperty("HoleOpening", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty HoleOpening { get; set; }

        [Newtonsoft.Json.JsonProperty("AxialVelocityBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty AxialVelocityBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableAxialVelocityBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableAxialVelocityBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("FlowBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty FlowBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("StableFlowBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableFlowBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("FlowHoleOpener", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty FlowHoleOpener { get; set; }

        [Newtonsoft.Json.JsonProperty("StableFlowHoleOpener", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableFlowHoleOpener { get; set; }

        [Newtonsoft.Json.JsonProperty("LedgeKeySeat", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty LedgeKeySeat { get; set; }

        [Newtonsoft.Json.JsonProperty("CuttingsBed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty CuttingsBed { get; set; }

        [Newtonsoft.Json.JsonProperty("DifferentialSticking", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty DifferentialSticking { get; set; }

        [Newtonsoft.Json.JsonProperty("TwistOffBackOff", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty TwistOffBackOff { get; set; }

        [Newtonsoft.Json.JsonProperty("WellIntegrity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty WellIntegrity { get; set; }

        [Newtonsoft.Json.JsonProperty("FormationFluidAtAnnulusOutlet", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FormationFluidAtAnnulusOutlet { get; set; }

        [Newtonsoft.Json.JsonProperty("FormationCollapse", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FormationCollapse { get; set; }

        [Newtonsoft.Json.JsonProperty("CavingsAtAnnulusOutlet", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty CavingsAtAnnulusOutlet { get; set; }

        [Newtonsoft.Json.JsonProperty("PipeWashout", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty PipeWashout { get; set; }

        [Newtonsoft.Json.JsonProperty("WhirlBottomOfString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty WhirlBottomOfString { get; set; }

        [Newtonsoft.Json.JsonProperty("WhirlHoleOpener", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty WhirlHoleOpener { get; set; }

        [Newtonsoft.Json.JsonProperty("FloatSub", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FloatSub { get; set; }

        [Newtonsoft.Json.JsonProperty("UnderReamer", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty UnderReamer { get; set; }

        [Newtonsoft.Json.JsonProperty("CirculationSub", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty CirculationSub { get; set; }

        [Newtonsoft.Json.JsonProperty("PortedFloat", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty PortedFloat { get; set; }

        [Newtonsoft.Json.JsonProperty("Whipstock", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty Whipstock { get; set; }

        [Newtonsoft.Json.JsonProperty("Plug", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty Plug { get; set; }

        [Newtonsoft.Json.JsonProperty("Liner", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty Liner { get; set; }

        [Newtonsoft.Json.JsonProperty("BoosterPumping", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty BoosterPumping { get; set; }

        [Newtonsoft.Json.JsonProperty("StableBoosterPumping", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableBoosterPumping { get; set; }

        [Newtonsoft.Json.JsonProperty("BackPressurePumping", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty BackPressurePumping { get; set; }

        [Newtonsoft.Json.JsonProperty("StableBackPressurePumping", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableBackPressurePumping { get; set; }

        [Newtonsoft.Json.JsonProperty("MPDChokeOpening", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty MPDChokeOpening { get; set; }

        [Newtonsoft.Json.JsonProperty("RCDSealing", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty RCDSealing { get; set; }

        [Newtonsoft.Json.JsonProperty("IsolationSeal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty IsolationSeal { get; set; }

        [Newtonsoft.Json.JsonProperty("IsolationSealPressureBalance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty IsolationSealPressureBalance { get; set; }

        [Newtonsoft.Json.JsonProperty("BearingAssemblyLatched", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty BearingAssemblyLatched { get; set; }

        [Newtonsoft.Json.JsonProperty("ScreenMPDChokePlugged", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ScreenMPDChokePlugged { get; set; }

        [Newtonsoft.Json.JsonProperty("MainFlowPathStable", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty MainFlowPathStable { get; set; }

        [Newtonsoft.Json.JsonProperty("AlternateFlowPathStable", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty AlternateFlowPathStable { get; set; }

        [Newtonsoft.Json.JsonProperty("FillPumpDGD", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty FillPumpDGD { get; set; }

        [Newtonsoft.Json.JsonProperty("LiftPumpDGD", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty LiftPumpDGD { get; set; }

        [Newtonsoft.Json.JsonProperty("StableFillPumpDGD", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableFillPumpDGD { get; set; }

        [Newtonsoft.Json.JsonProperty("StableLiftPumpDGD", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StableLiftPumpDGD { get; set; }

        [Newtonsoft.Json.JsonProperty("FormationChange", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty FormationChange { get; set; }

        [Newtonsoft.Json.JsonProperty("InsideHardStringer", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty InsideHardStringer { get; set; }

        [Newtonsoft.Json.JsonProperty("ToolJoint1AtLowestDrillHeight", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ToolJoint1AtLowestDrillHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("ToolJoint1AtStickUpHeight", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ToolJoint1AtStickUpHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("ToolJoint2AtStickUpHeight", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ToolJoint2AtStickUpHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("ToolJoint3AtStickUpHeight", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ToolJoint3AtStickUpHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("ToolJoint4AtStickUpHeight", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty ToolJoint4AtStickUpHeight { get; set; }

        [Newtonsoft.Json.JsonProperty("HeaveCompensation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDrillingProperty HeaveCompensation { get; set; }

        [Newtonsoft.Json.JsonProperty("LastStandToBottomHole", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty LastStandToBottomHole { get; set; }

        [Newtonsoft.Json.JsonProperty("WhirlInDrillString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty WhirlInDrillString { get; set; }

        [Newtonsoft.Json.JsonProperty("HFTO", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty HFTO { get; set; }

        [Newtonsoft.Json.JsonProperty("AxialOscillations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty AxialOscillations { get; set; }

        [Newtonsoft.Json.JsonProperty("TorsionalOscillations", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty TorsionalOscillations { get; set; }

        [Newtonsoft.Json.JsonProperty("LateralShocksInBHA", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty LateralShocksInBHA { get; set; }

        [Newtonsoft.Json.JsonProperty("LateralShocksInDrillString", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty LateralShocksInDrillString { get; set; }

        [Newtonsoft.Json.JsonProperty("StringRotationImpeded", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StringRotationImpeded { get; set; }

        [Newtonsoft.Json.JsonProperty("AnnulusFlowImpeded", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty AnnulusFlowImpeded { get; set; }

        [Newtonsoft.Json.JsonProperty("StringFlowImpeded", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDrillingProperty StringFlowImpeded { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class TernaryDrillingProperty : MultinomialDrillingProperty
    {
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DiscreteDistribution Value { get; set; }

        [Newtonsoft.Json.JsonProperty("TernaryValue", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TernaryDistribution TernaryValue { get; set; }

        [Newtonsoft.Json.JsonProperty("Probability1", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability1 { get; set; }

        [Newtonsoft.Json.JsonProperty("Probability2", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability2 { get; set; }

        [Newtonsoft.Json.JsonProperty("Probability", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<double> Probability { get; set; }

        [Newtonsoft.Json.JsonProperty("StateValue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? StateValue { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class DiscreteDistribution
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class TernaryDistribution : MultinomialDistribution
    {
        [Newtonsoft.Json.JsonProperty("Probability1", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability1 { get; set; }

        [Newtonsoft.Json.JsonProperty("Probability2", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability2 { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class MultinomialDistribution : DiscreteDistribution
    {
        [Newtonsoft.Json.JsonProperty("NumberTrials", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? NumberTrials { get; set; }

        [Newtonsoft.Json.JsonProperty("NumberOfStates", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? NumberOfStates { get; set; }

        [Newtonsoft.Json.JsonProperty("Probabilities", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<double> Probabilities { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class MultinomialDrillingProperty : DiscreteDrillingProperty
    {
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DiscreteDistribution Value { get; set; }

        [Newtonsoft.Json.JsonProperty("MultinomialValue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MultinomialDistribution MultinomialValue { get; set; }

        [Newtonsoft.Json.JsonProperty("NumberOfStates", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? NumberOfStates { get; set; }

        [Newtonsoft.Json.JsonProperty("Probabilities", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<double> Probabilities { get; set; }

        [Newtonsoft.Json.JsonProperty("StateValue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? StateValue { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public abstract partial class DiscreteDrillingProperty : DrillingProperty
    {
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DiscreteDistribution Value { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public abstract partial class DrillingProperty
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class BernoulliDrillingProperty : MultinomialDrillingProperty
    {
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DiscreteDistribution Value { get; set; }

        [Newtonsoft.Json.JsonProperty("BernoulliValue", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BernoulliDistribution BernoulliValue { get; set; }

        [Newtonsoft.Json.JsonProperty("Probability", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability { get; set; }

        [Newtonsoft.Json.JsonProperty("BooleanValue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool? BooleanValue { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class BernoulliDistribution : BinomialDistribution
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class BinomialDistribution : MultinomialDistribution
    {
        [Newtonsoft.Json.JsonProperty("Probability", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? Probability { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum MicroStateIndex
    {

        AxialVelocityTopOfString = 0,


        StableAxialVelocityTopOfString = 1,


        RotationalVelocityTopOfString = 2,


        StableRotationalVelocityTopOfString = 3,


        FlowAtTopOfString = 4,


        StableFlowAtTopOfString = 5,


        SlipState = 6,


        StableTensionTopOfString = 7,


        PressureTopOfString = 8,


        StablePressureTopOfString = 9,


        TorqueTopOfString = 10,


        StableTorqueTopOfString = 11,


        FlowAtAnnulusOutlet = 12,


        StableFlowAtAnnulusOutlet = 13,


        CuttingsReturnAtAnnulusOutlet = 14,


        OnBottomBottomOfString = 15,


        StableBottomOfStringRockForce = 16,


        OnBottomHoleOpener = 17,


        RotationalVelocityBottomOfString = 18,


        StableRotationalVelocityBottomOfString = 19,


        Drilling = 20,


        HoleOpening = 21,


        AxialVelocityBottomOfString = 22,


        StableAxialVelocityBottomOfString = 23,


        FlowBottomOfString = 24,


        StableFlowBottomOfString = 25,


        FlowHoleOpener = 26,


        StableFlowHoleOpener = 27,


        LedgeKeySeat = 28,


        CuttingsBed = 29,


        DifferentialSticking = 30,


        TwistOffBackOff = 31,


        WellIntegrity = 32,


        FormationFluidAtAnnulusOutlet = 33,


        FormationCollapse = 34,


        CavingsAtAnnulusOutlet = 35,


        PipeWashout = 36,


        WhirlBottomOfString = 37,


        WhirlHoleOpener = 38,


        FloatSub = 39,


        UnderReamer = 40,


        CirculationSub = 41,


        PortedFloat = 42,


        Whipstock = 43,


        Plug = 44,


        Liner = 45,


        BoosterPumping = 46,


        StableBoosterPumping = 47,


        BackPressurePumping = 48,


        StableBackPressurePumping = 49,


        MPDChokeOpening = 50,


        RCDSealing = 51,


        IsolationSeal = 52,


        IsolationSealPressureBalance = 53,


        BearingAssemblyLatched = 54,


        ScreenMPDChokePlugged = 55,


        MainFlowPathStable = 56,


        AlternateFlowPathStable = 57,


        FillPumpDGD = 58,


        LiftPumpDGD = 59,


        StableFillPumpDGD = 60,


        StableLiftPumpDGD = 61,


        FormationChange = 62,


        InsideHardStringer = 63,


        ToolJoint1AtLowestDrillHeight = 64,


        ToolJoint1AtStickUpHeight = 65,


        ToolJoint2AtStickUpHeight = 66,


        ToolJoint3AtStickUpHeight = 67,


        ToolJoint4AtStickUpHeight = 68,


        HeaveCompensation = 69,


        LastStandToBottomHole = 70,


        WhirlInDrillString = 71,


        HFTO = 72,


        AxialOscillations = 73,


        TorsionalOscillations = 74,


        LateralShocksInBHA = 75,


        LateralShocksInDrillString = 76,


        StringRotationImpeded = 77,


        AnnulusFlowImpeded = 78,


        StringFlowImpeded = 79,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class TupleOfMicroStatesAndProbabilisticMicroStatesAndMicroStateIndex
    {
        [Newtonsoft.Json.JsonProperty("Item1", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MicroStates Item1 { get; set; }

        [Newtonsoft.Json.JsonProperty("Item2", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ProbabilisticMicroStates Item2 { get; set; }

        [Newtonsoft.Json.JsonProperty("Item3", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MicroStateIndex Item3 { get; set; }


    }
}
