using Independent_Reader_GUI.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocolStep
    {
        public int Index { get; set; } = int.MinValue;
        public string TypeName { get; set; } = ThermocyclingProtocolStepType.Unknown;
        public double Temperature { get; set; } = double.NaN;
        public double Time { get; set; } = double.NaN;
        public int StepNumber { get; set; } = int.MinValue;
        public int CycleCount { get; set; } = int.MinValue;
        public string TemperatureUnits { get; set; } = TemperatureUnit.Default;
        public string TimeUnits { get; set; } = TimeUnit.Default;
    }
}
