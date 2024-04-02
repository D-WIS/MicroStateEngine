
using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace DWIS.MicroState.Model
{
    public class Thresholds
    {
        /// <summary>
        /// the timestamp in UTC when the thresholds have been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }
        public ScalarDrillingProperty StableAxialVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableRotationalVelocityTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableTensionTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty AtmosphericPressureThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StablePressureTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableTorqueTopOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowAnnulusOutletThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableBottomOfStringRockForceThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableRotationalVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableAxialVelocityBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowBottomOfStringThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowHoleOpenerThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty MinimumTensionTopOfString { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty MinimumPressureFloatValve { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowBoosterPumpThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowBackPressurePumpThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty MinimumDifferentialPressureRCDSealingThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty MinimumDifferentialPressureSealBalanceThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowFillPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableFlowLiftPumpDGDThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty StableCuttingsFlowThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty HardStringerThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty ChangeOfFormationUCSSlopeThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty ForceOnLedgeThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty ForceOnCuttingsBedThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty ForceDifferentialStickingThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty FluidFlowFormationThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty FlowCavingsFromFormationThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty WhirlRateThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty FlowPipeToAnnulusThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty AtDrillHeightThreshold { get; set; } = new ScalarDrillingProperty();
        public ScalarDrillingProperty AtStickUpHeightThreshold { get; set; } = new ScalarDrillingProperty();

        public void CopyTo(Thresholds destination)
        {
            if (destination != null)
            {
                destination.TimeStampUTC = TimeStampUTC;
                destination.StableAxialVelocityTopOfStringThreshold = StableAxialVelocityTopOfStringThreshold;
                destination.StableRotationalVelocityTopOfStringThreshold = StableRotationalVelocityTopOfStringThreshold;
                destination.StableFlowTopOfStringThreshold = StableFlowTopOfStringThreshold;
                destination.StableTensionTopOfStringThreshold = StableTensionTopOfStringThreshold;
                destination.AtmosphericPressureThreshold = AtmosphericPressureThreshold;
                destination.StablePressureTopOfStringThreshold = StablePressureTopOfStringThreshold;
                destination.StableTorqueTopOfStringThreshold = StableTorqueTopOfStringThreshold;
                destination.StableFlowAnnulusOutletThreshold = StableFlowAnnulusOutletThreshold;
                destination.StableBottomOfStringRockForceThreshold = StableBottomOfStringRockForceThreshold;
                destination.StableRotationalVelocityBottomOfStringThreshold = StableRotationalVelocityBottomOfStringThreshold;
                destination.StableAxialVelocityBottomOfStringThreshold = StableAxialVelocityBottomOfStringThreshold;
                destination.StableFlowBottomOfStringThreshold = StableFlowBottomOfStringThreshold;
                destination.StableFlowHoleOpenerThreshold = StableFlowHoleOpenerThreshold;
                destination.MinimumTensionTopOfString = MinimumTensionTopOfString;
                destination.MinimumPressureFloatValve = MinimumPressureFloatValve;
                destination.StableFlowBoosterPumpThreshold = StableFlowBoosterPumpThreshold;
                destination.StableFlowBackPressurePumpThreshold = StableFlowBackPressurePumpThreshold;
                destination.MinimumDifferentialPressureRCDSealingThreshold = MinimumDifferentialPressureRCDSealingThreshold;
                destination.MinimumDifferentialPressureSealBalanceThreshold = MinimumDifferentialPressureSealBalanceThreshold;
                destination.StableFlowFillPumpDGDThreshold = StableFlowFillPumpDGDThreshold;
                destination.StableFlowLiftPumpDGDThreshold = StableFlowLiftPumpDGDThreshold;
                destination.StableCuttingsFlowThreshold = StableCuttingsFlowThreshold;
                destination.HardStringerThreshold = HardStringerThreshold;
                destination.ChangeOfFormationUCSSlopeThreshold = ChangeOfFormationUCSSlopeThreshold;
                destination.ForceOnLedgeThreshold = ForceOnLedgeThreshold;
                destination.ForceOnCuttingsBedThreshold = ForceOnCuttingsBedThreshold;
                destination.ForceDifferentialStickingThreshold = ForceDifferentialStickingThreshold;
                destination.FluidFlowFormationThreshold = FluidFlowFormationThreshold;
                destination.FlowCavingsFromFormationThreshold = FlowCavingsFromFormationThreshold;
                destination.WhirlRateThreshold = WhirlRateThreshold;
                destination.FlowPipeToAnnulusThreshold = FlowPipeToAnnulusThreshold;
                destination.AtDrillHeightThreshold = AtDrillHeightThreshold;
                destination.AtStickUpHeightThreshold = AtStickUpHeightThreshold;
            }
        }
    }
}
