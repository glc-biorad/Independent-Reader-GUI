using Independent_Reader_GUI.Models.Hardware.TEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Scanning
{
    internal class ScanParameterData
    {
        public DataGridViewComboBoxCell Heater = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell Elastomer = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell Bergquist = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell Cartridge = new DataGridViewComboBoxCell();
        public double GlassOffset = double.NaN;
        public int? X0 = null;
        public int? Y0 = null;
        public int? Z0 = null;
        public int? FOVdX = null;
        public int? SampledX = null;
        public int? dY = null;
        public int? RotationalOffset = null;

        public ScanParameterData()
        {
            HeaterOptions heaterOptions = new HeaterOptions();
            Heater = heaterOptions.Options;
        }
    }
}
