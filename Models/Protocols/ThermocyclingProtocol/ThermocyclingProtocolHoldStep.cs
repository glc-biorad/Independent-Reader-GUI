using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Models.Units;

namespace Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol
{
    internal class ThermocyclingProtocolHoldStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolHoldStep(double temperature, string temperatureUnits = TemperatureUnit.Default)
        {
            Temperature = temperature;
            TypeName = ThermocyclingProtocolStepType.Hold;
            TemperatureUnits = temperatureUnits;
        }
    }
}
