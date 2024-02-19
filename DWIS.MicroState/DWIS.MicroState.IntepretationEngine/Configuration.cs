using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.IntepretationEngine
{
    public class Configuration
    {
        public string MQTTServerName { get; set; } = "localhost";
        public int MQTTServerPort { get; set; } = 707;
        public TimeSpan LoopDuration { get; set; } = TimeSpan.FromSeconds(1.0);
        public string? OPCUAURL { get; set; } = "opc.tcp://localhost:48030";

    }
}
