using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Elastomer
    {
        public string Name { get; set; } = string.Empty;
        public double ThermalCoefficient { get; set; } = double.NaN;
        public string ThermalCoefficientUnits { get; set; } = "W/m K";
        public double ShoreHardness {  get; set; } = double.NaN;
        public double Thickness {  get; set; } = double.NaN;
        public string ThicknessUnits { get; set; } = "mm";
        public string Color { get; set; } = "Black";
        public string Film { get; set; } = string.Empty;
        public string Mold { get; set; } = string.Empty;
        public string PreparedBy { get; set; } = "FF";
    }
}
