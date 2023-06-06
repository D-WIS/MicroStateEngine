namespace DWIS.MicroState.Model
{
    public struct MicroStates
    {
        /// <summary>
        /// an enumeration of the microstate indices
        /// </summary>
        public enum MicroStateIndex
        {
            AxialVelocityTopOfString = 0,
            StableAxialVelocityTopOfString,
            RotationalVelocityTopOfString,
            StableRotationalVelocityTopOfString,
            FlowAtTopOfString,
            StableFlowAtTopOfString,
            SlipState,
            StableTensionTopOfString,
            PressureTopOfString,
            StablePressureTopOfString,
            TorqueTopOfString,
            StableTorqueTopOfString,
            FlowAtAnnulusOutlet,
            StableFlowAtAnnulusOutlet,
            CuttingsReturnAtAnnulusOutlet,
            OnBottomBottomOfString,
            StableBottomOfStringRockForce,
            OnBottomHoleOpener,
            RotationalVelocityBottomOfString,
            StableRotationalVelocityBottomOfString,
            Drilling,
            HoleOpening,
            AxialVelocityBottomOfString,
            StableAxialVelocityBottomOfString,
            FlowBottomOfString,
            StableFlowBottomOfString,
            FlowHoleOpener,
            StableFlowHoleOpener,
            LedgeKeySeat,
            CuttingsBed,
            DifferentialSticking,
            TwistOffBackOff,
            WellIntegrity,
            FormationFluidAtAnnulusOutlet,
            FormationCollapse,
            CavingsAtAnnulusOutlet,
            PipeWashout,
            WhirlBottomOfString,
            WhirlHoleOpener,
            FloatSub,
            UnderReamer,
            CirculationSub,
            PortedFloat,
            Whipstock,
            Plug,
            Liner,
            BoosterPumping,
            StableBoosterPumping,
            BackPressurePumping,
            StableBackPressurePumping,
            MPDChokeOpening,
            RCDSealing,
            IsolationSeal,
            IsolationSealPressureBalance,
            BearingAssemblyLatched,
            ScreenMPDChokePlugged,
            FlowPathStable,
            FillPumpDGD,
            LiftPumpDGD,
            StableFillPumpDGD,
            StableLiftPumpDGD
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

    }
}