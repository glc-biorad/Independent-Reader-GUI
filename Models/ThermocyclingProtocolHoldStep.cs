using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolHoldStep : ThermocyclingProtocolStep
    {
        public ThermocyclingProtocolHoldStep(double temperature)
        {
            this.Temperature = temperature;
            this.TypeName = ThermocyclingProtocolStepType.Hold;
        }
    }
}
