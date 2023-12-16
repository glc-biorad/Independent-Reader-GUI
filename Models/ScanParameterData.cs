using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ScanParameterData
    {
        public DataGridViewComboBoxCell Heater = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell PartitionType = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell Cartridge = new DataGridViewComboBoxCell();
        public int? X0 = null;
        public int? Y0 = null;
        public int? Z0 = null;
        public int? FOVdX = null;
        public int? dY = null;
        public int? RotationalOffset = null;

        public ScanParameterData()
        {
            Heater.Items.AddRange(new object[]
            {
                "A",
                "B",
                "C",
                "D"
            });
            Heater.Value = Heater.Items[0];
            PartitionType.Items.AddRange(new object[]
            {
                "Microwells",
                "Standard Droplets",
                "Pico Droplets"
            });
            PartitionType.Value = PartitionType.Items[0];
            Cartridge.Items.AddRange(new object[]
            {
                "M2M",
                "Vantiva",
                "Parallel",
                "BEAMR"
            });
            Cartridge.Value = Cartridge.Items[0];
        }
    }
}
