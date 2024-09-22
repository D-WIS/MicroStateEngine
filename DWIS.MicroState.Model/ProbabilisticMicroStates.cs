using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;
using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using System.ComponentModel.Design;
using DWIS.Vocabulary.Schemas;
using static System.Net.WebRequestMethods;
using System.Text;

namespace DWIS.MicroState.Model
{
    public class ProbabilisticMicroStates
    {
        private static string prefix_ = "DWIS:MicroState:ProbabilisticMicroStates:";
        private static string companyName_ = "DWIS";

        /// <summary>
        /// the time stamp in UTC when the state has been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }

        //AxialVelocityTopOfString = 0,
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("AxialVelocityTopOfString", 3)]
        [SemanticFact("AxialVelocityTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("AxialVelocityTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("AxialVelocityTopOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("AxialVelocityTopOfString#01", Verbs.Enum.HasDynamicValue, "AxialVelocityTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("AxialVelocityTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("AxialVelocityTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Axial Velocity Top Oo String")]
        [GroupName("Top Side Boundaries")]
        public CategoricalDrillingProperty AxialVelocityTopOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableAxialVelocityTopOfString, // 1
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableAxialVelocityTopOfString")]
        [SemanticFact("StableAxialVelocityTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableAxialVelocityTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableAxialVelocityTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableAxialVelocityTopOfString#01", Verbs.Enum.HasDynamicValue, "StableAxialVelocityTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableAxialVelocityTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableAxialVelocityTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Axial Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableAxialVelocityTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    RotationalVelocityTopOfString, // 2
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("RotationalVelocityTopOfString", 3)]
        [SemanticFact("RotationalVelocityTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("RotationalVelocityTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("RotationalVelocityTopOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("RotationalVelocityTopOfString#01", Verbs.Enum.HasDynamicValue, "RotationalVelocityTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("RotationalVelocityTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("RotationalVelocityTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Rotational Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        public CategoricalDrillingProperty RotationalVelocityTopOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableRotationalVelocityTopOfString, // 3
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableRotationalVelocityTopOfString")]
        [SemanticFact("StableRotationalVelocityTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableRotationalVelocityTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableRotationalVelocityTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableRotationalVelocityTopOfString#01", Verbs.Enum.HasDynamicValue, "StableRotationalVelocityTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableRotationalVelocityTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableRotationalVelocityTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Rotational Velocity Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableRotationalVelocityTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowAtTopOfString, // 4
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FlowAtTopOfString")]
        [SemanticFact("FlowAtTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FlowAtTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FlowAtTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FlowAtTopOfString#01", Verbs.Enum.HasDynamicValue, "FlowAtTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("FlowAtTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FlowAtTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Flow at Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty FlowAtTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableFlowAtTopOfString, // 5
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableFlowAtTopOfString")]
        [SemanticFact("StableFlowAtTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableFlowAtTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableFlowAtTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableFlowAtTopOfString#01", Verbs.Enum.HasDynamicValue, "StableFlowAtTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableFlowAtTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableFlowAtTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Flow at Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableFlowAtTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    SlipState, // 6
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("SlipState", 3)]
        [SemanticFact("SlipState", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("SlipState#01", Nouns.Enum.ComputedData)]
        [SemanticFact("SlipState#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("SlipState#01", Verbs.Enum.HasDynamicValue, "SlipState")]
        [SemanticFact("Slips", Nouns.Enum.RotarySlips)]
        [SemanticFact("SlipsLogical", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("SlipsLogical", Verbs.Enum.IsAMechanicalRepresentationFor, "Slips")]
        [SemanticFact("SlipState#01", Verbs.Enum.IsMechanicallyLocatedAt, "SlipsLogical")]
        [Label("Slip State")]
        [GroupName("Top Side Boundaries")]
        public CategoricalDrillingProperty SlipState { get; set; } = new CategoricalDrillingProperty(3);
        //    StableTensionTopOfString, // 7
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableTensionTopOfString")]
        [SemanticFact("StableTensionTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableTensionTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableTensionTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableTensionTopOfString#01", Verbs.Enum.HasDynamicValue, "StableTensionTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableTensionTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("StableTensionTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Tension Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableTensionTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    PressureTopOfString, // 8
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("PressureTopOfString")]
        [SemanticFact("PressureTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("PressureTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("PressureTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("PressureTopOfString#01", Verbs.Enum.HasDynamicValue, "PressureTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("PressureTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("AbsolutePressure", Nouns.Enum.AbsolutePressureReference)]
        [SemanticFact("p_tos#01", Verbs.Enum.HasPressureReferenceType, "AbsolutePressure")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("PressureTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Pressure Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty PressureTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StablePressureTopOfString, // 9
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StablePressureTopOfString")]
        [SemanticFact("StablePressureTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StablePressureTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StablePressureTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StablePressureTopOfString#01", Verbs.Enum.HasDynamicValue, "StablePressureTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StablePressureTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("AbsolutePressure", Nouns.Enum.AbsolutePressureReference)]
        [SemanticFact("StablePressureTopOfString#01", Verbs.Enum.HasPressureReferenceType, "AbsolutePressure")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StablePressureTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Pressure Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StablePressureTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    TorqueTopOfString, // 10
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("TorqueTopOfString")]
        [SemanticFact("TorqueTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("TorqueTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("TorqueTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("TorqueTopOfString#01", Verbs.Enum.HasDynamicValue, "TorqueTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("TorqueTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("TorqueTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Torque Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty TorqueTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableTorqueTopOfString, // 11
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableTorqueTopOfString")]
        [SemanticFact("StableTorqueTopOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableTorqueTopOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableTorqueTopOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableTorqueTopOfString#01", Verbs.Enum.HasDynamicValue, "StableTorqueTopOfString")]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableTorqueTopOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableTorqueTopOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Torque Top of String")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableTorqueTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowAtAnnulusOutlet, // 12
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FlowAtAnnulusOutlet")]
        [SemanticFact("FlowAtAnnulusOutlet", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FlowAtAnnulusOutlet#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FlowAtAnnulusOutlet#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.HasDynamicValue, "FlowAtAnnulusOutlet")]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FlowAtAnnulusOutlet#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Flow at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty FlowAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    StableFlowAtAnnulusOutlet, // 13
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableFlowAtAnnulusOutlet")]
        [SemanticFact("StableFlowAtAnnulusOutlet", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.HasDynamicValue, "StableFlowAtAnnulusOutlet")]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("StableFlowAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [Label("Stable Flow at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty StableFlowAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    CuttingsReturnAtAnnulusOutlet, // 14
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("CuttingsReturnAtAnnulusOutlet")]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Nouns.Enum.ComputedData)]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Verbs.Enum.HasDynamicValue, "CuttingsReturnAtAnnulusOutlet")]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("CuttingsReturnAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [Label("Cuttings Return at Annulus Outlet")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty CuttingsReturnAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    OnBottomBottomOfString, // 15
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("OnBottomBottomOfString")]
        [SemanticFact("OnBottomBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("OnBottomBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("OnBottomBottomOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("OnBottomBottomOfString#01", Verbs.Enum.HasDynamicValue, "OnBottomBottomOfString")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("OnBottomBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("OnBottomBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [SemanticFact("hbl#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("OnBottomBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "hbl#01")]
        [Label("Bottom of String On Bottom")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty OnBottomBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableBottomOfStringRockForce, // 16
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableBottomOfStringRockForce")]
        [SemanticFact("StableBottomOfStringRockForce", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableBottomOfStringRockForce#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableBottomOfStringRockForce#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableBottomOfStringRockForce#01", Verbs.Enum.HasDynamicValue, "StableBottomOfStringRockForce")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableBottomOfStringRockForce#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("StableBottomOfStringRockForce#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [SemanticFact("hbl#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("StableBottomOfStringRockForce#01", Verbs.Enum.IsPhysicallyLocatedAt, "hbl#01")]
        [Label("Stable Bottom of String Rock Force")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty StableBottomOfStringRockForce { get; set; } = new BernoulliDrillingProperty();
        //    OnBottomHoleOpener, // 17
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("OnBottomHoleOpener")]
        [SemanticFact("OnBottomHoleOpener", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("OnBottomHoleOpener#01", Nouns.Enum.ComputedData)]
        [SemanticFact("OnBottomHoleOpener#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("OnBottomHoleOpener#01", Verbs.Enum.HasDynamicValue, "OnBottomHoleOpener")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("OnBottomHoleOpener#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("OnBottomHoleOpener#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_urho#01")]
        [SemanticFact("trh#01", Nouns.Enum.TopOfRatHoleLocation)]
        [SemanticFact("StableBottomOfStringRockForce#01", Verbs.Enum.IsPhysicallyLocatedAt, "trh#01")]
        [Label("On Bottom Hole Opener")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty OnBottomHoleOpener { get; set; } = new BernoulliDrillingProperty();
        //    RotationalVelocityBottomOfString, // 18
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("RotationalVelocityBottomOfString", 3)]
        [SemanticFact("RotationalVelocityBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Verbs.Enum.HasDynamicValue, "RotationalVelocityBottomOfString")]
        [SemanticFact("TorsionalMotion", Nouns.Enum.TorsionalMotionType)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Verbs.Enum.HasMotionType, "TorsionalMotion")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("RotationalVelocityBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Rotational Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public CategoricalDrillingProperty RotationalVelocityBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableRotationalVelocityBottomOfString, // 19
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableRotationalVelocityBottomOfString")]
        [SemanticFact("StableRotationalVelocityBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Verbs.Enum.HasDynamicValue, "StableRotationalVelocityBottomOfString")]
        [SemanticFact("TorsionalMotion", Nouns.Enum.TorsionalMotionType)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Verbs.Enum.HasMotionType, "TorsionalMotion")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("StableRotationalVelocityBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Stable Rotational Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty StableRotationalVelocityBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    Drilling, // 20
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("Drilling")]
        [SemanticFact("Drilling", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("Drilling#01", Nouns.Enum.ComputedData)]
        [SemanticFact("Drilling#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("Drilling#01", Verbs.Enum.HasDynamicValue, "Drilling")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("Drilling#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("Drilling#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [SemanticFact("bhl#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("Drilling#01", Verbs.Enum.IsPhysicallyLocatedAt, "bhl#01")]
        [Label("Drilling")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty Drilling { get; set; } = new BernoulliDrillingProperty();
        //    HoleOpening, // 21
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("HoleOpening")]
        [SemanticFact("HoleOpening", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("HoleOpening#01", Nouns.Enum.ComputedData)]
        [SemanticFact("HoleOpening#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("HoleOpening#01", Verbs.Enum.HasDynamicValue, "HoleOpening")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("HoleOpening#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("HoleOpening#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [SemanticFact("trh#01", Nouns.Enum.TopOfRatHoleLocation)]
        [SemanticFact("HoleOpening#01", Verbs.Enum.IsPhysicallyLocatedAt, "trh#01")]
        [Label("Hole Opening")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty HoleOpening { get; set; } = new BernoulliDrillingProperty();
        //    AxialVelocityBottomOfString, //22
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("AxialVelocityBottomOfString", 3)]
        [SemanticFact("AxialVelocityBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("AxialVelocityBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("AxialVelocityBottomOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("AxialVelocityBottomOfString#01", Verbs.Enum.HasDynamicValue, "AxialVelocityBottomOfString")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("AxialVelocityBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("AxialVelocityBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Axial Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public CategoricalDrillingProperty AxialVelocityBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableAxialVelocityBottomOfString, // 23
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableAxialVelocityBottomOfString")]
        [SemanticFact("StableAxialVelocityBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableAxialVelocityBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableAxialVelocityBottomOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableAxialVelocityBottomOfString#01", Verbs.Enum.HasDynamicValue, "StableAxialVelocityBottomOfString")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableAxialVelocityBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("StableAxialVelocityBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Stable Axial Velocity Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty StableAxialVelocityBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowBottomOfString, // 24
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("FlowBottomOfString", 3)]
        [SemanticFact("FlowBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FlowBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FlowBottomOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("FlowBottomOfString#01", Verbs.Enum.HasDynamicValue, "FlowBottomOfString")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FlowBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("FlowBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Flow Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public CategoricalDrillingProperty FlowBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableFlowBottomOfString, // 25
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableFlowBottomOfString")]
        [SemanticFact("StableFlowBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableFlowBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableFlowBottomOfString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableFlowBottomOfString#01", Verbs.Enum.HasDynamicValue, "StableFlowBottomOfString")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableFlowBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("StableFlowBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Stable Flow Bottom of String")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty StableFlowBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowHoleOpener, // 26
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("FlowHoleOpener", 3)]
        [SemanticFact("FlowHoleOpener", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FlowHoleOpener#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FlowHoleOpener#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("FlowHoleOpener#01", Verbs.Enum.HasDynamicValue, "FlowHoleOpener")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FlowHoleOpener#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("FlowHoleOpener#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_urho#01")]
        [Label("Flow Hole Opener")]
        [GroupName("Downhole Boundaries")]
        public CategoricalDrillingProperty FlowHoleOpener { get; set; } = new CategoricalDrillingProperty(3);
        //    StableFlowHoleOpener, // 27
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableFlowHoleOpener")]
        [SemanticFact("StableFlowHoleOpener", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableFlowHoleOpener#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableFlowHoleOpener#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableFlowHoleOpener#01", Verbs.Enum.HasDynamicValue, "StableFlowHoleOpener")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableFlowHoleOpener#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("StableFlowHoleOpener#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_urho#01")]
        [Label("Stable Flow Hole Opener")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty StableFlowHoleOpener { get; set; } = new BernoulliDrillingProperty();
        //    LedgeKeySeat, // 28
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("LedgeKeySeat", 3)]
        [SemanticFact("LedgeKeySeat", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("LedgeKeySeat#01", Nouns.Enum.ComputedData)]
        [SemanticFact("LedgeKeySeat#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("LedgeKeySeat#01", Verbs.Enum.HasDynamicValue, "LedgeKeySeat")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("LedgeKeySeat#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("ledge#01", Nouns.Enum.LedgeLocation)]
        [SemanticFact("LedgeKeySeat#01", Verbs.Enum.IsPhysicallyLocatedAt, "ledge#01")]
        [Label("Ledge Key Seat")]
        [GroupName("Abnormal Downhole Boundaries")]
        public CategoricalDrillingProperty LedgeKeySeat { get; set; } = new CategoricalDrillingProperty(3);
        //    CuttingsBed, // 29
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("CuttingsBed", 3)]
        [SemanticFact("CuttingsBed", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("CuttingsBed#01", Nouns.Enum.ComputedData)]
        [SemanticFact("CuttingsBed#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("CuttingsBed#01", Verbs.Enum.HasDynamicValue, "CuttingsBed")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("CuttingsBed#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bed#01", Nouns.Enum.CuttingsBedLocation)]
        [SemanticFact("CuttingsBed#01", Verbs.Enum.IsPhysicallyLocatedAt, "bed#01")]
        [Label("Cuttings Bed")]
        [GroupName("Abnormal Downhole Boundaries")]
        public CategoricalDrillingProperty CuttingsBed { get; set; } = new CategoricalDrillingProperty(3);
        //    DifferentialSticking, // 30
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("DifferentialSticking")]
        [SemanticFact("DifferentialSticking", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("DifferentialSticking#01", Nouns.Enum.ComputedData)]
        [SemanticFact("DifferentialSticking#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("DifferentialSticking#01", Verbs.Enum.HasDynamicValue, "DifferentialSticking")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("DifferentialSticking#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("DifferentialSticking#01", Nouns.Enum.DifferentialStickingLocation)]
        [SemanticFact("DifferentialSticking#01", Verbs.Enum.IsPhysicallyLocatedAt, "DifferentialSticking#01")]
        [Label("Differential Sticking")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty DifferentialSticking { get; set; } = new BernoulliDrillingProperty();
        //    TwistOffBackOff, // 31
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("TwistOffBackOff")]
        [SemanticFact("TwistOffBackOff", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("TwistOffBackOff#01", Nouns.Enum.ComputedData)]
        [SemanticFact("TwistOffBackOff#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("TwistOffBackOff#01", Verbs.Enum.HasDynamicValue, "TwistOffBackOff")]
        [SemanticFact("TwistOff", Nouns.Enum.TwistOff)]
        [SemanticFact("TwistOffBackOff#01", Verbs.Enum.IsRelatedToDrillingIncident, "TwistOff")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("TwistOffBackOff#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Twist-off Back-off")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty TwistOffBackOff { get; set; } = new BernoulliDrillingProperty();
        //    WellIntegrity, // 32
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("WellIntegrity", 3)]
        [SemanticFact("WellIntegrity", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("WellIntegrity#01", Nouns.Enum.ComputedData)]
        [SemanticFact("WellIntegrity#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("WellIntegrity#01", Verbs.Enum.HasDynamicValue, "WellIntegrity")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("WellIntegrity#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("FormationFluidFlow#01", Nouns.Enum.FormationFluidTransferLocation)]
        [SemanticFact("WellIntegrity#01", Verbs.Enum.IsPhysicallyLocatedAt, "FormationFluidFlow#01")]
        [Label("Well Integrity")]
        [GroupName("Abnormal Downhole Boundaries")]
        public CategoricalDrillingProperty WellIntegrity { get; set; } = new CategoricalDrillingProperty(3);
        //    FormationFluidAtAnnulusOutlet, // 33
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FormationFluidAtAnnulusOutlet")]
        [SemanticFact("FormationFluidAtAnnulusOutlet", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FormationFluidAtAnnulusOutlet#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FormationFluidAtAnnulusOutlet#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FormationFluidAtAnnulusOutlet#01", Verbs.Enum.HasDynamicValue, "FormationFluidAtAnnulusOutlet")]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("Q_FormationFluid_out#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("FormationLiquidComponent#01", Nouns.Enum.FormationLiquidComponent)]
        [SemanticFact("FormationGasComponent#01", Nouns.Enum.FormationGasComponent)]
        [SemanticFact("Q_FormationFluid_out#01", Verbs.Enum.ConcernsAFluidComponent, "FormationLiquidComponent#01")]
        [SemanticFact("Q_FormationFluid_out#01", Verbs.Enum.ConcernsAFluidComponent, "FormationGasComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("Q_FormationFluid_out#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Formation Fluid at Annulus Outlet")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty FormationFluidAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    FormationCollapse, // 34
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FormationCollapse")]
        [SemanticFact("FormationCollapse", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FormationCollapse#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FormationCollapse#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FormationCollapse#01", Verbs.Enum.HasDynamicValue, "FormationCollapse")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FormationCollapse#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("FormationCollapseLocation", Nouns.Enum.FormationCollapseLocation)]
        [SemanticFact("FormationCollapse#01", Verbs.Enum.IsPhysicallyLocatedAt, "FormationCollapseLocation")]
        [Label("Formation Collapse")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty FormationCollapse { get; set; } = new BernoulliDrillingProperty();
        //    CavingsAtAnnulusOutlet, // 35
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("CavingsAtAnnulusOutlet")]
        [SemanticFact("CavingsAtAnnulusOutlet", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Nouns.Enum.ComputedData)]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Verbs.Enum.HasDynamicValue, "CavingsAtAnnulusOutlet")]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("CavingsComponent#01", Nouns.Enum.CavingsComponent)]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Verbs.Enum.ConcernsAFluidComponent, "CavingsComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("CavingsAtAnnulusOutlet#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Cavings at Annulus Outlet")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty CavingsAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    PipeWashout, // 36
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("PipeWashout")]
        [SemanticFact("PipeWashout", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("PipeWashout#01", Nouns.Enum.ComputedData)]
        [SemanticFact("PipeWashout#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("PipeWashout#01", Verbs.Enum.HasDynamicValue, "PipeWashout")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("PipeWashout#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("PipeWashoutLocation", Nouns.Enum.PipeWashoutLocation)]
        [SemanticFact("PipeWashout#01", Verbs.Enum.IsPhysicallyLocatedAt, "PipeWashoutLocation")]
        [Label("Pipe Washout")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty PipeWashout { get; set; } = new BernoulliDrillingProperty();
        //    WhirlBottomOfString, // 37
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("WhirlBottomOfString", 3)]
        [SemanticFact("WhirlBottomOfString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("WhirlBottomOfString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("WhirlBottomOfString#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("WhirlBottomOfString#01", Verbs.Enum.HasDynamicValue, "WhirlBottomOfString")]
        [SemanticFact("LateralMotion", Nouns.Enum.LateralMotionType)]
        [SemanticFact("WhirlBottomOfString#01", Verbs.Enum.HasMotionType, "LateralMotion")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("WhirlBottomOfString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("WhirlBottomOfString#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [Label("Whirl Bottom of String")]
        [GroupName("Abnormal Downhole Boundaries")]
        public CategoricalDrillingProperty WhirlBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    WhirlHoleOpener, // 38
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("WhirlHoleOpener", 3)]
        [SemanticFact("WhirlHoleOpener", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("WhirlHoleOpener#01", Nouns.Enum.ComputedData)]
        [SemanticFact("WhirlHoleOpener#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("WhirlHoleOpener#01", Verbs.Enum.HasDynamicValue, "WhirlHoleOpener")]
        [SemanticFact("LateralMotion", Nouns.Enum.LateralMotionType)]
        [SemanticFact("LateralMotion#01", Verbs.Enum.HasMotionType, "LateralMotion")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("LateralMotion#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("LateralMotion#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_urho#01")]
        [Label("Whirl Hole Opener")]
        [GroupName("Abnormal Downhole Boundaries")]
        public CategoricalDrillingProperty WhirlHoleOpener { get; set; } = new CategoricalDrillingProperty(3);
        //    FloatSub, // 39
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FloatSub")]
        [SemanticFact("FloatSub", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FloatSub#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FloatSub#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FloatSub#01", Verbs.Enum.HasDynamicValue, "FloatSub")]
        [SemanticFact("FloatValve#01", Nouns.Enum.FloatValveNonreturnValve)]
        [SemanticFact("Logical_FloatValve#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_FloatValve#01", Verbs.Enum.IsAHydraulicRepresentationFor, "FloatValve#01")]
        [SemanticFact("FloatSub#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_FloatValve#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FloatSub#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("FloatSub#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [Label("Float-sub")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty FloatSub { get; set; } = new BernoulliDrillingProperty();
        //    UnderReamer, // 40
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("UnderReamer")]
        [SemanticFact("UnderReamer", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("UnderReamer#01", Nouns.Enum.ComputedData)]
        [SemanticFact("UnderReamer#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("UnderReamer#01", Verbs.Enum.HasDynamicValue, "UnderReamer")]
        [SemanticFact("UR", Nouns.Enum.Underreamers)]
        [SemanticFact("logical_underReamer", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_underReamer", Verbs.Enum.IsAMechanicalRepresentationFor, "UR")]
        [SemanticFact("UnderReamer#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_underReamer")]
        [Label("Under-reamer")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty UnderReamer { get; set; } = new BernoulliDrillingProperty();
        //    CirculationSub, // 41
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("CirculationSub")]
        [SemanticFact("CirculationSub", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("CirculationSub#01", Nouns.Enum.ComputedData)]
        [SemanticFact("CirculationSub#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("CirculationSub#01", Verbs.Enum.HasDynamicValue, "CirculationSub")]
        [SemanticFact("CS", Nouns.Enum.CirculationSub)]
        [SemanticFact("logical_circulationSub", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_circulationSub", Verbs.Enum.IsAMechanicalRepresentationFor, "CS")]
        [SemanticFact("CirculationSub#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_circulationSub")]
        [Label("Circulation-sub")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty CirculationSub { get; set; } = new BernoulliDrillingProperty();
        //    PortedFloat, // 42
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("PortedFloat")]
        [SemanticFact("PortedFloat", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("PortedFloat#01", Nouns.Enum.ComputedData)]
        [SemanticFact("PortedFloat#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("PortedFloat#01", Verbs.Enum.HasDynamicValue, "PortedFloat")]
        [SemanticFact("PF", Nouns.Enum.PortedPlungerValvesFa)]
        [SemanticFact("logical_portedFloat", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_portedFloat", Verbs.Enum.IsAMechanicalRepresentationFor, "PF")]
        [SemanticFact("PortedFloat#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_portedFloat")]
        [Label("Ported-float")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty PortedFloat { get; set; } = new BernoulliDrillingProperty();
        //    Whipstock, // 43
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("Whipstock")]
        [SemanticFact("Whipstock", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("Whipstock#01", Nouns.Enum.ComputedData)]
        [SemanticFact("Whipstock#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("Whipstock#01", Verbs.Enum.HasDynamicValue, "Whipstock")]
        [SemanticFact("whipstockElement", Nouns.Enum.Whipstock)]
        [SemanticFact("logical_whipstock", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_whipstock", Verbs.Enum.IsAMechanicalRepresentationFor, "whipstockElement")]
        [SemanticFact("Whipstock#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_whipstock")]
        [Label("Whipstock")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty Whipstock { get; set; } = new BernoulliDrillingProperty();
        //    Plug, // 44
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("Plug")]
        [SemanticFact("Plug", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("Plug#01", Nouns.Enum.ComputedData)]
        [SemanticFact("Plug#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("Plug#01", Verbs.Enum.HasDynamicValue, "Plug")]
        [SemanticFact("plugElement", Nouns.Enum.Plugs)]
        [SemanticFact("logical_plug", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_plug", Verbs.Enum.IsAMechanicalRepresentationFor, "plugElement")]
        [SemanticFact("Plug#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_plug")]
        [Label("Plug")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty Plug { get; set; } = new BernoulliDrillingProperty();
        //    Liner, // 45
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("Liner")]
        [SemanticFact("Liner", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("Liner#01", Nouns.Enum.ComputedData)]
        [SemanticFact("Liner#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("Liner#01", Verbs.Enum.HasDynamicValue, "Liner")]
        [SemanticFact("linerElement", Nouns.Enum.Liner)]
        [SemanticFact("logical_liner", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_liner", Verbs.Enum.IsAMechanicalRepresentationFor, "linerElement")]
        [SemanticFact("AttachedState", Nouns.Enum.MechanicallyConnectedState, "Value", "true")]
        [SemanticFact("logical_liner", Verbs.Enum.HasMechanicalState, "AttachedState")]
        [SemanticFact("Liner#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_liner")]
        [Label("Liner")]
        [GroupName("Downhole Element Change Boundaries")]
        public BernoulliDrillingProperty Liner { get; set; } = new BernoulliDrillingProperty();
        //    BoosterPumping, // 46
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("BoosterPumping")]
        [SemanticFact("BoosterPumping", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("BoosterPumping#01", Nouns.Enum.ComputedData)]
        [SemanticFact("BoosterPumping#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.HasDynamicValue, "BoosterPumping")]
        [SemanticFact("BoosterPump#01", Nouns.Enum.BoosterPump)]
        [SemanticFact("Logical_BoosterPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BoosterPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BoosterPump#01")]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BoosterPump#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("BoosterPumping#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Booster Pumping")]
        [GroupName("Floating Rig Boundaries")]
        public BernoulliDrillingProperty BoosterPumping { get; set; } = new BernoulliDrillingProperty();
        //    StableBoosterPumping, // 47
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableBoosterPumping")]
        [SemanticFact("StableBoosterPumping", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableBoosterPumping#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableBoosterPumping#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.HasDynamicValue, "StableBoosterPumping")]
        [SemanticFact("BoosterPump#01", Nouns.Enum.BoosterPump)]
        [SemanticFact("Logical_BoosterPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BoosterPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BoosterPump#01")]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BoosterPump#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("StableBoosterPumping#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [Label("Stable Booster Pumping")]
        [GroupName("Floating Rig Boundaries")]
        public BernoulliDrillingProperty StableBoosterPumping { get; set; } = new BernoulliDrillingProperty();
        //    BackPressurePumping, // 48
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("BackPressurePumping")]
        [SemanticFact("BackPressurePumping#01", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("BackPressurePumping#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.HasDynamicValue, "BackPressurePumping")]
        [SemanticFact("BackPressurePump#01", Nouns.Enum.BackPressurePump)]
        [SemanticFact("Logical_BackPressurePump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BackPressurePump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BackPressurePump#01")]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BackPressurePump#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("BackPressurePumping#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Back Pressure Pumping")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty BackPressurePumping { get; set; } = new BernoulliDrillingProperty();
        //    StableBackPressurePumping, // 49
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableBackPressurePumping")]
        [SemanticFact("StableBackPressurePumping", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableBackPressurePumping#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableBackPressurePumping#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.HasDynamicValue, "StableBackPressurePumping")]
        [SemanticFact("BackPressurePump#01", Nouns.Enum.BackPressurePump)]
        [SemanticFact("Logical_BackPressurePump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BackPressurePump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BackPressurePump#01")]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BackPressurePump#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("StableBackPressurePumping#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [Label("Stable Back Pressure Pumping")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty StableBackPressurePumping { get; set; } = new BernoulliDrillingProperty();
        //    MPDChokeOpening, // 50
        [SemanticCategoricalVariable("", 3)]
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("MPDChokeOpening")]
        [SemanticFact("MPDChokeOpening", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("MPDChokeOpening#01", Nouns.Enum.ComputedData)]
        [SemanticFact("MPDChokeOpening#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("MPDChokeOpening#01", Verbs.Enum.HasDynamicValue, "MPDChokeOpening")]
        [SemanticFact("MPDChoke#01", Nouns.Enum.MpdChoke)]
        [SemanticFact("Logical_MPDChoke#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_MPDChoke#01", Verbs.Enum.IsAHydraulicRepresentationFor, "MPDChoke#01")]
        [SemanticFact("MPDChokeOpening#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_MPDChoke#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("MPDChokeOpening#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("MPD Choke Opening")]
        [GroupName("Back-pressure MPD Boundaries")]
        public CategoricalDrillingProperty MPDChokeOpening { get; set; } = new CategoricalDrillingProperty(3);
        //    RCDSealing, // 51
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("RCDSealing")]
        [SemanticFact("RCDSealing", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("RCDSealing#01", Nouns.Enum.ComputedData)]
        [SemanticFact("RCDSealing#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("RCDSealing#01", Verbs.Enum.HasDynamicValue, "RCDSealing")]
        [SemanticFact("RCD#01", Nouns.Enum.RotatingControlDeviceRcd)]
        [SemanticFact("Logical_RCD#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_RCD#01", Verbs.Enum.IsAHydraulicRepresentationFor, "RCD#01")]
        [SemanticFact("RCDSealing#01", Verbs.Enum.IsHydraulicallyLocatedAt, "RCD#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("RCDSealing#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("RCDSealing#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [Label("RCD Sealing")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty RCDSealing { get; set; } = new BernoulliDrillingProperty();
        //    IsolationSeal, // 52
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("IsolationSeal")]
        [SemanticFact("IsolationSeal", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("IsolationSeal#01", Nouns.Enum.ComputedData)]
        [SemanticFact("IsolationSeal#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("IsolationSeal#01", Verbs.Enum.HasDynamicValue, "IsolationSeal")]
        [SemanticFact("isolationSealElement", Nouns.Enum.IsolationSeal)]
        [SemanticFact("logical_isolationSeal", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_isolationSeal", Verbs.Enum.IsAMechanicalRepresentationFor, "isolationSealElement")]
        [SemanticFact("IsolationSeal#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_isolationSeal")]
        [Label("Isolation Seal")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty IsolationSeal { get; set; } = new BernoulliDrillingProperty();
        //    IsolationSealPressureBalance, // 53
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("IsolationSealPressureBalance")]
        [SemanticFact("IsolationSealPressureBalance", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("IsolationSealPressureBalance#01", Nouns.Enum.ComputedData)]
        [SemanticFact("IsolationSealPressureBalance#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("IsolationSealPressureBalance#01", Verbs.Enum.HasDynamicValue, "IsolationSealPressureBalance")]
        [SemanticFact("IsolationSeal#01", Nouns.Enum.IsolationSeal)]
        [SemanticFact("Logical_IsolationSeal#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_IsolationSeal#01", Verbs.Enum.IsAHydraulicRepresentationFor, "IsolationSeal#01")]
        [SemanticFact("IsolationSealPressureBalance#01", Verbs.Enum.IsHydraulicallyLocatedAt, "IsolationSeal#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("IsolationSealPressureBalance#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("IsolationSealPressureBalance#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [Label("Isolation Seal Pressure Balance")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty IsolationSealPressureBalance { get; set; } = new BernoulliDrillingProperty();
        //    BearingAssemblyLatched, // 54
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("BearingAssemblyLatched")]
        [SemanticFact("BearingAssemblyLatched", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("BearingAssemblyLatched#01", Nouns.Enum.ComputedData)]
        [SemanticFact("BearingAssemblyLatched#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("BearingAssemblyLatched#01", Verbs.Enum.HasDynamicValue, "BearingAssemblyLatched")]
        [SemanticFact("bearingAssembly", Nouns.Enum.BottomholeAssembly)] // should be MPD bearing assembly
        [SemanticFact("logical_bearingAssembly", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_bearingAssembly", Verbs.Enum.IsAMechanicalRepresentationFor, "bearingAssembly")]
        [SemanticFact("AttachedState", Nouns.Enum.MechanicallyConnectedState, "Value", "true")]
        [SemanticFact("logical_bearingAssembly", Verbs.Enum.HasMechanicalState, "AttachedState")]
        [SemanticFact("BearingAssemblyLatched#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_bearingAssembly")]
        [Label("Bearing Assembly Latched")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty BearingAssemblyLatched { get; set; } = new BernoulliDrillingProperty();
        //    ScreenMPDChokePlugged, // 55
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ScreenMPDChokePlugged")]
        [SemanticFact("ScreenMPDChokePlugged", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ScreenMPDChokePlugged#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ScreenMPDChokePlugged#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ScreenMPDChokePlugged#01", Verbs.Enum.HasDynamicValue, "ScreenMPDChokePlugged")]
        [SemanticFact("MPDScreen", Nouns.Enum.ShakerScreens)] // should be MPD Screen
        [SemanticFact("logical_MPDScreen", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_MPDScreen", Verbs.Enum.IsAMechanicalRepresentationFor, "MPDScreen")]
        [SemanticFact("Plugged", Nouns.Enum.Plugged)]
        [SemanticFact("logical_MPDScreen", Verbs.Enum.IsRelatedToDrillingIncident, "Plugged")]
        [SemanticFact("ScreenMPDChokePlugged#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_MPDScreen")]
        [Label("Screen MPD Choke Plugged")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty ScreenMPDChokePlugged { get; set; } = new BernoulliDrillingProperty();
        //    MainFlowPathStable, // 56
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("MainFlowPathStable")]
        [SemanticFact("MainFlowPathStable", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("MainFlowPathStable#01", Nouns.Enum.ComputedData)]
        [SemanticFact("MainFlowPathStable#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("MainFlowPathStable#01", Verbs.Enum.HasDynamicValue, "MainFlowPathStable")]
        [SemanticFact("MPDMainFlowPath", Nouns.Enum.MPDMainFlowPath)]
        [SemanticFact("logical_MPDMainFlowPath", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_MPDMainFlowPath", Verbs.Enum.IsAMechanicalRepresentationFor, "MPDMainFlowPath")]
        [SemanticFact("MainFlowPathStable#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_MPDMainFlowPath")]
        [Label("Main Flow Path Stable")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty MainFlowPathStable { get; set; } = new BernoulliDrillingProperty();
        //    AlternateFlowPathStable, // 57
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("AlternateFlowPathStable")]
        [SemanticFact("AlternateFlowPathStable", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("AlternateFlowPathStable#01", Nouns.Enum.ComputedData)]
        [SemanticFact("AlternateFlowPathStable#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("AlternateFlowPathStable#01", Verbs.Enum.HasDynamicValue, "AlternateFlowPathStable")]
        [SemanticFact("MPDAlternateFlowPath", Nouns.Enum.MPDAlternateFlowPath)]
        [SemanticFact("logical_MPDAlternateFlowPath", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_MPDAlternateFlowPath", Verbs.Enum.IsAMechanicalRepresentationFor, "MPDAlternateFlowPath")]
        [SemanticFact("AlternateFlowPathStable#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_MPDAlternateFlowPath")]
        [Label("Alternate Flow Path Stable")]
        [GroupName("Back-pressure MPD Boundaries")]
        public BernoulliDrillingProperty AlternateFlowPathStable { get; set; } = new BernoulliDrillingProperty();
        //    FillPumpDGD, // 58
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("FillPumpDGD")]
        [SemanticFact("FillPumpDGD", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FillPumpDGD#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FillPumpDGD#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("FillPumpDGD#01", Verbs.Enum.HasDynamicValue, "FillPumpDGD")]
        [SemanticFact("fillPump#01", Nouns.Enum.FillPump)]
        [SemanticFact("logical_fillPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("FillPumpDGD#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_fillPump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("FillPumpDGD#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Fill Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        public BernoulliDrillingProperty FillPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    LiftPumpDGD, // 59
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("LiftPumpDGD")]
        [SemanticFact("LiftPumpDGD", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("LiftPumpDGD#01", Nouns.Enum.ComputedData)]
        [SemanticFact("LiftPumpDGD#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("LiftPumpDGD#01", Verbs.Enum.HasDynamicValue, "LiftPumpDGD")]
        [SemanticFact("liftPump#01", Nouns.Enum.RiserLiftPump)]
        [SemanticFact("logical_liftPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("LiftPumpDGD#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_liftPump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("LiftPumpDGD#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [Label("Lift Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        public BernoulliDrillingProperty LiftPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    StableFillPumpDGD, // 60
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableFillPumpDGD")]
        [SemanticFact("StableFillPumpDGD", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableFillPumpDGD#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableFillPumpDGD#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableFillPumpDGD#01", Verbs.Enum.HasDynamicValue, "StableFillPumpDGD")]
        [SemanticFact("fillPump#01", Nouns.Enum.FillPump)]
        [SemanticFact("logical_fillPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("StableFillPumpDGD#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_fillPump#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableFillPumpDGD#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Fill Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        public BernoulliDrillingProperty StableFillPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    StableLiftPumpDGD, // 61
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StableLiftPumpDGD")]
        [SemanticFact("StableLiftPumpDGD", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StableLiftPumpDGD#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StableLiftPumpDGD#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StableLiftPumpDGD#01", Verbs.Enum.HasDynamicValue, "StableLiftPumpDGD")]
        [SemanticFact("liftPump#01", Nouns.Enum.RiserLiftPump)]
        [SemanticFact("logical_liftPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("StableLiftPumpDGD#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_liftPump#01")]
        [SemanticFact("MovingStandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("StableLiftPumpDGD#01", Verbs.Enum.IsTransformationOutput, "MovingStandardDeviation")]
        [Label("Stable Lift Pump DGD")]
        [GroupName("Dual-gradient Drilling Boundaries")]
        public BernoulliDrillingProperty StableLiftPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    FormationChange, // 62
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("FormationChange", 3)]
        [SemanticFact("FormationChange", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("FormationChange#01", Nouns.Enum.ComputedData)]
        [SemanticFact("FormationChange#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("FormationChange#01", Verbs.Enum.HasDynamicValue, "FormationChange")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("UCS_bh#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("FormationChange#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("FormationChange#01", Verbs.Enum.IsPhysicallyLocatedAt, "bh#01")]
        [Label("Formation Change")]
        [GroupName("Downhole Boundaries")]
        public CategoricalDrillingProperty FormationChange { get; set; } = new CategoricalDrillingProperty(3);
        //    InsideHardStringer, // 63
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("InsideHardStringer")]
        [SemanticFact("InsideHardStringer", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("InsideHardStringer#01", Nouns.Enum.ComputedData)]
        [SemanticFact("InsideHardStringer#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("InsideHardStringer#01", Verbs.Enum.HasDynamicValue, "InsideHardStringer")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("InsideHardStringer#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("InsideHardStringer#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("InsideHardStringer#01", Verbs.Enum.IsPhysicallyLocatedAt, "bh#01")]
        [Label("Inside Hard Stringer")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty InsideHardStringer { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint1AtLowestDrillHeight, // 64
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ToolJoint1AtLowestDrillHeight")]
        [SemanticFact("ToolJoint1AtLowestDrillHeight", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Verbs.Enum.HasDynamicValue, "ToolJoint1AtLowestDrillHeight")]
        [SemanticFact("TJ1#01", Nouns.Enum.ToolJoint1ReferenceLocation)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Verbs.Enum.IsPhysicallyLocatedAt, "TJ1#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("MinDrillHeight", Nouns.Enum.MinDrillHeightVerticalLocation)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight", Verbs.Enum.IsPhysicallyLocatedAt, "MinDrillHeight")]
        [Label("Tool-joint #1 at Lowest Drill Height")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty ToolJoint1AtLowestDrillHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint1AtStickUpHeight, // 65
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ToolJoint1AtStickUpHeight")]
        [SemanticFact("ToolJoint1AtStickUpHeight", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ToolJoint1AtStickUpHeight#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ToolJoint1AtStickUpHeight#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ToolJoint1AtStickUpHeight#01", Verbs.Enum.HasDynamicValue, "ToolJoint1AtStickUpHeight")]
        [SemanticFact("TJ1#01", Nouns.Enum.ToolJoint1ReferenceLocation)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Verbs.Enum.IsPhysicallyLocatedAt, "TJ1#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("StickUpHeight", Nouns.Enum.StickUpHeightVerticalLocation)]
        [SemanticFact("ToolJoint1AtLowestDrillHeight", Verbs.Enum.IsPhysicallyLocatedAt, "StickUpHeight")]
        [Label("Tool-joint #1 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty ToolJoint1AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint2AtStickUpHeight, // 66
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ToolJoint2AtStickUpHeight")]
        [SemanticFact("ToolJoint2AtStickUpHeight", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ToolJoint2AtStickUpHeight#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ToolJoint2AtStickUpHeight#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ToolJoint2AtStickUpHeight#01", Verbs.Enum.HasDynamicValue, "ToolJoint2AtStickUpHeight")]
        [SemanticFact("TJ2#01", Nouns.Enum.ToolJoint2ReferenceLocation)]
        [SemanticFact("ToolJoint2AtLowestDrillHeight#01", Verbs.Enum.IsPhysicallyLocatedAt, "TJ2#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("ToolJoint2AtLowestDrillHeight#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("StickUpHeight", Nouns.Enum.StickUpHeightVerticalLocation)]
        [SemanticFact("ToolJoint2AtLowestDrillHeight", Verbs.Enum.IsPhysicallyLocatedAt, "StickUpHeight")]
        [Label("Tool-joint #2 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty ToolJoint2AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();

        //    ToolJoint3AtStickUpHeight, // 67
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ToolJoint3AtStickUpHeight")]
        [SemanticFact("ToolJoint3AtStickUpHeight", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ToolJoint3AtStickUpHeight#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ToolJoint3AtStickUpHeight#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ToolJoint3AtStickUpHeight#01", Verbs.Enum.HasDynamicValue, "ToolJoint3AtStickUpHeight")]
        [SemanticFact("TJ3#01", Nouns.Enum.ToolJoint2ReferenceLocation)]
        [SemanticFact("ToolJoint3AtLowestDrillHeight#01", Verbs.Enum.IsPhysicallyLocatedAt, "TJ3#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("ToolJoint3AtLowestDrillHeight#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("StickUpHeight", Nouns.Enum.StickUpHeightVerticalLocation)]
        [SemanticFact("ToolJoint3AtLowestDrillHeight", Verbs.Enum.IsPhysicallyLocatedAt, "StickUpHeight")]
        [Label("Tool-joint #3 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty ToolJoint3AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint4AtStickUpHeight, // 68
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("ToolJoint4AtStickUpHeight")]
        [SemanticFact("ToolJoint4AtStickUpHeight", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("ToolJoint4AtStickUpHeight#01", Nouns.Enum.ComputedData)]
        [SemanticFact("ToolJoint4AtStickUpHeight#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("ToolJoint4AtStickUpHeight#01", Verbs.Enum.HasDynamicValue, "ToolJoint4AtStickUpHeight")]
        [SemanticFact("TJ4#01", Nouns.Enum.ToolJoint2ReferenceLocation)]
        [SemanticFact("ToolJoint4AtLowestDrillHeight#01", Verbs.Enum.IsPhysicallyLocatedAt, "TJ4#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("ToolJoint4AtLowestDrillHeight#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("StickUpHeight", Nouns.Enum.StickUpHeightVerticalLocation)]
        [SemanticFact("ToolJoint4AtLowestDrillHeight", Verbs.Enum.IsPhysicallyLocatedAt, "StickUpHeight")]
        [Label("Tool-joint #4 at Stick-up Height")]
        [GroupName("Top Side Boundaries")]
        public BernoulliDrillingProperty ToolJoint4AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    HeaveCompensation, // 69
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticCategoricalVariable("HeaveCompensation", 3)]
        [SemanticFact("HeaveCompensation", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("HeaveCompensation#01", Nouns.Enum.ComputedData)]
        [SemanticFact("HeaveCompensation#01", Nouns.Enum.EnumerationDataType)]
        [SemanticFact("HeaveCompensation#01", Verbs.Enum.HasDynamicValue, "HeaveCompensation")]
        [SemanticFact("heaveCompensationSystem", Nouns.Enum.HeaveCompensationSystem)]
        [SemanticFact("logical_heaveCompensation", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_heaveCompensation", Verbs.Enum.IsAMechanicalRepresentationFor, "heaveCompensationSystem")]
        [SemanticFact("HeaveCompensation#01", Verbs.Enum.IsPhysicallyLocatedAt, "logical_heaveCompensation")]
        [Label("Heave Compensation")]
        [GroupName("Floating Rig Boundaries")]
        public CategoricalDrillingProperty HeaveCompensation { get; set; } = new CategoricalDrillingProperty(3);

        //    LastStandToBottomHole, // 70
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("LastStandToBottomHole")]
        [SemanticFact("LastStandToBottomHole", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("LastStandToBottomHole#01", Nouns.Enum.ComputedData)]
        [SemanticFact("LastStandToBottomHole#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.HasDynamicValue, "LastStandToBottomHole")]
        [SemanticFact("bos", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos")]
        [SemanticFact("boh", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "boh")]
        [SemanticFact("urho", Nouns.Enum.HoleOpener)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "urho")]
        [SemanticFact("trh", Nouns.Enum.TopOfRatHoleLocation)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "trh")]
        [SemanticFact("tj1", Nouns.Enum.ToolJoint1ReferenceLocation)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "tj1")]
        [SemanticFact("ldh", Nouns.Enum.MinDrillHeightVerticalLocation)]
        [SemanticFact("LastStandToBottomHole#01", Verbs.Enum.IsPhysicallyLocatedAt, "ldh")]
        [Label("Last Stand to Bottom Hole")]
        [GroupName("Downhole Boundaries")]
        public BernoulliDrillingProperty LastStandToBottomHole { get; set; } = new BernoulliDrillingProperty();
        //    WhirlInDrillString, // 71
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("WhirlInDrillString")]
        [SemanticFact("WhirlInDrillString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("WhirlInDrillString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("WhirlInDrillString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("WhirlInDrillString#01", Verbs.Enum.HasDynamicValue, "WhirlInDrillString")]
        [SemanticFact("LateralMotion", Nouns.Enum.LateralMotionType)]
        [SemanticFact("WhirlInDrillString#01", Verbs.Enum.HasMotionType, "LateralMotion")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("WhirlInDrillString#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAMechanicalRepresentationFor, "DS")]
        [SemanticFact("WhirlInDrillString#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_DS")]
        [Label("Whirl in Drill-string")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty WhirlInDrillString { get; set; } = new BernoulliDrillingProperty();
        //    HFTO, // 72
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("HFTO")]
        [SemanticFact("HFTO", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("HFTO#01", Nouns.Enum.ComputedData)]
        [SemanticFact("HFTO#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("HFTO#01", Verbs.Enum.HasDynamicValue, "HFTO")]
        [SemanticFact("AbnormalHFTO", Nouns.Enum.HFTO)]
        [SemanticFact("HFTO#01", Verbs.Enum.IsRelatedToDrillingIncident, "AbnormalHFTO")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("HFTO#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("HFTO#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA")]
        [Label("HFTO")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty HFTO { get; set; } = new BernoulliDrillingProperty();
        //    AxialOscillations, // 73
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("AxialOscillations")]
        [SemanticFact("AxialOscillations", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("AxialOscillations#01", Nouns.Enum.ComputedData)]
        [SemanticFact("AxialOscillations#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("AxialOscillations#01", Verbs.Enum.HasDynamicValue, "AxialOscillations")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("AxialOscillations#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("AbnormalAxialOscillation", Nouns.Enum.AbnormalAxialOscillation)]
        [SemanticFact("AxialOscillations#01", Verbs.Enum.IsRelatedToDrillingIncident, "AbnormalAxialOscillation")]
        [SemanticFact("bos", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("AxialOscillations#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos")]
        [Label("Axial Oscillations")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty AxialOscillations { get; set; } = new BernoulliDrillingProperty();
        //    TorsionalOscillations, // 74
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("TorsionalOscillations")]
        [SemanticFact("TorsionalOscillations", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("TorsionalOscillations#01", Nouns.Enum.ComputedData)]
        [SemanticFact("TorsionalOscillations#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("TorsionalOscillations#01", Verbs.Enum.HasDynamicValue, "TorsionalOscillations")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("TorsionalOscillations#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [SemanticFact("AbnormalTorsionalOscillation", Nouns.Enum.AbnormalTorsionalOscillation)]
        [SemanticFact("TorsionalOscillations#01", Verbs.Enum.IsRelatedToDrillingIncident, "AbnormalTorsionalOscillation")]
        [SemanticFact("bos", Nouns.Enum.BottomOfStringReferenceLocation)]
        [SemanticFact("AxialOscillations#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos")]
        [Label("Torsional Oscillations")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty TorsionalOscillations { get; set; } = new BernoulliDrillingProperty();
        //    LateralShocksInBHA, // 75
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("LateralShocksInBHA")]
        [SemanticFact("LateralShocksInBHA", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("LateralShocksInBHA#01", Nouns.Enum.ComputedData)]
        [SemanticFact("LateralShocksInBHA#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("LateralShocksInBHA#01", Verbs.Enum.HasDynamicValue, "LateralShocksInBHA")]
        [SemanticFact("Shock", Nouns.Enum.Shock)]
        [SemanticFact("LateralShocksInBHA#01", Verbs.Enum.IsRelatedToDrillingIncident, "Shock")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("LateralShocksInBHA#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA")]
        [Label("Lateral Shocks in BHA")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty LateralShocksInBHA { get; set; } = new BernoulliDrillingProperty();
        //    LateralShocksInDrillString // 76
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("LateralShocksInDrillString")]
        [SemanticFact("LateralShocksInDrillString", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("LateralShocksInDrillString#01", Nouns.Enum.ComputedData)]
        [SemanticFact("LateralShocksInDrillString#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("LateralShocksInDrillString#01", Verbs.Enum.HasDynamicValue, "LateralShocksInDrillString")]
        [SemanticFact("Shock", Nouns.Enum.Shock)]
        [SemanticFact("LateralShocksInBHA#01", Verbs.Enum.IsRelatedToDrillingIncident, "Shock")]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAMechanicalRepresentationFor, "DS")]
        [SemanticFact("LateralShocksInDrillString#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_DS")]
        [Label("Lateral Shocks in Drill-string")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty LateralShocksInDrillString { get; set; } = new BernoulliDrillingProperty();
        //      StringRotationImpeded // 77
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StringRotationImpeded")]
        [SemanticFact("StringRotationImpeded", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StringRotationImpeded#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StringRotationImpeded#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StringRotationImpeded#01", Verbs.Enum.HasDynamicValue, "StringRotationImpeded")]
        [SemanticFact("OverTorque", Nouns.Enum.OverTorque)]
        [SemanticFact("StringRotationImpeded#01", Verbs.Enum.IsRelatedToDrillingIncident, "OverTorque")]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAMechanicalRepresentationFor, "DS")]
        [SemanticFact("StringRotationImpeded#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_DS")]
        [Label("String Rotation Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty StringRotationImpeded { get; set; } = new BernoulliDrillingProperty();

        //      AnnulusFlowImpeded // 78
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("AnnulusFlowImpeded")]
        [SemanticFact("AnnulusFlowImpeded", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("AnnulusFlowImpeded#01", Nouns.Enum.ComputedData)]
        [SemanticFact("AnnulusFlowImpeded#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("AnnulusFlowImpeded#01", Verbs.Enum.HasDynamicValue, "AnnulusFlowImpeded")]
        [SemanticFact("Plugged", Nouns.Enum.Plugged)]
        [SemanticFact("AnnulusFlowImpeded#01", Verbs.Enum.IsRelatedToDrillingIncident, "Plugged")]
        [SemanticFact("DrillPipesAnnular", Nouns.Enum.DrillPipesAnnular)]
        [SemanticFact("logical_DrillPipesAnnular", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("logical_DrillPipesAnnular", Verbs.Enum.IsAHydraulicRepresentationFor, "DrillPipesAnnular")]
        [SemanticFact("AnnulusFlowImpeded#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_DrillPipesAnnular")]
        [Label("Annulus Flow Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty AnnulusFlowImpeded { get; set; } = new BernoulliDrillingProperty();

        //      StringFlowImpeded // 79
        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticBernoulliVariable("StringFlowImpeded")]
        [SemanticFact("StringFlowImpeded", Nouns.Enum.DynamicDrillingSignal)]
        [SemanticFact("StringFlowImpeded#01", Nouns.Enum.ComputedData)]
        [SemanticFact("StringFlowImpeded#01", Nouns.Enum.BooleanDataType)]
        [SemanticFact("StringFlowImpeded#01", Verbs.Enum.HasDynamicValue, "StringFlowImpeded")]
        [SemanticFact("Plugged", Nouns.Enum.Plugged)]
        [SemanticFact("StringFlowImpeded#01", Verbs.Enum.IsRelatedToDrillingIncident, "Plugged")]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAHydraulicRepresentationFor, "DS")]
        [SemanticFact("StringFlowImpeded#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_DS")]
        [Label("String Flow Impeded")]
        [GroupName("Abnormal Downhole Boundaries")]
        public BernoulliDrillingProperty StringFlowImpeded { get; set; } = new BernoulliDrillingProperty();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ProbabilisticMicroStates other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                bool equal = true;
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType.IsSubclassOf(typeof(DrillingProperty)))
                    {
                        object? val1 = property.GetValue(this);
                        object? val2 = property.GetValue(other);
                        if (val1 is not null and DrillingProperty drillProp1 && val2 is not null and DrillingProperty drillProp2)
                        { 
                            equal &= drillProp1.Equals(drillProp2);
                        }
                        else
                        {
                            equal &= val1 == null && val2 == null;
                        }
                    }
                }
                return equal;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void CopyTo(ProbabilisticMicroStates? dest)
        {
            if (dest != null)
            {
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType.IsSubclassOf(typeof(DrillingProperty)))
                    {
                        object? val1 = property.GetValue(this);
                        object? val2 = property.GetValue(dest);
                        if (val1 is not null and DrillingProperty drillProp1 && val2 is not null and DrillingProperty drillProp2)
                        {
                            drillProp1.CopyTo(drillProp2);
                        }
                    }
                }
            }
        }

        public bool RegisterToBlackboard(IOPCUADWISClient? DWISClient, Dictionary<string, QueryResult>? placeHolders)
        {
            if (DWISClient != null && placeHolders != null)
            {
                Type type = GetType();
                Assembly assembly = type.Assembly;
                PropertyInfo[] properties = type.GetProperties();
                
                bool ok = true;
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType.IsSubclassOf(typeof(DiscreteDrillingProperty)))
                    {
                        string propName = property.Name;
                        if (!string.IsNullOrEmpty(propName))
                        {
                            string manifestName = type.FullName + "_" + propName;
                            ManifestFile? manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, type.FullName, propName, manifestName, companyName_, prefix_);
                            Dictionary<string, QuerySpecification>? queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName, propName);
                            if (queries != null && queries.Count > 0 && manifestFile != null)
                            {
                                QueryResult? res = null;
                                foreach (var kvp in queries)
                                {
                                    if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                                    {
                                        StringBuilder stringBuilder = new StringBuilder();
                                        stringBuilder.AppendLine(@"PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>");
                                        stringBuilder.AppendLine(@"PREFIX ddhub:<http://ddhub.no/>");
                                        stringBuilder.AppendLine(@"PREFIX quantity:<http://ddhub.no/UnitAndQuantity>");
                                        stringBuilder.AppendLine(@"SELECT ?AxialVelocityTopOfString");
                                        stringBuilder.AppendLine(@"WHERE {");
                                        stringBuilder.AppendLine(@"	?AxialVelocityTopOfString_01 rdf:type ddhub:ComputedData .");
                                        stringBuilder.AppendLine(@"	?AxialVelocityTopOfString_01 rdf:type ddhub:EnumerationDataType .");
                                        stringBuilder.AppendLine(@"	?AxialVelocityTopOfString_01 ddhub:HasDynamicValue ?AxialVelocityTopOfString .");
                                        stringBuilder.AppendLine(@"	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .");
                                        stringBuilder.AppendLine(@"	?AxialVelocityTopOfString_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .");
                                        stringBuilder.AppendLine(@"	?MovingAverage rdf:type ddhub:MovingAverage .");
                                        stringBuilder.AppendLine(@"	?AxialVelocityTopOfString_01 ddhub:IsTransformationOutput ?MovingAverage . ");
                                        stringBuilder.AppendLine(@"}");
                                        stringBuilder.AppendLine(@"");
                                        stringBuilder.AppendLine(@"");
                                        //string sparql = stringBuilder.ToString();
                                        string sparql = kvp.Value.SparQL;
                                        var result = DWISClient.GetQueryResult(sparql);
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
                                    if (r != null && r.Success )
                                    {
                                        res = null;
                                        foreach (var kvp in queries)
                                        {
                                            if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                                            {
                                                StringBuilder stringBuilder = new StringBuilder();
                                                stringBuilder.AppendLine(@"PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>");
                                                stringBuilder.AppendLine(@"PREFIX ddhub:<http://ddhub.no/>");
                                                stringBuilder.AppendLine(@"PREFIX quantity:<http://ddhub.no/UnitAndQuantity>");
                                                stringBuilder.AppendLine(@"SELECT ?ZeroCuttingsFlowAnnulusOutletThreshold ?factOptionSet");
                                                stringBuilder.AppendLine(@"WHERE {");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 rdf:type ddhub:Limit .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:HasValue ?ZeroCuttingsFlowAnnulusOutletThreshold .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:IsOfMeasurableQuantity quantity:VolumetricFlowrateDrilling .");
                                                stringBuilder.AppendLine(@"	?AnnulusTerminator_01 rdf:type ddhub:WellControlSystem .");
                                                stringBuilder.AppendLine(@"	?Logical_AnnulusTerminator_01 rdf:type ddhub:HydraulicLogicalElement .");
                                                stringBuilder.AppendLine(@"	?Logical_AnnulusTerminator_01 ddhub:IsAHydraulicRepresentationFor ?AnnulusTerminator_01 .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:IsHydraulicallyLocatedAt ?Logical_AnnulusTerminator_01 .");
                                                stringBuilder.AppendLine(@"	?LiquidComponent_01 rdf:type ddhub:LiquidComponent .");
                                                stringBuilder.AppendLine(@"	?CuttingsComponent_01 rdf:type ddhub:CuttingsComponent .");
                                                stringBuilder.AppendLine(@"	?GasComponent_01 rdf:type ddhub:GasComponent .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:ConcernsAFluidComponent ?CuttingsComponent_01 .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:ConcernsAFluidComponent ?LiquidComponent_01 .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:ConcernsAFluidComponent ?GasComponent_01 .");
                                                stringBuilder.AppendLine(@"	?bh_01 rdf:type ddhub:HoleBottomLocation .");
                                                stringBuilder.AppendLine(@"	?MovingAverage rdf:type ddhub:MovingAverage .");
                                                stringBuilder.AppendLine(@"	?signal_01 rdf:type ddhub:DrillingDataPoint .");
                                                stringBuilder.AppendLine(@"	?signal_01 ddhub:IsTransformationOutput ?MovingAverage .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:IsToBeComparedWith ?signal_01 .");
                                                stringBuilder.AppendLine(@" BIND ('1' AS ?factOptionSet) ");
                                                stringBuilder.AppendLine(@"  FILTER NOT EXISTS {");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:ConcernsAFluidComponent ?LiquidComponent_01 .");
                                                stringBuilder.AppendLine(@"	?ZeroCuttingsFlowAnnulusOutletThreshold_01 ddhub:ConcernsAFluidComponent ?GasComponent_01 .");
                                                stringBuilder.AppendLine(@" }");
                                                stringBuilder.AppendLine(@"}");
                                                stringBuilder.AppendLine(@"");
                                                string sparql = stringBuilder.ToString(); // sparql = kvp.Value.SparQL
                                                //string sparql = kvp.Value.SparQL;
                                                var result = DWISClient.GetQueryResult(sparql);
                                                if (result != null && result.Results != null && result.Results.Count > 0)
                                                {
                                                    res = result;
                                                    break;
                                                }
                                            }
                                        }
                                        if (res != null)
                                        {
                                            placeHolders.Add(propName, res);
                                        }
                                        else
                                        {
                                            ok = false;
                                        }
                                    }
                                }
                                else
                                {
                                    // a manifest has already been injected.
                                    placeHolders.Add(propName, res);
                                }
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

        public bool SendToBlackboard(IOPCUADWISClient? DWISClient, Dictionary<string, QueryResult>? placeHolders)
        {
            bool ok = false;
            if (DWISClient != null && placeHolders != null)
            {
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                ok = true;
                foreach (PropertyInfo property in properties)
                {
                    bool ok1 = false;
                    if (property != null && property.PropertyType.IsSubclassOf(typeof(DiscreteDrillingProperty)))
                    {
                        string propName = property.Name;
                        if (!string.IsNullOrEmpty(propName) && placeHolders.ContainsKey(propName))
                        {
                            QueryResult? queryResult = placeHolders[propName];
                            if (queryResult != null && queryResult.Count > 0 && queryResult[0].Count > 0)
                            {
                                object? propValue = property.GetValue(this);
                                if (propValue is not null and DiscreteDrillingProperty discretePropValue)
                                {
                                    uint? numberOfStates = discretePropValue.NumberOfStates;
                                    double[]? probabilities = discretePropValue.Probabilities;
                                    NodeIdentifier id = queryResult[0][0];
                                    if (numberOfStates != null && probabilities != null && probabilities.Length >= numberOfStates &&
                                        id != null && !string.IsNullOrEmpty(id.ID) && !string.IsNullOrEmpty(id.NameSpace))
                                    {
                                        // OPC-UA code to set the value at the node id = ID
                                        (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                                        outputs[0].nameSpace = id.NameSpace;
                                        outputs[0].id = id.ID;
                                        outputs[0].value = probabilities;
                                        outputs[0].sourceTimestamp = DateTime.UtcNow;
                                        DWISClient.UpdateAnyVariables(outputs);
                                        ok1 = true;
                                    }
                                }
                            }
                        }
                    }
                    ok &= ok1;
                }
            }
            return ok;
        }
    }
}
