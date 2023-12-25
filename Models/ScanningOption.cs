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
        public double GlassOffset = double.NaN;
        public int X0 = int.MinValue;
        public int Y0 = int.MinValue;
        public int Z0 = int.MinValue;
        public int FOVdX = int.MinValue;
        public int dY = int.MinValue;
        public double RotationalOffset = double.NaN;
        public string Units = SpatialUnits.Microsteps;
    }
}
