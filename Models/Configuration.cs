using SpinnakerNET.GenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Models
{
    /// <summary>
    /// Model to represent the Configuration of the Reader 
    /// </summary>
    internal class Configuration
    {
        // FIXME: Replace this with a variable taken from a config file
        private string user = "u112958";
        public string ConfigurationDataPath;
        public string InstrumentName = string.Empty;
        public string InstrumentID = string.Empty;
        public int TECWaitTimeoutInSeconds = int.MinValue;
        public int Cy5ID = int.MinValue;
        public int FAMID = int.MinValue;
        public int HEXID = int.MinValue;
        public int AttoID = int.MinValue;
        public int AlexaID = int.MinValue;
        public int Cy5p5ID = int.MinValue;
        public int Cy5Intensity = int.MinValue;
        public int FAMIntensity = int.MinValue;
        public int HEXIntensity = int.MinValue;
        public int AttoIntensity = int.MinValue;
        public int AlexaIntensity = int.MinValue;
        public int Cy5p5Intensity = int.MinValue;
        public int Cy5FilterWheelPosition = int.MinValue;
        public int FAMFilterWheelPosition = int.MinValue;
        public int HEXFilterWheelPosition = int.MinValue;
        public int AttoFilterWheelPosition = int.MinValue;
        public int AlexaFilterWheelPosition = int.MinValue;
        public int Cy5p5FilterWheelPosition = int.MinValue;
        public int xMotorAddress = int.MinValue;
        public int xMotorDefaultSpeed = int.MinValue;
        public int xMotorAcceleration = int.MinValue;
        public int xMotorDeacceleration = int.MinValue;
        public int xMotorHomeSpeed = int.MinValue;
        public string xMotorEnabled = string.Empty;
        public int yMotorAddress = int.MinValue;
        public int yMotorDefaultSpeed = int.MinValue;
        public int yMotorAcceleration = int.MinValue;
        public int yMotorDeacceleration = int.MinValue;
        public int yMotorHomeSpeed = int.MinValue;
        public string yMotorEnabled = string.Empty;
        public int zMotorAddress = int.MinValue;
        public int zMotorDefaultSpeed = int.MinValue;
        public int zMotorAcceleration = int.MinValue;
        public int zMotorDeacceleration = int.MinValue;
        public int zMotorHomeSpeed = int.MinValue;
        public string zMotorEnabled = string.Empty;
        public int FilterWheelMotorAddress = int.MinValue;
        public int FilterWheelMotorDefaultSpeed = int.MinValue;
        public int FilterWheelMotorAcceleration = int.MinValue;
        public int FilterWheelMotorDeacceleration = int.MinValue;
        public int FilterWheelMotorHomeSpeed = int.MinValue;
        public string FilterWheelMotorEnabled = string.Empty;
        public int ClampAMotorAddress = int.MinValue;
        public int ClampAMotorDefaultSpeed = int.MinValue;
        public int ClampAMotorAcceleration = int.MinValue;
        public int ClampAMotorDeacceleration = int.MinValue;
        public int ClampAMotorHomeSpeed = int.MinValue;
        public string ClampAMotorEnabled = string.Empty;
        public int ClampBMotorAddress = int.MinValue;
        public int ClampBMotorDefaultSpeed = int.MinValue;
        public int ClampBMotorAcceleration = int.MinValue;
        public int ClampBMotorDeacceleration = int.MinValue;
        public int ClampBMotorHomeSpeed = int.MinValue;
        public string ClampBMotorEnabled = string.Empty;
        public int ClampCMotorAddress = int.MinValue;
        public int ClampCMotorDefaultSpeed = int.MinValue;
        public int ClampCMotorAcceleration = int.MinValue;
        public int ClampCMotorDeacceleration = int.MinValue;
        public int ClampCMotorHomeSpeed = int.MinValue;
        public string ClampCMotorEnabled = string.Empty;
        public int ClampDMotorAddress = int.MinValue;
        public int ClampDMotorDefaultSpeed = int.MinValue;
        public int ClampDMotorAcceleration = int.MinValue;
        public int ClampDMotorDeacceleration = int.MinValue;
        public int ClampDMotorHomeSpeed = int.MinValue;
        public string ClampDMotorEnabled = string.Empty;
        public int TrayABMotorAddress = int.MinValue;
        public int TrayABMotorDefaultSpeed = int.MinValue;
        public int TrayABMotorAcceleration = int.MinValue;
        public int TrayABMotorDeacceleration = int.MinValue;
        public int TrayABMotorHomeSpeed = int.MinValue;
        public string TrayABMotorEnabled = string.Empty;
        public int TrayCDMotorAddress = int.MinValue;
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
        public string DefaultElastomer = string.Empty;
        public string DefaultBergquist = string.Empty;
        public double DefaultGlassOffset = double.NaN;
        public int RunDataTimerInterval = int.MinValue;
        public int ControlTabTimerInterval = int.MinValue;
        public int MainFormFastTimerInterval = int.MinValue;
        public int MainFormSlowTimerInterval = int.MinValue;
        public int ThermocyclingTabTimerInterval = int.MinValue;
        public int EstimateFOVCaptureTimeSeconds = int.MinValue;
        public int EstimateAssayCaptureTimeSeconds = int.MinValue;
        public int EstimateSampleCaptureTimeSeconds = int.MinValue;
        public int TECAAddress = int.MinValue;
        public int TECBAddress = int.MinValue;
        public int TECCAddress = int.MinValue;
        public int TECDAddress = int.MinValue;
        public double TECAObjectUpperErrorThreshold = double.NaN;
        public double TECAObjectLowerErrorThreshold = double.NaN;
        public double TECASinkUpperErrorThreshold = double.NaN;
        public double TECASinkLowerErrorThreshold = double.NaN;
        public double TECBObjectUpperErrorThreshold = double.NaN;
        public double TECBObjectLowerErrorThreshold = double.NaN;
        public double TECBSinkUpperErrorThreshold = double.NaN;
        public double TECBSinkLowerErrorThreshold = double.NaN;
        public double TECCObjectUpperErrorThreshold = double.NaN;
        public double TECCObjectLowerErrorThreshold = double.NaN;
        public double TECCSinkUpperErrorThreshold = double.NaN;
        public double TECCSinkLowerErrorThreshold = double.NaN;
        public double TECDObjectUpperErrorThreshold = double.NaN;
        public double TECDObjectLowerErrorThreshold = double.NaN;
        public double TECDSinkUpperErrorThreshold = double.NaN;
        public double TECDSinkLowerErrorThreshold = double.NaN;
        public double TECAFanOnTemp = double.NaN;
        public double TECBFanOnTemp = double.NaN;
        public double TECCFanOnTemp = double.NaN;
        public double TECDFanOnTemp = double.NaN;
        public string ReportsDataPath;
        public string AssetsDataPath;
        public string ScanningDataPath;
        public string ReportLogoDataPath;
        public string DefaultThermocyclingProtocolName;

        public Configuration()
        {
            ConfigurationDataPath = $"C:\\Users\\{user}\\source\\repos\\Independent-Reader-GUI\\Resources\\Configuration\\Configuration.xml";
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
            Cy5ID = int.Parse(ledsNode.Element("Cy5ID").Value);
            Cy5Intensity = int.Parse(ledsNode.Element("Cy5Intensity").Value);
            FAMID = int.Parse(ledsNode.Element("FAMID").Value);
            FAMIntensity = int.Parse(ledsNode.Element("FAMIntensity").Value);
            HEXID = int.Parse(ledsNode.Element("HEXID").Value);
            HEXIntensity = int.Parse(ledsNode.Element("HEXIntensity").Value);
            AttoID = int.Parse(ledsNode.Element("AttoID").Value);
            AttoIntensity = int.Parse(ledsNode.Element("AttoIntensity").Value);
            AlexaID = int.Parse(ledsNode.Element("AlexaID").Value);
            AlexaIntensity = int.Parse(ledsNode.Element("AlexaIntensity").Value);
            Cy5p5ID = int.Parse(ledsNode.Element("Cy5.5ID").Value);
            Cy5p5Intensity = int.Parse(ledsNode.Element("Cy5.5Intensity").Value);
            // Read in the isntrument configuration for the Filter Wheel
            var filterWheelNode = instrumentNode.Element("FilterWheel");
            // Read in the instrument configuration for the Motors
            var motorsNode = instrumentNode.Element("Motors");
            var xMotorNode = motorsNode.Element("x");
            xMotorAddress = int.Parse(xMotorNode.Element("Address").Value);
            xMotorDefaultSpeed = int.Parse(xMotorNode.Element("DefaultSpeed").Value);
            var yMotorNode = motorsNode.Element("y");
            yMotorAddress = int.Parse(yMotorNode.Element("Address").Value);
            yMotorDefaultSpeed = int.Parse(yMotorNode.Element("DefaultSpeed").Value);
            var zMotorNode = motorsNode.Element("z");
            zMotorAddress = int.Parse(zMotorNode.Element("Address").Value);
            zMotorDefaultSpeed = int.Parse(zMotorNode.Element("DefaultSpeed").Value);
            var filterWheelMotorNode = motorsNode.Element("FilterWheel");
            FilterWheelMotorAddress = int.Parse(filterWheelMotorNode.Element("Address").Value);
            FilterWheelMotorDefaultSpeed = int.Parse(filterWheelMotorNode.Element("DefaultSpeed").Value);
            var clampAMotorNode = motorsNode.Element("ClampA");
            ClampAMotorAddress = int.Parse(clampAMotorNode.Element("Address").Value);
            ClampAMotorDefaultSpeed = int.Parse(clampAMotorNode.Element("DefaultSpeed").Value);
            var clampBMotorNode = motorsNode.Element("ClampB");
            ClampBMotorAddress = int.Parse(clampBMotorNode.Element("Address").Value);
            ClampBMotorDefaultSpeed = int.Parse(clampBMotorNode.Element("DefaultSpeed").Value);
            var clampCMotorNode = motorsNode.Element("ClampC");
            ClampCMotorAddress = int.Parse(clampCMotorNode.Element("Address").Value);
            ClampCMotorDefaultSpeed = int.Parse(clampCMotorNode.Element("DefaultSpeed").Value);
            var clampDMotorNode = motorsNode.Element("ClampD");
            ClampDMotorAddress = int.Parse(clampDMotorNode.Element("Address").Value);
            ClampDMotorDefaultSpeed = int.Parse(clampDMotorNode.Element("DefaultSpeed").Value);
            var trayABMotorNode = motorsNode.Element("TrayAB");
            TrayABMotorAddress = int.Parse(trayABMotorNode.Element("Address").Value);
            TrayABMotorDefaultSpeed = int.Parse(trayABMotorNode.Element("DefaultSpeed").Value);
            var trayCDMotorNode = motorsNode.Element("TrayCD");
            TrayCDMotorAddress = int.Parse(trayCDMotorNode.Element("Address").Value);
            TrayCDMotorDefaultSpeed = int.Parse(trayCDMotorNode.Element("DefaultSpeed").Value);
            // Read in the instrument configuration for the TECs
            var tecsNode = instrumentNode.Element("TECs");
            var tecANode = tecsNode.Element("TECA");
            var tecBNode = tecsNode.Element("TECB");
            var tecCNode = tecsNode.Element("TECC");
            var tecDNode = tecsNode.Element("TECD");
            TECAAddress = int.Parse(tecANode.Element("Address").Value.ToString());
            TECBAddress = int.Parse(tecBNode.Element("Address").Value.ToString());
            TECCAddress = int.Parse(tecCNode.Element("Address").Value.ToString());
            TECDAddress = int.Parse(tecDNode.Element("Address").Value.ToString());
            TECAObjectUpperErrorThreshold = double.Parse(tecANode.Element("ObjectUpperErrorThreshold").Value);
            TECAObjectLowerErrorThreshold = double.Parse(tecANode.Element("ObjectLowerErrorThreshold").Value);
            TECASinkUpperErrorThreshold = double.Parse(tecANode.Element("SinkUpperErrorThreshold").Value);
            TECASinkLowerErrorThreshold = double.Parse(tecANode.Element("SinkLowerErrorThreshold").Value);
            TECBObjectUpperErrorThreshold = double.Parse(tecBNode.Element("ObjectUpperErrorThreshold").Value);
            TECBObjectLowerErrorThreshold = double.Parse(tecBNode.Element("ObjectLowerErrorThreshold").Value);
            TECBSinkUpperErrorThreshold = double.Parse(tecBNode.Element("SinkUpperErrorThreshold").Value);
            TECBSinkLowerErrorThreshold = double.Parse(tecBNode.Element("SinkLowerErrorThreshold").Value);
            TECCObjectUpperErrorThreshold = double.Parse(tecCNode.Element("ObjectUpperErrorThreshold").Value);
            TECCObjectLowerErrorThreshold = double.Parse(tecCNode.Element("ObjectLowerErrorThreshold").Value);
            TECCSinkUpperErrorThreshold = double.Parse(tecCNode.Element("SinkUpperErrorThreshold").Value);
            TECCSinkLowerErrorThreshold = double.Parse(tecCNode.Element("SinkLowerErrorThreshold").Value);
            TECDObjectUpperErrorThreshold = double.Parse(tecDNode.Element("ObjectUpperErrorThreshold").Value);
            TECDObjectLowerErrorThreshold = double.Parse(tecDNode.Element("ObjectLowerErrorThreshold").Value);
            TECDSinkUpperErrorThreshold = double.Parse(tecDNode.Element("SinkUpperErrorThreshold").Value);
            TECDSinkLowerErrorThreshold = double.Parse(tecDNode.Element("SinkLowerErrorThreshold").Value);
            TECAFanOnTemp = double.Parse(tecANode.Element("FanOnTemp").Value);
            TECBFanOnTemp = double.Parse(tecBNode.Element("FanOnTemp").Value);
            TECCFanOnTemp = double.Parse(tecCNode.Element("FanOnTemp").Value);
            TECDFanOnTemp = double.Parse(tecDNode.Element("FanOnTemp").Value);
            // Read in the DataPaths
            var dataPathsNode = rootNode.Element("DataPaths");
            ReportsDataPath = dataPathsNode.Element("Reports").Value;
            ReportLogoDataPath = dataPathsNode.Element("ReportLogo").Value;
            BergquistDataPath = dataPathsNode.Element("Bergquists").Value;
            CartridgeDataPath = dataPathsNode.Element("Cartridges").Value;
            ElastomerDataPath = dataPathsNode.Element("Elastomers").Value;
            ScanningDataPath = dataPathsNode.Element("Scanning").Value;
            AssetsDataPath = dataPathsNode.Element("Assets").Value;
            DefaultPartitionType = rootNode.Element("DefaultPartitionType").Value;
            DefaultCartridge = rootNode.Element("DefaultCartridge").Value;
            DefaultElastomer = rootNode.Element("DefaultElastomer").Value;
            DefaultBergquist = rootNode.Element("DefaultBergquist").Value;
            DefaultGlassOffset = double.Parse(rootNode.Element("DefaultGlassOffset").Value);
            RunDataTimerInterval = int.Parse(rootNode.Element("RunDataTimerInterval").Value);
            ControlTabTimerInterval = int.Parse(rootNode.Element("ControlTabTimerInterval").Value);
            MainFormFastTimerInterval = int.Parse(rootNode.Element("MainFormFastTimerInterval").Value);
            MainFormSlowTimerInterval = int.Parse(rootNode.Element("MainFormSlowTimerInterval").Value);
            ThermocyclingTabTimerInterval = int.Parse(rootNode.Element("ThermocyclingTabTimerInterval").Value);
            EstimateFOVCaptureTimeSeconds = int.Parse(rootNode.Element("EstimateFOVCaptureTimeSeconds").Value);
            EstimateAssayCaptureTimeSeconds = int.Parse(rootNode.Element("EstimateAssayCaptureTimeSeconds").Value);
            EstimateSampleCaptureTimeSeconds = int.Parse(rootNode.Element("EstimateSampleCaptureTimeSeconds").Value);
            DefaultThermocyclingProtocolName = rootNode.Element("DefaultThermocyclingProtocolName").Value;
            TECWaitTimeoutInSeconds = int.Parse(rootNode.Element("TECWaitTimeoutInSeconds").Value);
        }
    }
}
