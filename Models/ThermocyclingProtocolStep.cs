using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ThermocyclingProtocolStep
    {
        private double temperature;
        private double time;
        private string typeName;

        public ThermocyclingProtocolStep(double stepTemperature, double stepTime, string stepTypeName)
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
