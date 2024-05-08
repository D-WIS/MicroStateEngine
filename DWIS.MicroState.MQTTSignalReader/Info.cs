using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.MQTTSignalReader
{
    public class Info
    {
        public int timestamp { get; set; }
        public int sequenceNo { get; set; }
        public string well { get; set; }
        public string run { get; set; }
        public string record { get; set; }
        public string descriptor { get; set; }
        public bool isWellBased { get; set; }
        public int numRows { get; set; }
    }

}
