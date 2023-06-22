using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.MQTTSignalReader
{
    public class Payload
    {
        public Info info { get; set; }
        public List<string> variables { get; set; }
        public List<List<object>> data { get; set; }
    }

}
