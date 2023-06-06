using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DWIS.RigOS.Capabilities.ModelShared;

namespace DWIS.MicroState.Model
{
    public class Signals
    {
        /// <summary>
        /// the timestamp in UTC when the signals have been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }
        /// <summary>
        /// the maximum interval before the signal binding is refreshed, i.e. that new SparQL queries are sent to the DDHub server
        /// </summary>
        public TimeSpan MaximumBindingRefreshInterval { get; set; }

        public ReadableReferenceOfScalarValue AxialVelocityTopOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationAxialVelocityTopOfString { get; set; }
        public ReadableReferenceOfScalarValue RotationalVelocityTopOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationRotationalVelocityTopOfString { get; set; }
        public ReadableReferenceOfScalarValue FlowTopOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowTopOfString { get; set; }
        public ReadableReferenceOfScalarValue TensionTopOfString { get; set; }
        public ReadableReferenceOfScalarValue ForceBottomTopDrive { get; set; }
        public ReadableReferenceOfScalarValue ForceElevator { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationTensionTopOfString { get; set; }
        public ReadableReferenceOfScalarValue PressureTopOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationPressureTopOfString { get; set; }
        public ReadableReferenceOfScalarValue TorqueTopOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationTorqueTopOfString { get; set; }
        public ReadableReferenceOfScalarValue FlowAnnulusOutlet { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowAnnulusOutlet { get; set; }
        public ReadableReferenceOfScalarValue FlowCuttingsAnnulusOutlet { get; set; }
        public ReadableReferenceOfScalarValue ForceBottomOfStringOnRock { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationForceBottomOfStringOnRock { get; set; }
        public ReadableReferenceOfScalarValue ForceHoleOpenerOnRock { get; set; }
        public ReadableReferenceOfScalarValue RotationaVelocityBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationRotationalVelocityBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue FlowCuttingsBottomHole { get; set; }
        public ReadableReferenceOfScalarValue FlowCuttingsTopOfRateHole { get; set; }
        public ReadableReferenceOfScalarValue AxialVelocityBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationAxialVelocityBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue FlowBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue StableFlowBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue FlowHoleOpener { get; set; }  
        public ReadableReferenceOfScalarValue StableFlowHoleOpener { get; set; }
        public ReadableReferenceOfScalarValue ForceOnLedge { get; set; }
        public ReadableReferenceOfScalarValue ForceOnCuttingsBed { get; set; }
        public ReadableReferenceOfScalarValue ForceDifferentialSticking { get; set; }
        public ReadableReferenceOfScalarValue FlowFluidFromOrToFormation { get; set; }
        public ReadableReferenceOfScalarValue FlowFormationFluidAnnulusOutlet { get; set; }
        public ReadableReferenceOfScalarValue FlowCavingsFromFormation { get; set; }
        public ReadableReferenceOfScalarValue FlowCavingsAnnulusOutlet { get; set; }
        public ReadableReferenceOfScalarValue FlowPipeToAnnulus { get; set; }
        public ReadableReferenceOfScalarValue WhirlRateBottomOfString { get; set; }
        public ReadableReferenceOfScalarValue WhirlRateHoleOpener { get; set; }
        public ReadableReferenceOfScalarValue DifferentialPressureFloatValve { get; set; }
        public ReadableReferenceOfBooleanValue UnderReamerOpen { get; set; }
        public ReadableReferenceOfBooleanValue CirculationSubOpen { get; set; }
        public ReadableReferenceOfBooleanValue PortedFloatOpen { get; set; }
        public ReadableReferenceOfBooleanValue WhipstockAttached { get; set; }
        public ReadableReferenceOfBooleanValue PlugAttached {get; set; }
        public ReadableReferenceOfBooleanValue LinerAttached { get; set; }  
        public ReadableReferenceOfScalarValue FlowBoosterPump { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowBoosterPump { get; set; }
        public ReadableReferenceOfScalarValue FlowBackPressurePump { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowBackPressurePump { get; set; }
        public ReadableReferenceOfScalarValue OpeningMPDChoke { get; set; }
        public ReadableReferenceOfScalarValue DifferentialPressureRCD { get; set; }
        public ReadableReferenceOfBooleanValue IsolationSealActivated { get; set; }
        public ReadableReferenceOfScalarValue DifferentialPressureIsolationSeal { get; set; }
        public ReadableReferenceOfBooleanValue BearingAssemblyLatched { get; set; }
        public ReadableReferenceOfBooleanValue ScreenMPDChokePlugged { get; set; }
        public ReadableReferenceOfBooleanValue MainFlowPathMPDEstablished { get; set; }
        public ReadableReferenceOfBooleanValue AlternateFlowPathMPDEstablished { get; set; }    
        public ReadableReferenceOfScalarValue FlowFillPumpDGD { get; set; }
        public ReadableReferenceOfScalarValue FlowLiftPumpDGD { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowFillPumpDGD { get; set; }
        public ReadableReferenceOfScalarValue StandardDeviationFlowLiftPumpDGD { get; set; }



    }
}
