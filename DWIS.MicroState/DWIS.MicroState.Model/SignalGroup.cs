
namespace DWIS.MicroState.Model
{
    public class SignalGroup
    {
        public double? AxialVelocityTopOfString { get; set; }
        public double? StandardDeviationAxialVelocityTopOfString { get; set; }
        public double? RotationalVelocityTopOfString { get; set; }
        public double? StandardDeviationRotationalVelocityTopOfString { get; set; }
        public double? FlowTopOfString { get; set; }
        public double? StandardDeviationFlowTopOfString { get; set; }
        public double? TensionTopOfString { get; set; }
        public double? ForceBottomTopDrive { get; set; }
        public double? ForceElevator { get; set; }
        public double? StandardDeviationTensionTopOfString { get; set; }
        public double? PressureTopOfString { get; set; }
        public double? StandardDeviationPressureTopOfString { get; set; }
        public double? TorqueTopOfString { get; set; }
        public double? StandardDeviationTorqueTopOfString { get; set; }
        public double? FlowAnnulusOutlet { get; set; }
        public double? StandardDeviationFlowAnnulusOutlet { get; set; }
        public double? FlowCuttingsAnnulusOutlet { get; set; }
        public double? ForceBottomOfStringOnRock { get; set; }
        public double? StandardDeviationForceBottomOfStringOnRock { get; set; }
        public double? ForceHoleOpenerOnRock { get; set; }
        public double? RotationaVelocityBottomOfString { get; set; }
        public double? StandardDeviationRotationalVelocityBottomOfString { get; set; }
        public double? FlowCuttingsBottomHole { get; set; }
        public double? FlowCuttingsTopOfRateHole { get; set; }
        public double? AxialVelocityBottomOfString { get; set; }
        public double? StandardDeviationAxialVelocityBottomOfString { get; set; }
        public double? FlowBottomOfString { get; set; }
        public double? StableFlowBottomOfString { get; set; }
        public double? FlowHoleOpener { get; set; }
        public double? StableFlowHoleOpener { get; set; }
        public double? ForceOnLedge { get; set; }
        public double? ForceOnCuttingsBed { get; set; }
        public double? ForceDifferentialSticking { get; set; }
        public double? FlowFluidFromOrToFormation { get; set; }
        public double? FlowFormationFluidAnnulusOutlet { get; set; }
        public double? FlowCavingsFromFormation { get; set; }
        public double? FlowCavingsAnnulusOutlet { get; set; }
        public double? FlowPipeToAnnulus { get; set; }
        public double? WhirlRateBottomOfString { get; set; }
        public double? WhirlRateHoleOpener { get; set; }
        public double? DifferentialPressureFloatValve { get; set; }
        public bool? UnderReamerOpen { get; set; }
        public bool? CirculationSubOpen { get; set; }
        public bool? PortedFloatOpen { get; set; }
        public bool? WhipstockAttached { get; set; }
        public bool? PlugAttached { get; set; }
        public bool? LinerAttached { get; set; }
        public double? FlowBoosterPump { get; set; }
        public double? StandardDeviationFlowBoosterPump { get; set; }
        public double? FlowBackPressurePump { get; set; }
        public double? StandardDeviationFlowBackPressurePump { get; set; }
        public double? OpeningMPDChoke { get; set; }
        public double? DifferentialPressureRCD { get; set; }
        public bool? IsolationSealActivated { get; set; }
        public double? DifferentialPressureIsolationSeal { get; set; }
        public bool? BearingAssemblyLatched { get; set; }
        public bool? ScreenMPDChokePlugged { get; set; }
        public bool? MainFlowPathMPDEstablished { get; set; }
        public bool? AlternateFlowPathMPDEstablished { get; set; }
        public double? FlowFillPumpDGD { get; set; }
        public double? FlowLiftPumpDGD { get; set; }
        public double? StandardDeviationFlowFillPumpDGD { get; set; }
        public double? StandardDeviationFlowLiftPumpDGD { get; set; }
        public double? UCS { get; set; }
        public double? UCSSlope { get; set; }
        public double? MinimumTensionForTwistOffDetection { get; set; }
        public double? ToolJoint1Height { get; set; }
        public double? ToolJoint2Height { get; set; }
        public double? ToolJoint3Height { get; set; }
        public double? ToolJoint4Height { get; set; }
        public double? MinDrillHeight { get; set; }
        public double? StickUpHeight { get; set; }


        public SignalGroup() : base()
        {

        }

