using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolStep
    {
        public string TypeName { get; set; } = ThermocyclingProtocolStepType.Unknown;
        public double Temperature { get; set; } = double.NaN;
        public double Time { get; set; } = double.NaN;
        public int StepNumber { get; set; } = int.MinValue;
        public int CycleCount { get; set; } = int.MinValue;
    }
}
