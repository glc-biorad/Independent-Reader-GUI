using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Resources;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolSetStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolSetStep(double temperature, double time, string temperatureUnits = TemperatureUnit.Default, string timeUnits = TimeUnit.Default)
        { 
            this.Temperature = temperature;
            this.Time = time;
            this.TypeName = ThermocyclingProtocolStepType.Set;
            this.TemperatureUnits = temperatureUnits;
            this.TimeUnits = timeUnits;
        }
    }
}
