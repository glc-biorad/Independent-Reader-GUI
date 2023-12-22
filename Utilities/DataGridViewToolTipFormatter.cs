using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class DataGridViewToolTipFormatter
    {
        /// <summary>
        /// Obtain the tool tip text for a given cell value to format a data grid
        /// view on form initialization.
        /// </summary>
        /// <param name="cellValue">Cell value text in the data grid view.</param>
        /// <returns>The tool tip text as a string.</returns>
        public string GetCellValueToolTipText(string cellValue)
        {
            if (cellValue == "Cartridge")
            {
                return "Cartridge to be used for dPCR.";
            }
            else if (cellValue == "Clamp Position (\u03BCs)")
            {
                return "Position of Clamp during run.";
            }
            else if (cellValue == "Tray Position (\u03BCs)")
            {
                return "Position of Tray during run.";
            }
            else if (cellValue == "Glass Offset (mm)")
            {
                return "Distance between glass and heater surface when clamp is homed.";
            }
            else if (cellValue == "Elastomer")
            {
                return "dPCR compatible elastomer layer between cartridge and heater surface if applicable.";
            }
            else if (cellValue == "Bergquist")
            {
                return "Thermally conductive layer if applicable.";
            }
            else if (cellValue == "Contact Surface Area (mm x mm)")
            {
                return "Surface area interface between dPCR system and heater surface (used for estimating pressure on dPCR system)";
            }
            else if (cellValue == "Pressure (KPa)")
            {
                return "Pressure on dPCR system.";
            }
            return cellValue;
        }
    }
}
