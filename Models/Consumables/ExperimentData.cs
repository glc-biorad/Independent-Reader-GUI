using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class ExperimentData
    {
        public string Name { get; set; } = string.Empty;
        public DataGridViewComboBoxCell ProtocolComboBoxCell = new DataGridViewComboBoxCell();
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public string Heater { get; set; } = string.Empty;
        public DataGridViewComboBoxCell PartitionTypeComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell CartridgeComboBoxCell = new DataGridViewComboBoxCell();
        public double CartridgeLength { get; set; } = 0.0;
        public double CartridgeWidth { get; set; } = 0.0;
        public double CartridgeHeight { get; set; } = 0.0;
        public int ClampPosition { get; set; } = 350000;
        public int TrayPosition { get; set; } = 790000;
        public double GlassOffset { get; set; } = 12.3;
        public DataGridViewComboBoxCell ElastomerComboBoxCell = new DataGridViewComboBoxCell();
        public double ElastomerThickness { get; set; } = 0.0;
        public DataGridViewComboBoxCell BergquistComboBoxCell = new DataGridViewComboBoxCell();
        public double BergquistThickness { get; set; } = 0.0;
        public double ContactSurfaceArea { get; set; } = 0.0;
        public double Pressure { get; set; } = 0.0;

        public ExperimentData()
        {
            Configuration configuration = new Configuration();
            ThermocyclingProtocolManager protocolManager = new ThermocyclingProtocolManager(configuration);
            ProtocolComboBoxCell = protocolManager.GetOptionNamesComboBoxCell();
            BergquistOptions bergquistOptions = new BergquistOptions(configuration);
            BergquistComboBoxCell = bergquistOptions.GetOptionNamesComboBoxCell();
            CartridgeOptions cartridgeOptions = new CartridgeOptions(configuration);
            CartridgeComboBoxCell = cartridgeOptions.GetOptionNamesComboBoxCell(configuration.DefaultPartitionType);
            PartitionTypeOptions partitionTypeOptions = new PartitionTypeOptions();
            PartitionTypeComboBoxCell = partitionTypeOptions.GetOptionNamesComboBoxCell();
            ElastomerOptions elastomerOptions = new ElastomerOptions(configuration);
            ElastomerComboBoxCell = elastomerOptions.GetOptionNamesComboBoxCell();
        }
    }
}
