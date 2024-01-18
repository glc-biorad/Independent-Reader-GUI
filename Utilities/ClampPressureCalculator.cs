using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class ClampPressureCalculator
    {
        public double ComputePressureKPa(double k, double glassOffset, Cartridge cartridge, Elastomer elastomer, Bergquist bergquist, double clampPositionMM, double A)
        {
            // Compute the difference between the Glass Offset and the Thickness of the dPCR system (Cartridge + Elastomer + Bergquist) 
            // This will tell us when the Pressure is 0 and non-zero.
            double cartridgeHeight = 0;
            double elastomerThickness = 0;
            double bergquistThickness = 0;
            if (double.TryParse(cartridge.Height.ToString(), out _))
            {
                cartridgeHeight = cartridge.Height;
            }
            if (double.TryParse(elastomer.Thickness.ToString(), out _))
            {
                elastomerThickness = elastomer.Thickness;
            }
            if (double.TryParse(bergquist.Thickness.ToString(), out _))
            {
                bergquistThickness = bergquist.Thickness;
            }
            double threshold = glassOffset - (cartridgeHeight + elastomerThickness + bergquistThickness);
            // Determine the pressure
            if (clampPositionMM <= threshold)
            {
                return 0.0;
            }
            else
            {
                double x = (clampPositionMM - threshold);
                double pressurePSI = k * (x) / A;
                double pressureKPa = pressurePSI * 6.89476;
                return pressureKPa;
            }
        }
    }
}
