using SpinnakerNET.GenApi;
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
        public int Cy5ID = int.MinValue;
        public int FAMID = int.MinValue;
        public int HEXID = int.MinValue;
        public int AttoID = int.MinValue;
        public int AlexaID = int.MinValue;
        public int Cy5p5ID = int.MinValue;
        public int Cy5FilterWheelPosition = int.MinValue;
        public int FAMFilterWheelPosition = int.MinValue;
        public int HEXFilterWheelPosition = int.MinValue;
        public int AttoFilterWheelPosition = int.MinValue;
        public int AlexaFilterWheelPosition = int.MinValue;
        public int Cy5p5FilterWheelPosition = int.MinValue;
        public int xMotorDefaultSpeed = int.MinValue;
        public int xMotorAcceleration = int.MinValue;
        public int xMotorDeacceleration = int.MinValue;
        public int xMotorHomeSpeed = int.MinValue;
        public string xMotorEnabled = string.Empty;
        public int yMotorDefaultSpeed = int.MinValue;
        public int yMotorAcceleration = int.MinValue;
        public int yMotorDeacceleration = int.MinValue;
        public int yMotorHomeSpeed = int.MinValue;
        public string yMotorEnabled = string.Empty;
        public int zMotorDefaultSpeed = int.MinValue;
        public int zMotorAcceleration = int.MinValue;
        public int zMotorDeacceleration = int.MinValue;
        public int zMotorHomeSpeed = int.MinValue;
        public string zMotorEnabled = string.Empty;
        public int FilterWheelMotorDefaultSpeed = int.MinValue;
        public int FilterWheelMotorAcceleration = int.MinValue;
        public int FilterWheelMotorDeacceleration = int.MinValue;
        public int FilterWheelMotorHomeSpeed = int.MinValue;
        public string FilterWheelMotorEnabled = string.Empty;
        public int ClampAMotorDefaultSpeed = int.MinValue;
        public int ClampAMotorAcceleration = int.MinValue;
        public int ClampAMotorDeacceleration = int.MinValue;
        public int ClampAMotorHomeSpeed = int.MinValue;
        public string ClampAMotorEnabled = string.Empty;
        public int ClampBMotorDefaultSpeed = int.MinValue;
        public int ClampBMotorAcceleration = int.MinValue;
        public int ClampBMotorDeacceleration = int.MinValue;
        public int ClampBMotorHomeSpeed = int.MinValue;
        public string ClampBMotorEnabled = string.Empty;
        public int ClampCMotorDefaultSpeed = int.MinValue;
        public int ClampCMotorAcceleration = int.MinValue;
        public int ClampCMotorDeacceleration = int.MinValue;
        public int ClampCMotorHomeSpeed = int.MinValue;
        public string ClampCMotorEnabled = string.Empty;
        public int ClampDMotorDefaultSpeed = int.MinValue;
        public int ClampDMotorAcceleration = int.MinValue;
        public int ClampDMotorDeacceleration = int.MinValue;
        public int ClampDMotorHomeSpeed = int.MinValue;
        public string ClampDMotorEnabled = string.Empty;
        public int TrayABMotorDefaultSpeed = int.MinValue;
        public int TrayABMotorAcceleration = int.MinValue;
        public int TrayABMotorDeacceleration = int.MinValue;
        public int TrayABMotorHomeSpeed = int.MinValue;
        public string TrayABMotorEnabled = string.Empty;
        public int TrayCDMotorDefaultSpeed = int.MinValue;
        public int TrayCDMotorAcceleration = int.MinValue;
        public int TrayCDMotorDeacceleration = int.MinValue;
        public int TrayCDMotorHomeSpeed = int.MinValue;
        public string TrayCDMotorEnabled = string.Empty;
        public string ThermocyclingProtocolsDataPath = string.Empty;
        public string ElastomerDataPath = string.Empty;
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
            var rootNode = xDocument.Root;
            // Read in the Instrument configuration            
            var instrumentNode = rootNode.Element("Instrument");
            InstrumentName = instrumentNode.Element("Name").Value;
            InstrumentID = instrumentNode.Element("ID").Value;
            ThermocyclingProtocolsDataPath = rootNode.Element("ThermocyclingProtocolsDataPath").Value;
            // Read in the instrument configuration for the LEDs
            var ledsNode = instrumentNode.Element("LEDs");
            // Read in the isntrument configuration for the Filter Wheel
            var filterWheelNode = instrumentNode.Element("FilterWheel");
            // Read in the instrument configuration for the Motors
            var motorsNode = instrumentNode.Element("Motors");
            // Read in the instrument configuration for the TECs
            var tecsNode = instrumentNode.Element("TECs");
            // Read in the DataPaths
            var dataPathsNode = rootNode.Element("DataPaths");
            BergquistDataPath = dataPathsNode.Element("Bergquists").Value;
            CartridgeDataPath = dataPathsNode.Element("Cartridges").Value;
            ElastomerDataPath = dataPathsNode.Element("Elastomers").Value;
            DefaultPartitionType = rootNode.Element("DefaultPartitionType").Value;
            DefaultCartridge = rootNode.Element("DefaultCartridge").Value;
        }
    }
}
