using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Resources;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolHoldStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolHoldStep(double temperature, string temperatureUnits = TemperatureUnit.Default)
        {
            this.Temperature = temperature;
            this.TypeName = ThermocyclingProtocolStepType.Hold;
            this.TemperatureUnits = temperatureUnits;
        }
    }
}
