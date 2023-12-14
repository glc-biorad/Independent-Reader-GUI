using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: This class should become obsolute and removed, this was the ThermocyclingProtocolStep class but now is the
// ThermocyclingProtocolStepOld class, I want to replace it with something more general.

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolStepOld
    {
        private double temperature;
        private double time;
        private string typeName;

        public ThermocyclingProtocolStepOld(double stepTemperature, double stepTime, string stepTypeName)
        {
            temperature = stepTemperature;
            time = stepTime;
            typeName = stepTypeName;
        }

        public double Temperature
        {
            get { return temperature; }
        }

        public double Time
        {
            get { return time; }
        }

        public string TypeName
        {
            get { return typeName; }
        }
    }
}
