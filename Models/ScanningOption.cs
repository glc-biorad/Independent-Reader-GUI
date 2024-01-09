using Independent_Reader_GUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ScanningOption
    {
        public string Heater = string.Empty;
        public string CartridgeName = string.Empty;
        public string? ElastomerName;
        public string? BergquistName;
        public double GlassOffset = 0;
        public int X0 = 0;
        public int Y0 = 0;
        public int Z0 = 0;
        public int FOVdX = 0;
        public int SampledX = 0;
        public int dY = 0;
        public double RotationalOffset = 0;
        public string Units = SpatialUnits.Microsteps;
    }
}
