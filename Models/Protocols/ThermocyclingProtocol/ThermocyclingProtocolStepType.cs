using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocolStepType
    {
        public const string Hold = "Hold"; // hold a temperature indefinitely (nothing can be placed after in a protocol)
        public const string GoTo = "GoTo"; // goto a different step to repeat
        public const string Set = "Set"; // sets the temperature
        public const string RampUp = "Ramp Up"; // increase in temperature
        public const string RampDown = "Ramp Down"; // decrease in temperature
        public const string Unknown = "Unknown";
    }
}
