
namespace DWIS.MicroState.Model
{
    public class Thresholds
    {
        /// <summary>
        /// the timestamp in UTC when the thresholds have been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }
        public double StableAxialVelocityTopOfStringThreshold { get; set; }
        public double StableRotationalVelocityTopOfStringThreshold { get; set; }
        public double StableFlowTopOfStringThreshold { get; set; }
        public double StableTensionTopOfStringThreshold { get; set; }
        public double AtmosphericPressureThreshold { get; set; }
        public double StablePressureTopOfStringThreshold { get; set; }
        public double StableTorqueTopOfStringThreshold { get; set; }
        public double StableFlowAnnulusOutletThreshold { get; set; }
        public double StableBottomOfStringRockForceThreshold { get; set; }
        public double StableRotationalVelocityBottomOfStringThreshold { get; set; }
        public double StableAxialVelocityBottomOfStringThreshold { get; set; }
        public double StableFlowBottomOfStringThreshold { get; set; }
        public double StableFlowHoleOpenerThreshold { get; set; }
        public double MinimumTensionTopOfString { get; set; }
        public double MinimumPressureFloatValve { get; set; }
        public double StableFlowBoosterPumpThreshold { get; set; }
        public double StableFlowBackPressurePumpThreshold { get; set; }
        public double MinimumDifferentialPressureRCDSealingThreshold { get; set; }
        public double MinimumDifferentialPressureSealBalanceThreshold { get; set; }
        public double StableFlowFillPumpDGDThreshold { get; set; }
        public double StableFlowLiftPumpDGDThreshold { get; set; }
        public double StableCuttingsFlowThreshold { get; set; }
        public double HardStringerThreshold { get; set; }
        public double ChangeOfFormationUCSSlopeThreshold { get; set; }
        public double ForceOnLedgeThreshold { get; set; }
        public double ForceOnCuttingsBedThreshold { get; set; }
        public double ForceDifferentialStickingThreshold { get; set; }
        public double FluidFlowFormationThreshold { get; set; }
        public double FlowCavingsFromFormationThreshold { get; set; }
        public double WhirlRateThreshold { get; set; }
        public double FlowPipeToAnnulusThreshold { get; set; }
        public double AtDrillHeightThreshold { get; set; }
        public double AtStickUpHeightThreshold { get; set; }

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
