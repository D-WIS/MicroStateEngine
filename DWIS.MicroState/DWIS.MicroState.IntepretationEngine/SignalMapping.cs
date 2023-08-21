using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.IntepretationEngine
{
    internal class SignalMapping
    {
        public string MQTTTopic { get; set; }
        public int Index { get; set; }
        public object? Value { get; set; }

        public SignalMapping() { }
        public SignalMapping(string topic, int index)
        {
            MQTTTopic = topic;
            Index = index;
        }
    }
}
