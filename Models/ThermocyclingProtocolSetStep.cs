using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolSetStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolSetStep(double temperature, double time)
        {
            this.Temperature = temperature;
            this.Time = time;
            this.TypeName = ThermocyclingProtocolStepType.Set;
        }
    }
}
