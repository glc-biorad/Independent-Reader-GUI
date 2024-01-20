using Independent_Reader_GUI.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class Cartridge
    {
        public string Name = string.Empty;
        public string PartitionType = string.Empty;
        public int NumberofSampleChambers = int.MinValue;
        public int NumberofAssayChambers = int.MinValue;
        public double Length = double.MinValue;
        public string LengthUnits = SpatialUnits.Millimeter;
        public double Width = double.MinValue;
        public string WidthUnits = SpatialUnits.Millimeter;
        public double Height = double.MinValue;
        public string HeightUnits = SpatialUnits.Millimeter;
    }
}
