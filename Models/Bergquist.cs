using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Bergquist
    {
        public string? Name { get; set; } = null;
        public double? ThermalCoefficient { get; set; } = null;
        public string ThermalCoefficientUnits { get; set; } = string.Empty;
        public double Thickness { get; set; } = double.MinValue;
        public string ThicknessUnits { get; set; } = string.Empty;
        public int? ShoreHardness {  get; set; } = null;
    }
}