        public SignalGroup(SignalGroup src) : base()
        {
            if (src != null)
            {
                AxialVelocityTopOfString = src.AxialVelocityTopOfString;
                StandardDeviationAxialVelocityTopOfString = src.StandardDeviationAxialVelocityTopOfString;
                RotationalVelocityTopOfString = src.RotationalVelocityTopOfString;
                StandardDeviationRotationalVelocityTopOfString = src.StandardDeviationRotationalVelocityTopOfString;
                FlowTopOfString = src.FlowTopOfString;
                StandardDeviationFlowTopOfString = src.StandardDeviationFlowTopOfString;
                TensionTopOfString = src.TensionTopOfString;
                ForceBottomTopDrive = src.ForceBottomTopDrive;
                ForceElevator = src.ForceElevator;
                StandardDeviationTensionTopOfString = src.StandardDeviationTensionTopOfString;
                PressureTopOfString = src.PressureTopOfString;
                StandardDeviationPressureTopOfString = src.StandardDeviationPressureTopOfString;
                TorqueTopOfString = src.TorqueTopOfString;
                StandardDeviationTorqueTopOfString = src.StandardDeviationTorqueTopOfString;
                FlowAnnulusOutlet = src.FlowAnnulusOutlet;
                StandardDeviationFlowAnnulusOutlet = src.StandardDeviationFlowAnnulusOutlet;
                FlowCuttingsAnnulusOutlet = src.FlowCuttingsAnnulusOutlet;
                ForceBottomOfStringOnRock = src.ForceBottomOfStringOnRock;
                StandardDeviationForceBottomOfStringOnRock = src.StandardDeviationForceBottomOfStringOnRock;
                ForceHoleOpenerOnRock = src.ForceHoleOpenerOnRock;
                RotationaVelocityBottomOfString = src.RotationaVelocityBottomOfString;
                StandardDeviationRotationalVelocityBottomOfString = src.StandardDeviationRotationalVelocityBottomOfString;
                FlowCuttingsBottomHole = src.FlowCuttingsBottomHole;
                FlowCuttingsTopOfRateHole = src.FlowCuttingsTopOfRateHole;
                AxialVelocityBottomOfString = src.AxialVelocityBottomOfString;
                StandardDeviationAxialVelocityBottomOfString = src.StandardDeviationAxialVelocityBottomOfString;
                FlowBottomOfString = src.FlowBottomOfString;
                StableFlowBottomOfString = src.StableFlowBottomOfString;
                FlowHoleOpener = src.FlowHoleOpener;
                StableFlowHoleOpener = src.StableFlowHoleOpener;
                ForceOnLedge = src.ForceOnLedge;
                ForceOnCuttingsBed = src.ForceOnCuttingsBed;
                ForceDifferentialSticking = src.ForceDifferentialSticking;
                FlowFluidFromOrToFormation = src.FlowFluidFromOrToFormation;
                FlowFormationFluidAnnulusOutlet = src.FlowFormationFluidAnnulusOutlet;
                FlowCavingsFromFormation = src.FlowCavingsFromFormation;
                FlowCavingsAnnulusOutlet = src.FlowCavingsAnnulusOutlet;
                FlowPipeToAnnulus = src.FlowPipeToAnnulus;
                WhirlRateBottomOfString = src.WhirlRateBottomOfString;
                WhirlRateHoleOpener = src.WhirlRateHoleOpener;
                DifferentialPressureFloatValve = src.DifferentialPressureFloatValve;
                UnderReamerOpen = src.UnderReamerOpen;
                CirculationSubOpen = src.CirculationSubOpen;
                PortedFloatOpen = src.PortedFloatOpen;
                WhipstockAttached = src.WhipstockAttached;
                PlugAttached = src.PlugAttached;
                LinerAttached = src.LinerAttached;
                FlowBoosterPump = src.FlowBoosterPump;
                StandardDeviationFlowBoosterPump = src.StandardDeviationFlowBoosterPump;
                FlowBackPressurePump = src.FlowBackPressurePump;
                StandardDeviationFlowBackPressurePump = src.StandardDeviationFlowBackPressurePump;
                OpeningMPDChoke = src.OpeningMPDChoke;
                DifferentialPressureRCD = src.DifferentialPressureRCD;
                IsolationSealActivated = src.IsolationSealActivated;
                DifferentialPressureIsolationSeal = src.DifferentialPressureIsolationSeal;
                BearingAssemblyLatched = src.BearingAssemblyLatched;
                ScreenMPDChokePlugged = src.ScreenMPDChokePlugged;
                MainFlowPathMPDEstablished = src.MainFlowPathMPDEstablished;
                AlternateFlowPathMPDEstablished = src.AlternateFlowPathMPDEstablished;
                FlowFillPumpDGD = src.FlowFillPumpDGD;
                FlowLiftPumpDGD = src.FlowLiftPumpDGD;
                StandardDeviationFlowFillPumpDGD = src.StandardDeviationFlowFillPumpDGD;
                StandardDeviationFlowLiftPumpDGD = src.StandardDeviationFlowLiftPumpDGD;
                UCS = src.UCS;
                UCSSlope = src.UCSSlope;
                MinimumTensionForTwistOffDetection = src.MinimumTensionForTwistOffDetection;
                ToolJoint1Height = src.ToolJoint1Height;
                ToolJoint2Height = src.ToolJoint2Height;
                ToolJoint3Height = src.ToolJoint3Height;
                ToolJoint4Height = src.ToolJoint4Height;
                MinDrillHeight = src.MinDrillHeight;
                StickUpHeight = src.StickUpHeight;
            }
        }
    }
}
