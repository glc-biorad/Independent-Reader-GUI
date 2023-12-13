using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolStepType
    {
        public const string Hold = "Hold";
        public const string GoTo = "GoTo";
        public const string RampUp = "Ramp Up";
        public const string RampDown = "Ramp Down";
    }
}
