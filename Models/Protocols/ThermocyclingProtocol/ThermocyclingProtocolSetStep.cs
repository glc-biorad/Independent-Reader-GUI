using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Models.Units;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocolSetStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolSetStep(double temperature, double time, string temperatureUnits = TemperatureUnit.Default, string timeUnits = TimeUnit.Default)
        {
            Temperature = temperature;
            Time = time;
            TypeName = ThermocyclingProtocolStepType.Set;
            TemperatureUnits = temperatureUnits;
            TimeUnits = timeUnits;
        }
    }
}
