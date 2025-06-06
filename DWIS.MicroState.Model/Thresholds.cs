﻿using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.Vocabulary.Schemas;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.UnitConversion.Conversion;
using OSDC.UnitConversion.Conversion.DrillingEngineering;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DWIS.MicroState.Model
{
    [AccessToVariable(CommonProperty.VariableAccessType.Assignable)]
    [SemanticTypeVariable("MicroStateThresholds")]
    [SemanticFact("MicroStateThresholds", Nouns.Enum.DynamicDrillingSignal)]
    [SemanticFact("MicroStateThresholds#01", Nouns.Enum.ConfigurationData)]
    [SemanticFact("MicroStateThresholds#01", Verbs.Enum.HasDynamicValue, "MicroStateThresholds")]
    [SemanticFact("MicroStateInterpreter", Nouns.Enum.DWISDrillingProcessStateInterpreter)]
    [SemanticFact("MicroStateThresholds#01", Verbs.Enum.IsProvidedTo, "MicroStateInterpreter")]
    [SemanticFact("MicroStateThresholds#01", Verbs.Enum.IsLimitFor, "MicroStateInterpreter")]
    public class Thresholds
    {
        private static string prefix_ = "DWIS:MicroState:Thresholds:";
        private static string companyName_ = "DWIS";

        /// <summary>
        /// the timestamp in UTC when the thresholds have been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroAxialVelocityTopOfStringThreshold")]
        [SemanticExclusiveOr(1,2)]
        [SemanticFact("ZeroAxialVelocityTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroAxialVelocityTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroAxialVelocityTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroAxialVelocityTopOfStringThreshold")]
        [SemanticFact("ZeroAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.BlockVelocityDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroAxialVelocityTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroAxialVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableAxialVelocityTopOfStringThreshold")]
        [SemanticFact("StableAxialVelocityTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableAxialVelocityTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableAxialVelocityTopOfStringThreshold#01", Verbs.Enum.HasValue, "StableAxialVelocityTopOfStringThreshold")]
        [SemanticFact("StableAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.BlockVelocityDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableAxialVelocityTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableAxialVelocityTopOfStringThreshold#01")]
        public ScalarDrillingProperty StableAxialVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroRotationalVelocityTopOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroRotationalVelocityTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroRotationalVelocityTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroRotationalVelocityTopOfStringThreshold")]
        [SemanticFact("ZeroRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroRotationalVelocityTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroRotationalVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableRotationalVelocityTopOfStringThreshold")]
        [SemanticFact("StableRotationalVelocityTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableRotationalVelocityTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.HasValue, "StableRotationalVelocityTopOfStringThreshold")]
        [SemanticFact("StableRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableRotationalVelocityTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableRotationalVelocityTopOfStringThreshold#01")]
        public ScalarDrillingProperty StableRotationalVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowTopOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroFlowTopOfStringThreshold")]
        [SemanticFact("ZeroFlowTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroFlowTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroFlowTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowTopOfStringThreshold")]
        [SemanticFact("StableFlowTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowTopOfStringThreshold#01", Verbs.Enum.HasValue, "StableFlowTopOfStringThreshold")]
        [SemanticFact("StableFlowTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableFlowTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowTopOfStringThreshold#01")]
        public ScalarDrillingProperty StableFlowTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroTensionTopOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroTensionTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroTensionTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroTensionTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroTensionTopOfStringThreshold")]
        [SemanticFact("ZeroTensionTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroTensionTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroTensionTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroTensionTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroTensionTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableTensionTopOfStringThreshold")]
        [SemanticFact("StableTensionTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableTensionTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableTensionTopOfStringThreshold#01", Verbs.Enum.HasValue, "StableTensionTopOfStringThreshold")]
        [SemanticFact("StableTensionTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableTensionTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableTensionTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableTensionTopOfStringThreshold#01")] 
        public ScalarDrillingProperty StableTensionTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroPressureTopOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroPressureTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroPressureTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroPressureTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroPressureTopOfStringThreshold")]
        [SemanticFact("ZeroPressureTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroPressureTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroPressureTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroPressureTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroPressureTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StablePressureTopOfStringThreshold")]
        [SemanticFact("StablePressureTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StablePressureTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StablePressureTopOfStringThreshold#01", Verbs.Enum.HasValue, "StablePressureTopOfStringThreshold")]
        [SemanticFact("StablePressureTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StablePressureTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StablePressureTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StablePressureTopOfStringThreshold#01")]
        public ScalarDrillingProperty StablePressureTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroTorqueTopOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroTorqueTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroTorqueTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroTorqueTopOfStringThreshold#01", Verbs.Enum.HasValue, "ZeroTorqueTopOfStringThreshold")]
        [SemanticFact("ZeroTorqueTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.TorqueDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("ZeroTorqueTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroTorqueTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroTorqueTopOfStringThreshold#01")]
        public ScalarDrillingProperty ZeroTorqueTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableTorqueTopOfStringThreshold")]
        [SemanticFact("StableTorqueTopOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableTorqueTopOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableTorqueTopOfStringThreshold#01", Verbs.Enum.HasValue, "StableTorqueTopOfStringThreshold")]
        [SemanticFact("StableTorqueTopOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.TorqueDrilling)]
        [SemanticFact("tos#01", Nouns.Enum.TopOfStringReferenceLocation)]
        [SemanticFact("StableTorqueTopOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "tos#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableTorqueTopOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableTorqueTopOfStringThreshold#01")]
        public ScalarDrillingProperty StableTorqueTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowAnnulusOutletThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.HasValue, "ZeroFlowAnnulusOutletThreshold")]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowAnnulusOutletThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowAnnulusOutletThreshold#01")]
        public ScalarDrillingProperty ZeroFlowAnnulusOutletThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowAnnulusOutletThreshold")]
        [SemanticFact("StableFlowAnnulusOutletThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.HasValue, "StableFlowAnnulusOutletThreshold")]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("SolidComponent#01", Nouns.Enum.SolidComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "SolidComponent#01")]
        [SemanticFact("StableFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowAnnulusOutletThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowAnnulusOutletThreshold#01")]
        public ScalarDrillingProperty StableFlowAnnulusOutletThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroBottomOfStringRockForceThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("ZeroBottomOfStringRockForceThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroBottomOfStringRockForceThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroBottomOfStringRockForceThreshold#01", Verbs.Enum.HasValue, "ZeroBottomOfStringRockForceThreshold")]
        [SemanticFact("ZeroBottomOfStringRockForceThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "ZeroBottomOfStringRockForceThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "ZeroBottomOfStringRockForceThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroBottomOfStringRockForceThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroBottomOfStringRockForceThreshold#01")]
        public ScalarDrillingProperty ZeroBottomOfStringRockForceThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableBottomOfStringRockForceThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("StableBottomOfStringRockForceThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableBottomOfStringRockForceThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableBottomOfStringRockForceThreshold#01", Verbs.Enum.HasValue, "StableBottomOfStringRockForceThreshold")]
        [SemanticFact("StableBottomOfStringRockForceThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "StableBottomOfStringRockForceThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "StableBottomOfStringRockForceThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableBottomOfStringRockForceThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableBottomOfStringRockForceThreshold#01")]
        public ScalarDrillingProperty StableBottomOfStringRockForceThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroBottomOfStringRotationalVelocityThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("ZeroBottomOfStringRotationalVelocityThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroBottomOfStringRotationalVelocityThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.HasValue, "ZeroBottomOfStringRotationalVelocityThreshold")]
        [SemanticFact("ZeroBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "ZeroBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "ZeroBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroBottomOfStringRotationalVelocityThreshold#01")]
        public ScalarDrillingProperty ZeroRotationalVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableBottomOfStringRotationalVelocityThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("StableBottomOfStringRotationalVelocityThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableBottomOfStringRotationalVelocityThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.HasValue, "StableBottomOfStringRotationalVelocityThreshold")]
        [SemanticFact("StableBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "StableBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "StableBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableBottomOfStringRotationalVelocityThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableBottomOfStringRotationalVelocityThreshold#01")]
        public ScalarDrillingProperty StableRotationalVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroBottomOfStringAxialVelocityThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("ZeroBottomOfStringAxialVelocityThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroBottomOfStringAxialVelocityThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.HasValue, "ZeroBottomOfStringAxialVelocityThreshold")]
        [SemanticFact("ZeroBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.BlockVelocityDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "ZeroBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "ZeroBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroBottomOfStringAxialVelocityThreshold#01")]
        public ScalarDrillingProperty ZeroAxialVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableBottomOfStringAxialVelocityThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("StableBottomOfStringAxialVelocityThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableBottomOfStringAxialVelocityThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.HasValue, "StableBottomOfStringAxialVelocityThreshold")]
        [SemanticFact("StableBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.BlockVelocityDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "StableBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "StableBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableBottomOfStringAxialVelocityThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableBottomOfStringAxialVelocityThreshold#01")]
        public ScalarDrillingProperty StableAxialVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroBottomOfStringFlowThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("ZeroBottomOfStringFlowThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroBottomOfStringFlowThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroBottomOfStringFlowThreshold#01", Verbs.Enum.HasValue, "ZeroBottomOfStringFlowThreshold")]
        [SemanticFact("ZeroBottomOfStringFlowThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "ZeroBottomOfStringFlowThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "ZeroBottomOfStringFlowThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroBottomOfStringFlowThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroBottomOfStringFlowThreshold#01")]
        public ScalarDrillingProperty ZeroFlowBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableBottomOfStringFlowThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("StableBottomOfStringFlowThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableBottomOfStringFlowThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableBottomOfStringFlowThreshold#01", Verbs.Enum.HasValue, "StableBottomOfStringFlowThreshold")]
        [SemanticFact("StableBottomOfStringFlowThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "StableBottomOfStringFlowThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "StableBottomOfStringFlowThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableBottomOfStringFlowThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableBottomOfStringFlowThreshold#01")]
        public ScalarDrillingProperty StableFlowBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroHoleOpenerFlowThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroHoleOpenerFlowThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroHoleOpenerFlowThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroHoleOpenerFlowThreshold#01", Verbs.Enum.HasValue, "ZeroHoleOpenerFlowThreshold")]
        [SemanticFact("ZeroHoleOpenerFlowThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("ZeroHoleOpenerFlowThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_urho#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroHoleOpenerFlowThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroHoleOpenerFlowThreshold#01")]
        public ScalarDrillingProperty ZeroFlowHoleOpenerThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableHoleOpenerFlowThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StableHoleOpenerFlowThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableHoleOpenerFlowThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableHoleOpenerFlowThreshold#01", Verbs.Enum.HasValue, "StableHoleOpenerFlowThreshold")]
        [SemanticFact("StableHoleOpenerFlowThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("StableHoleOpenerFlowThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_urho#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableHoleOpenerFlowThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableHoleOpenerFlowThreshold#01")]
        public ScalarDrillingProperty StableFlowHoleOpenerThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroHoleOpenerOnRockForceThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("ZeroHoleOpenerOnRockForceThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroHoleOpenerOnRockForceThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroHoleOpenerOnRockForceThreshold#01", Verbs.Enum.HasValue, "ZeroHoleOpenerOnRockForceThreshold")]
        [SemanticFact("ZeroHoleOpenerOnRockForceThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("ZeroHoleOpenerOnRockForceThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_urho#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroHoleOpenerOnRockForceThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroHoleOpenerOnRockForceThreshold#01")]
        public ScalarDrillingProperty ZeroHoleOpenerOnRockForceThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("MinimumPressureFloatValve")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("MinimumPressureFloatValve", Nouns.Enum.DrillingSignal)]
        [SemanticFact("MinimumPressureFloatValve#01", Nouns.Enum.MinimumLimit)]
        [SemanticFact("MinimumPressureFloatValve#01", Verbs.Enum.HasValue, "MinimumPressureFloatValve")]
        [SemanticFact("MinimumPressureFloatValve#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureDrilling)]
        [SemanticFact("FloatValve#01", Nouns.Enum.FloatValveNonreturnValve)]
        [SemanticFact("Logical_FloatValve#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_FloatValve#01", Verbs.Enum.IsAHydraulicRepresentationFor, "FloatValve#01")]
        [SemanticFact("MinimumPressureFloatValve#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_FloatValve#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("MinimumPressureFloatValve#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "MinimumPressureFloatValve#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "MinimumPressureFloatValve#01")]
        public ScalarDrillingProperty MinimumPressureFloatValve { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowBoosterPumpThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowBoosterPumpThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowBoosterPumpThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowBoosterPumpThreshold#01", Verbs.Enum.HasValue, "ZeroFlowBoosterPumpThreshold")]
        [SemanticFact("ZeroFlowBoosterPumpThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("BoosterPump#01", Nouns.Enum.BoosterPump)]
        [SemanticFact("Logical_BoosterPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BoosterPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BoosterPump#01")]
        [SemanticFact("ZeroFlowBoosterPumpThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BoosterPump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowBoosterPumpThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowBoosterPumpThreshold#01")]
        public ScalarDrillingProperty ZeroFlowBoosterPumpThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowBoosterPumpThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StableFlowBoosterPumpThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowBoosterPumpThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowBoosterPumpThreshold#01", Verbs.Enum.HasValue, "StableFlowBoosterPumpThreshold")]
        [SemanticFact("StableFlowBoosterPumpThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("BoosterPump#01", Nouns.Enum.BoosterPump)]
        [SemanticFact("Logical_BoosterPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BoosterPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BoosterPump#01")]
        [SemanticFact("StableFlowBoosterPumpThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BoosterPump#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowBoosterPumpThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowBoosterPumpThreshold#01")]
        public ScalarDrillingProperty StableFlowBoosterPumpThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowBackPressurePumpThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowBackPressurePumpThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowBackPressurePumpThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowBackPressurePumpThreshold#01", Verbs.Enum.HasValue, "ZeroFlowBackPressurePumpThreshold")]
        [SemanticFact("ZeroFlowBackPressurePumpThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("BackPressurePump#01", Nouns.Enum.BackPressurePump)]
        [SemanticFact("Logical_BackPressurePump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BackPressurePump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BackPressurePump#01")]
        [SemanticFact("ZeroFlowBackPressurePumpThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BackPressurePump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowBackPressurePumpThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowBackPressurePumpThreshold#01")]
        public ScalarDrillingProperty ZeroFlowBackPressurePumpThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowBackPressurePumpThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StableFlowBackPressurePumpThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowBackPressurePumpThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowBackPressurePumpThreshold#01", Verbs.Enum.HasValue, "StableFlowBackPressurePumpThreshold")]
        [SemanticFact("StableFlowBackPressurePumpThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("BackPressurePump#01", Nouns.Enum.BackPressurePump)]
        [SemanticFact("Logical_BackPressurePump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_BackPressurePump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "BackPressurePump#01")]
        [SemanticFact("StableFlowBackPressurePumpThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_BackPressurePump#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowBackPressurePumpThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowBackPressurePumpThreshold#01")]
        public ScalarDrillingProperty StableFlowBackPressurePumpThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("MinimumDifferentialPressureRCDSealing")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("MinimumDifferentialPressureRCDSealing", Nouns.Enum.DrillingSignal)]
        [SemanticFact("MinimumDifferentialPressureRCDSealing#01", Nouns.Enum.Limit)]
        [SemanticFact("MinimumDifferentialPressureRCDSealing#01", Verbs.Enum.HasValue, "MinimumDifferentialPressureRCDSealing")]
        [SemanticFact("MinimumDifferentialPressureRCDSealing#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureDrilling)]
        [SemanticFact("IsolationSeal#01", Nouns.Enum.IsolationSeal)]
        [SemanticFact("Logical_IsolationSeal#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_IsolationSeal#01", Verbs.Enum.IsAHydraulicRepresentationFor, "IsolationSeal#01")]
        [SemanticFact("MinimumDifferentialPressureRCDSealing#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_IsolationSeal#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("MinimumDifferentialPressureRCDSealing#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "MinimumDifferentialPressureRCDSealing#01", Verbs.Enum.IsToBeGreaterThan, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeSmallerThan, "MinimumDifferentialPressureRCDSealing#01")]
        public ScalarDrillingProperty MinimumDifferentialPressureRCDSealingThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("MinimumDifferentialPressureSealBalance")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("MinimumDifferentialPressureSealBalance", Nouns.Enum.DrillingSignal)]
        [SemanticFact("MinimumDifferentialPressureSealBalance#01", Nouns.Enum.MinimumLimit)]
        [SemanticFact("MinimumDifferentialPressureSealBalance#01", Verbs.Enum.HasValue, "MinimumDifferentialPressureSealBalance")]
        [SemanticFact("MinimumDifferentialPressureSealBalance#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureDrilling)]
        [SemanticFact("IsolationSeal#01", Nouns.Enum.IsolationSeal)]
        [SemanticFact("Logical_IsolationSeal#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_IsolationSeal#01", Verbs.Enum.IsAHydraulicRepresentationFor, "IsolationSeal#01")]
        [SemanticFact("MinimumDifferentialPressureSealBalance#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_IsolationSeal#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("DifferentialPressure", Nouns.Enum.RelativePressureReference)]
        [SemanticFact("MinimumDifferentialPressureSealBalance#01", Verbs.Enum.HasPressureReferenceType, "DifferentialPressure")]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "MinimumDifferentialPressureSealBalance#01", Verbs.Enum.IsToBeSmallerThan, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeGreaterThan, "MinimumDifferentialPressureSealBalance#01")]
        public ScalarDrillingProperty MinimumDifferentialPressureSealBalanceThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowFillPumpDGDThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowFillPumpDGDThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowFillPumpDGDThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowFillPumpDGDThreshold#01", Verbs.Enum.HasValue, "ZeroFlowFillPumpDGDThreshold")]
        [SemanticFact("ZeroFlowFillPumpDGDThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("FillPump#01", Nouns.Enum.FillPump)]
        [SemanticFact("Logical_FillPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_FillPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "FillPump#01")]
        [SemanticFact("ZeroFlowFillPumpDGDThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_FillPump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowFillPumpDGDThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowFillPumpDGDThreshold#01")]
        public ScalarDrillingProperty ZeroFlowFillPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowFillPumpDGDThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StableFlowFillPumpDGDThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowFillPumpDGDThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowFillPumpDGDThreshold#01", Verbs.Enum.HasValue, "StableFlowFillPumpDGDThreshold")]
        [SemanticFact("StableFlowFillPumpDGDThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("FillPump#01", Nouns.Enum.FillPump)]
        [SemanticFact("Logical_FillPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_FillPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "FillPump#01")]
        [SemanticFact("StableFlowFillPumpDGDThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_FillPump#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowFillPumpDGDThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowFillPumpDGDThreshold#01")]
        public ScalarDrillingProperty StableFlowFillPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroFlowLiftPumpDGDThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroFlowLiftPumpDGDThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroFlowLiftPumpDGDThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroFlowLiftPumpDGDThreshold#01", Verbs.Enum.HasValue, "ZeroFlowLiftPumpDGDThreshold")]
        [SemanticFact("ZeroFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("LiftPump#01", Nouns.Enum.RiserLiftPump)]
        [SemanticFact("Logical_LiftPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_LiftPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "LiftPump#01")]
        [SemanticFact("ZeroFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_LiftPump#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroFlowLiftPumpDGDThreshold#01")]
        public ScalarDrillingProperty ZeroFlowLiftPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StableFlowLiftPumpDGDThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StableFlowLiftPumpDGDThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StableFlowLiftPumpDGDThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StableFlowLiftPumpDGDThreshold#01", Verbs.Enum.HasValue, "StableFlowLiftPumpDGDThreshold")]
        [SemanticFact("StableFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("LiftPump#01", Nouns.Enum.RiserLiftPump)]
        [SemanticFact("Logical_LiftPump#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_LiftPump#01", Verbs.Enum.IsAHydraulicRepresentationFor, "LiftPump#01")]
        [SemanticFact("StableFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_LiftPump#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "StableFlowLiftPumpDGDThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StableFlowLiftPumpDGDThreshold#01")]
        public ScalarDrillingProperty StableFlowLiftPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroCuttingsFlowAnnulusOutletThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.HasValue, "ZeroCuttingsFlowAnnulusOutletThreshold")]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("AnnulusTerminator#01", Nouns.Enum.WellControlSystem)]
        [SemanticFact("Logical_AnnulusTerminator#01", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("Logical_AnnulusTerminator#01", Verbs.Enum.IsAHydraulicRepresentationFor, "AnnulusTerminator#01")]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "Logical_AnnulusTerminator#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroCuttingsFlowAnnulusOutletThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroCuttingsFlowAnnulusOutletThreshold#01")]
        public ScalarDrillingProperty ZeroCuttingsFlowAnnulusOutletThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroCuttingsFlowBottomHoleThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.HasValue, "ZeroCuttingsFlowBottomHoleThreshold")]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bh#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroCuttingsFlowBottomHoleThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroCuttingsFlowBottomHoleThreshold#01")]
        public ScalarDrillingProperty ZeroCuttingsFlowBottomHoleThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ZeroCuttingsFlowTopOfRatHoleThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.HasValue, "ZeroCuttingsFlowTopOfRatHoleThreshold")]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("trh#01", Nouns.Enum.TopOfRatHoleLocation)]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "trh#01")]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [ExcludeFact("ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ZeroCuttingsFlowTopOfRatHoleThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ZeroCuttingsFlowTopOfRatHoleThreshold#01")]
        public ScalarDrillingProperty ZeroCuttingsFlowTopOfRatHoleThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("HardStringerThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("HardStringerThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("HardStringerThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("HardStringerThreshold#01", Verbs.Enum.HasValue, "HardStringerThreshold")]
        [SemanticFact("HardStringerThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.FormationStrengthDrilling)]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("HardStringerThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [ExcludeFact("HardStringerThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [ExcludeFact("HardStringerThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("HardStringerThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bh#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "HardStringerThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "HardStringerThreshold#01")]
        public ScalarDrillingProperty HardStringerThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ChangeOfFormationUCSSlopeThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.HasValue, "ChangeOfFormationUCSSlopeThreshold")]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureGradientPerLengthDrilling)]
        [SemanticFact("LiquidComponent#01", Nouns.Enum.LiquidComponent)]
        [SemanticFact("CuttingsComponent#01", Nouns.Enum.CuttingsComponent)]
        [SemanticFact("GasComponent#01", Nouns.Enum.GasComponent)]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "CuttingsComponent#01")]
        [ExcludeFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "LiquidComponent#01")]
        [ExcludeFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.ConcernsAFluidComponent, "GasComponent#01")]
        [SemanticFact("bh#01", Nouns.Enum.HoleBottomLocation)]
        [SemanticFact("ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bh#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ChangeOfFormationUCSSlopeThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ChangeOfFormationUCSSlopeThreshold#01")]
        public ScalarDrillingProperty ChangeOfFormationUCSSlopeThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ForceOnLedgeThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ForceOnLedgeThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ForceOnLedgeThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ForceOnLedgeThreshold#01", Verbs.Enum.HasValue, "ForceOnLedgeThreshold")]
        [SemanticFact("ForceOnLedgeThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("ledge#01", Nouns.Enum.LedgeLocation)]
        [SemanticFact("ForceOnLedgeThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "ledge#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ForceOnLedgeThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ForceOnLedgeThreshold#01")]
        public ScalarDrillingProperty ForceOnLedgeThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ForceOnCuttingsBedThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ForceOnCuttingsBedThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ForceOnCuttingsBedThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ForceOnCuttingsBedThreshold#01", Verbs.Enum.HasValue, "ForceOnCuttingsBedThreshold")]
        [SemanticFact("ForceOnCuttingsBedThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("CuttingsBed#01", Nouns.Enum.CuttingsBedLocation)]
        [SemanticFact("ForceOnCuttingsBedThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "CuttingsBed#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "ForceOnCuttingsBedThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ForceOnCuttingsBedThreshold#01")]
        public ScalarDrillingProperty ForceOnCuttingsBedThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("ForceDifferentialStickingThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("ForceDifferentialStickingThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("ForceDifferentialStickingThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("ForceDifferentialStickingThreshold#01", Verbs.Enum.HasValue, "ForceDifferentialStickingThreshold")]
        [SemanticFact("ForceDifferentialStickingThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.ForceDrilling)]
        [SemanticFact("DifferentialSticking#01", Nouns.Enum.DifferentialStickingLocation)]
        [SemanticFact("ForceDifferentialStickingThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "DifferentialSticking#01")]
        [SemanticFact("StandardDeviation", Nouns.Enum.MovingStandardDeviation)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "StandardDeviation")]
        [OptionalFact(1, "ForceDifferentialStickingThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "ForceDifferentialStickingThreshold#01")]
        public ScalarDrillingProperty ForceDifferentialStickingThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("FluidFlowFormationThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("FluidFlowFormationThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("FluidFlowFormationThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("FluidFlowFormationThreshold#01", Verbs.Enum.HasValue, "FluidFlowFormationThreshold")]
        [SemanticFact("FluidFlowFormationThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("FormationFluidFlow#01", Nouns.Enum.FormationFluidTransferLocation)]
        [SemanticFact("FluidFlowFormationThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "FormationFluidFlow#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "FluidFlowFormationThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "FluidFlowFormationThreshold#01")]
        public ScalarDrillingProperty FluidFlowFormationThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("FlowCavingsFromFormationThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("FlowCavingsFromFormationThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("FlowCavingsFromFormationThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("FlowCavingsFromFormationThreshold#01", Verbs.Enum.HasValue, "FlowCavingsFromFormationThreshold")]
        [SemanticFact("FlowCavingsFromFormationThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("FormationCollapse#01", Nouns.Enum.FormationCollapseLocation)]
        [SemanticFact("FlowCavingsFromFormationThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "FormationCollapse#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "FlowCavingsFromFormationThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "FlowCavingsFromFormationThreshold#01")]
        public ScalarDrillingProperty FlowCavingsFromFormationThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("WhirlRateBottomOfStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticExclusiveOr(4, 5)]
        [SemanticFact("WhirlRateBottomOfStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("WhirlRateBottomOfStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("WhirlRateBottomOfStringThreshold#01", Verbs.Enum.HasValue, "WhirlRateBottomOfStringThreshold")]
        [SemanticFact("WhirlRateBottomOfStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [OptionalFact(4, "bos#01", Nouns.Enum.BottomOfStringReferenceLocation)]
        [OptionalFact(4, "WhirlRateBottomOfStringThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "bos#01")]
        [OptionalFact(5, "dst#01", Nouns.Enum.DrillstemTerminator)]
        [OptionalFact(5, "logical_dst#01", Nouns.Enum.MechanicalLogicalElement)]
        [OptionalFact(5, "logical_dst#01", Verbs.Enum.IsAMechanicalRepresentationFor, "dst#01")]
        [OptionalFact(5, "WhirlRateBottomOfStringThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_dst#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "WhirlRateBottomOfStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "WhirlRateBottomOfStringThreshold#01")]
        public ScalarDrillingProperty WhirlRateBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("WhirlRateHoleOpenerThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("WhirlRateHoleOpenerThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("WhirlRateHoleOpenerThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("WhirlRateHoleOpenerThreshold#01", Verbs.Enum.HasValue, "WhirlRateHoleOpenerThreshold")]
        [SemanticFact("WhirlRateHoleOpenerThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [SemanticFact("urho#01", Nouns.Enum.HoleOpener)]
        [SemanticFact("logical_urho#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_urho#01", Verbs.Enum.IsAMechanicalRepresentationFor, "urho#01")]
        [SemanticFact("WhirlRateHoleOpenerThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_urho#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "WhirlRateHoleOpenerThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "WhirlRateHoleOpenerThreshold#01")]
        public ScalarDrillingProperty WhirlRateHoleOpenerThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("WhirlRateDrillStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("WhirlRateDrillStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("WhirlRateDrillStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("WhirlRateDrillStringThreshold#01", Verbs.Enum.HasValue, "WhirlRateDrillStringThreshold")]
        [SemanticFact("WhirlRateDrillStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AngularVelocityDrilling)]
        [SemanticFact("ds#01", Nouns.Enum.DrillString)]
        [SemanticFact("logical_ds#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_ds#01", Verbs.Enum.IsAMechanicalRepresentationFor, "ds#01")]
        [SemanticFact("WhirlRateDrillStringThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_ds#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "WhirlRateDrillStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "WhirlRateDrillStringThreshold#01")]
        public ScalarDrillingProperty WhirlRateDrillStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("PowerHFTOThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("PowerHFTOThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("PowerHFTOThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("PowerHFTOThreshold#01", Verbs.Enum.HasValue, "PowerHFTOThreshold")]
        [SemanticFact("PowerHFTOThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PowerDrilling)]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("PowerHFTOThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "PowerHFTOThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "PowerHFTOThreshold#01")]
        public ScalarDrillingProperty PowerHFTOThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("LateralShockRateBHAThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("LateralShockRateBHAThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("LateralShockRateBHAThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("LateralShockRateBHAThreshold#01", Verbs.Enum.HasValue, "LateralShockRateBHAThreshold")]
        [SemanticFact("LateralShockRateBHAThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, BasePhysicalQuantity.QuantityEnum.ShockRate)]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("LateralShockRateBHAThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "LateralShockRateBHAThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "LateralShockRateBHAThreshold#01")]
        public ScalarDrillingProperty LateralShockRateBHAThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("LateralShockRateDrillStringThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("LateralShockRateDrillStringThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("LateralShockRateDrillStringThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("LateralShockRateDrillStringThreshold#01", Verbs.Enum.HasValue, "LateralShockRateDrillStringThreshold")]
        [SemanticFact("LateralShockRateDrillStringThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, BasePhysicalQuantity.QuantityEnum.ShockRate)]
        [SemanticFact("DrillString", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DrillString#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_DrillString#01", Verbs.Enum.IsAMechanicalRepresentationFor, "DrillString")]
        [SemanticFact("LateralShockRateDrillStringThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_DrillString#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "LateralShockRateDrillStringThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "LateralShockRateDrillStringThreshold#01")]
        public ScalarDrillingProperty LateralShockRateDrillStringThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("PeakToPeakAxialOscillationsBHAThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold#01", Verbs.Enum.HasValue, "PeakToPeakAxialOscillationsBHAThreshold")]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.AxialVelocityDrilling)]
        [SemanticFact("AxialMotion", Nouns.Enum.AxialMotionType)]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold#01", Verbs.Enum.IsForMotionType, "AxialMotion")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("PeakToPeakAxialOscillationsBHAThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "PeakToPeakAxialOscillationsBHAThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "PeakToPeakAxialOscillationsBHAThreshold#01")]
        public ScalarDrillingProperty PeakToPeakAxialOscillationsThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("PeakToPeakTorsionalOscillationsBHAThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold#01", Verbs.Enum.HasValue, "PeakToPeakTorsionalOscillationsBHAThreshold")]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, BasePhysicalQuantity.QuantityEnum.RotationalFrequency)]
        [SemanticFact("TorsionalMotion", Nouns.Enum.TorsionalMotionType)]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold#01", Verbs.Enum.IsForMotionType, "TorsionalMotion")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("PeakToPeakTorsionalOscillationsBHAThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "PeakToPeakTorsionalOscillationsBHAThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "PeakToPeakTorsionalOscillationsBHAThreshold#01")]
        public ScalarDrillingProperty PeakToPeakTorsionalOscillationsThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("AxialStickDurationBHAThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("AxialStickDurationBHAThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("AxialStickDurationBHAThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("AxialStickDurationBHAThreshold#01", Verbs.Enum.HasValue, "AxialStickDurationBHAThreshold")]
        [SemanticFact("AxialStickDurationBHAThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.StickDurationDrilling)]
        [SemanticFact("AxialMotion", Nouns.Enum.AxialMotionType)]
        [SemanticFact("AxialStickDurationBHAThreshold#01", Verbs.Enum.IsForMotionType, "AxialMotion")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("AxialStickDurationBHAThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "AxialStickDurationBHAThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "AxialStickDurationBHAThreshold#01")]
        public ScalarDrillingProperty AxialStickDurationThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("TorsionalStickDurationBHAThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("TorsionalStickDurationBHAThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("TorsionalStickDurationBHAThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("TorsionalStickDurationBHAThreshold#01", Verbs.Enum.HasValue, "TorsionalStickDurationBHAThreshold")]
        [SemanticFact("TorsionalStickDurationBHAThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.StickDurationDrilling)]
        [SemanticFact("TorsionalMotion", Nouns.Enum.TorsionalMotionType)]
        [SemanticFact("TorsionalStickDurationBHAThreshold#01", Verbs.Enum.IsForMotionType, "TorsionalMotion")]
        [SemanticFact("BHA", Nouns.Enum.BottomholeAssembly)]
        [SemanticFact("logical_BHA#01", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_BHA#01", Verbs.Enum.IsAMechanicalRepresentationFor, "BHA")]
        [SemanticFact("TorsionalStickDurationBHAThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_BHA#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "TorsionalStickDurationBHAThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "TorsionalStickDurationBHAThreshold#01")]
        public ScalarDrillingProperty TorsionalStickDurationThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("FlowPipeToAnnulusThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("FlowPipeToAnnulusThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("FlowPipeToAnnulusThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("FlowPipeToAnnulusThreshold#01", Verbs.Enum.HasValue, "FlowPipeToAnnulusThreshold")]
        [SemanticFact("FlowPipeToAnnulusThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.VolumetricFlowrateDrilling)]
        [SemanticFact("PipeWashout#01", Nouns.Enum.PipeWashoutLocation)]
        [SemanticFact("FlowPipeToAnnulusThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "PipeWashout#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "FlowPipeToAnnulusThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "FlowPipeToAnnulusThreshold#01")]
        public ScalarDrillingProperty FlowPipeToAnnulusThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("AtDrillHeightThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("AtDrillHeightThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("AtDrillHeightThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("AtDrillHeightThreshold#01", Verbs.Enum.HasValue, "AtDrillHeightThreshold")]
        [SemanticFact("AtDrillHeightThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.HeightDrilling)]
        [SemanticFact("MinDrillHeight#01", Nouns.Enum.MinDrillHeightVerticalLocation)]
        [SemanticFact("AtDrillHeightThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "MinDrillHeight#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "AtDrillHeightThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "AtDrillHeightThreshold#01")]
        public ScalarDrillingProperty AtDrillHeightThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("AtStickUpHeightThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("AtStickUpHeightThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("AtStickUpHeightThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("AtStickUpHeightThreshold#01", Verbs.Enum.HasValue, "AtStickUpHeightThreshold")]
        [SemanticFact("AtStickUpHeightThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.HeightDrilling)]
        [SemanticFact("StickUpHeight#01", Nouns.Enum.StickUpHeightVerticalLocation)]
        [SemanticFact("AtStickUpHeightThreshold#01", Verbs.Enum.IsPhysicallyLocatedAt, "StickUpHeight#01")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "AtStickUpHeightThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "AtStickUpHeightThreshold#01")]
        public ScalarDrillingProperty AtStickUpHeightThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("TorqueGradientThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("TorqueGradientThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("TorqueGradientThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("TorqueGradientThreshold#01", Verbs.Enum.HasValue, "TorqueGradientThreshold")]
        [SemanticFact("TorqueGradientThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.TorqueGradientPerLengthDrilling)]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.MechanicalLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAMechanicalRepresentationFor, "DS")]
        [SemanticFact("TorqueGradientThreshold#01", Verbs.Enum.IsMechanicallyLocatedAt, "logical_DS")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "TorqueGradientThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "TorqueGradientThreshold#01")]
        public ScalarDrillingProperty TorqueGradientThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("AnnulusPressureGradientThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("AnnulusPressureGradientThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("AnnulusPressureGradientThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("AnnulusPressureGradientThreshold#01", Verbs.Enum.HasValue, "AnnulusPressureGradientThreshold")]
        [SemanticFact("AnnulusPressureGradientThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureGradientPerLengthDrilling)]
        [SemanticFact("DrillPipesAnnular", Nouns.Enum.DrillPipesAnnular)]
        [SemanticFact("logical_DrillPipesAnnular", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("logical_DrillPipesAnnular", Verbs.Enum.IsAHydraulicRepresentationFor, "DrillPipesAnnular")]
        [SemanticFact("AnnulusPressureGradientThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_DrillPipesAnnular")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "AnnulusPressureGradientThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "AnnulusPressureGradientThreshold#01")]
        public ScalarDrillingProperty AnnulusPressureGradientThreshold { get; set; } = new ScalarDrillingProperty();

        [AccessToVariable(CommonProperty.VariableAccessType.Readable)]
        [Mandatory(CommonProperty.MandatoryType.General)]
        [SemanticDiracVariable("StringPressureGradientThreshold")]
        [SemanticExclusiveOr(1, 2)]
        [SemanticFact("StringPressureGradientThreshold", Nouns.Enum.DrillingSignal)]
        [SemanticFact("StringPressureGradientThreshold#01", Nouns.Enum.Limit)]
        [SemanticFact("StringPressureGradientThreshold#01", Verbs.Enum.HasValue, "StringPressureGradientThreshold")]
        [SemanticFact("StringPressureGradientThreshold#01", Verbs.Enum.IsOfMeasurableQuantity, DrillingPhysicalQuantity.QuantityEnum.PressureGradientPerLengthDrilling)]
        [SemanticFact("DS", Nouns.Enum.DrillString)]
        [SemanticFact("logical_DS", Nouns.Enum.HydraulicLogicalElement)]
        [SemanticFact("logical_DS", Verbs.Enum.IsAHydraulicRepresentationFor, "DS")]
        [SemanticFact("StringPressureGradientThreshold#01", Verbs.Enum.IsHydraulicallyLocatedAt, "logical_DS")]
        [SemanticFact("MovingAverage", Nouns.Enum.MovingAverage)]
        [SemanticFact("signal#01", Nouns.Enum.DrillingDataPoint)]
        [SemanticFact("signal#01", Verbs.Enum.IsTransformationOutput, "MovingAverage")]
        [OptionalFact(1, "StringPressureGradientThreshold#01", Verbs.Enum.IsToBeComparedWith, "signal#01")]
        [OptionalFact(2, "signal#01", Verbs.Enum.IsToBeComparedWith, "StringPressureGradientThreshold#01")]
        public ScalarDrillingProperty StringPressureGradientThreshold { get; set; } = new ScalarDrillingProperty();

        public Thresholds() : base() { }

        public Thresholds(Thresholds src) :base()
        {
            if (src != null)
            {
                PropertyInfo[] sourceProperties = src.GetType().GetProperties();
                Type destinationType = this.GetType();

                foreach (PropertyInfo property in sourceProperties)
                {
                    PropertyInfo? destinationProperty = destinationType.GetProperty(property.Name);
                    if (destinationProperty != null && destinationProperty.CanWrite)
                    {
                        object? value = property.GetValue(src);
                        destinationProperty.SetValue(this, value);
                    }
                }
            }
        }
        public void CopyTo(Thresholds destination)
        {
            if (destination != null)
            {
                PropertyInfo[] sourceProperties = this.GetType().GetProperties();
                Type destinationType = destination.GetType();

                foreach (PropertyInfo property in sourceProperties)
                {
                    PropertyInfo? destinationProperty = destinationType.GetProperty(property.Name);
                    if (destinationProperty != null && destinationProperty.CanWrite)
                    {
                        object? value = property.GetValue(this);
                        destinationProperty.SetValue(destination, value);
                    }
                }
            }
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
    }
}
