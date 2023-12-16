using Independent_Reader_GUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Cartridge
    {
        private string Name = string.Empty;
        private string PartitionType = string.Empty;
        private int NumberofSampleChambers = int.MinValue;
        private int NumberofAssayChambers = int.MinValue;
        private double Length = double.MinValue;
        private string LengthUnits = SpatialUnits.Millimeter;
        private double Width = double.MinValue;
        private string WidthUnits = SpatialUnits.Millimeter;
        private double Height = double.MinValue;
        private string HeightUnits = SpatialUnits.Millimeter;
    }
}
