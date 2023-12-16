using Independent_Reader_GUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ExperimentData
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public string Heater { get; set; } = string.Empty;
        public string PartitionType { get; set; } = string.Empty;
        public DataGridViewComboBoxCell CartridgeComboBoxCell = new DataGridViewComboBoxCell();
        public double CartridgeLength { get; set; } = 0.0;
        public double CartridgeWidth { get; set; } = 0.0;
        public double CartridgeHeight { get; set; } = 0.0;
        public int ClampPosition { get; set; } = 350000;
        public int TrayPosition { get; set; } = 790000;
        public double GlassOffset { get; set; } = 12.3;
        public string Elastomer { get; set; } = string.Empty;
        public double ElastomerThickness { get; set; } = 0.0;
        public DataGridViewComboBoxCell BergquistComboBoxCell = new DataGridViewComboBoxCell();
        public double BergquistThickness { get; set; } = 0.0;
        public double SurfaceArea { get; set; } = 0.0;
        public double Pressure { get; set; } = 0.0;

        public ExperimentData()
        {
            Configuration config = new Configuration();
            BergquistOptions bergquistOptions = new BergquistOptions();
            BergquistComboBoxCell = bergquistOptions.GetOptionNamesComboBoxCell();
            CartridgeOptions cartridgeOptions = new CartridgeOptions();
            CartridgeComboBoxCell = cartridgeOptions.GetOptionNamesComboBoxCell(config.DefaultPartitionType);
        }
    }
}
