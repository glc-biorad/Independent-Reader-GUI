using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Models
{
    internal class Configuration
    {
        // FIXME: Replace this with a variable taken from a config file
        public string ConfigurationDataPath = "C:\\Users\\u112958\\source\\repos\\Independent-Reader-GUI\\Resources\\Configuration\\Configuration.xml";
        public string InstrumentName = string.Empty;
        public string InstrumentID = string.Empty;
        public string ThermocyclingProtocolsDataPath = string.Empty;
        public string BergquistDataPath = string.Empty;
        public string CartridgeDataPath = string.Empty;
        public string DefaultPartitionType = string.Empty;
        public string DefaultCartridge = string.Empty;

        public Configuration()
        {
            ReadInConfigurationData();
        }

        private void ReadInConfigurationData()
        {
            // Load in the protocol file
            XDocument xDocument = XDocument.Load(ConfigurationDataPath);
            // Read in the config
            var node = xDocument.Root;
            InstrumentName = node.Element("InstrumentName").Value;
            InstrumentID = node.Element("InstrumentID").Value;
            ThermocyclingProtocolsDataPath = node.Element("ThermocyclingProtocolsDataPath").Value;
            BergquistDataPath = node.Element("BergquistDataPath").Value;
            CartridgeDataPath = node.Element("CartridgeDataPath").Value;
            DefaultPartitionType = node.Element("DefaultPartitionType").Value;
            DefaultCartridge = node.Element("DefaultCartridge").Value;
        }
    }
}
