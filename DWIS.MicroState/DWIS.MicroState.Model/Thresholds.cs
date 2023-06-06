using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
