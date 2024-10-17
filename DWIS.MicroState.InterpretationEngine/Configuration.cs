using MathNet.Numerics.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.InterpretationEngine
{
    public class Configuration
    {
        public TimeSpan LoopDuration { get; set; } = TimeSpan.FromSeconds(1.0);
        public string? OPCUAURL { get; set; } = "opc.tcp://localhost:48030";

        public double DefaultProbability { get; set; } = 0.1;

        public double DefaultStandardDeviation { get; set; } = 0.1;

        public int CircularBufferSize { get; set; } = 300;

        public TimeSpan CalibrationMinTimeWindow { get; set; } = TimeSpan.FromSeconds(120);

        public double CalibrationTimeWindowFactor { get; set; } = 0.5;

        public double CalibrationConvergenceTolerance { get; set; } = 1e-6;

        public int CalibrationMaxNumberOfIterations { get; set; } = 1000;

        public bool GenerateRandomValues { get; set; } = false;

    }
}
