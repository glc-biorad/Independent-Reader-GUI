using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using iText.Kernel.XMP.Impl;
using Independent_Reader_GUI.Models.Protocols.ThermocyclingProtocol;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage a Thermocycling Protocol
    /// </summary>
    internal class ThermocyclingProtocolManager
    {
        Configuration configuration;

        public ThermocyclingProtocolManager(Configuration configuration)
        {
            this.configuration = configuration;
        }
        /// <summary>
        /// Load in a protocol to a ThermocyclingProtocol instance 
        /// </summary>
        /// <param name="filePath">Path to the protocol XML file</param>
        /// <returns></returns>
        public ThermocyclingProtocol LoadProtocol(string filePath)
        {
            ThermocyclingProtocol protocol = new ThermocyclingProtocol();           
            // Load in the protocol file
            XDocument protocolXDocument = XDocument.Load(filePath);
            protocol.SetName(protocolXDocument.Root.Element("Name").Value);
            // Read in the steps
            foreach (var stepNode in protocolXDocument.Descendants("Step"))
            {
                ThermocyclingProtocolStep step = new ThermocyclingProtocolStep();
                step.Index = int.Parse(stepNode.Element("Index").Value);
                step.TypeName = stepNode.Element("TypeName").Value;
                if (step.TypeName.Equals(ThermocyclingProtocolStepType.Set))
                {
                    step.Temperature = double.Parse(stepNode.Element("Temperature").Value);
                    step.Time = double.Parse(stepNode.Element("Time").Value);
                    step.TemperatureUnits = stepNode.Element("TemperatureUnits").Value.ToString();
                    step.TimeUnits = stepNode.Element("TimeUnits").Value;
                }
                else if (step.TypeName.Equals(ThermocyclingProtocolStepType.GoTo))
                {
                    step.StepNumber = int.Parse(stepNode.Element("StepNumber").Value);
                    step.CycleCount = int.Parse(stepNode.Element("CycleCount").Value);
                }
                else if (step.TypeName.Equals(ThermocyclingProtocolStepType.Hold))
                {
                    step.Temperature = double.Parse(stepNode.Element("Temperature").Value);
                    step.TemperatureUnits = stepNode.Element("TemperatureUnits").Value;
                }
                // Add the step
                protocol.AddStep(step);
            }
            return protocol;
        }

        /// <summary>
        /// Save a protocol as an XML
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="filePath"></param>
        /// <param name="username"></param>
        /// <param name="date"></param>
        public void SaveProtocol(ThermocyclingProtocol protocol, string filePath, string username, string date)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var protocolXDocument = new XDocument();
            var rootXElement = new XElement("Protocol");
            var versionXElement = new XElement("Versions", "1.0");
            var nameXElement = new XElement("Name", protocol.Name);
            var authorXElement = new XElement("Author", username.Replace("User: ", ""));
            var dateXElement = new XElement("Date", date);
            rootXElement.Add(versionXElement);
            rootXElement.Add(nameXElement);
            rootXElement.Add(authorXElement);
            rootXElement.Add(dateXElement);

            foreach (ThermocyclingProtocolStep step in protocol.Steps)
            {
                var stepElement = new XElement("Step");
                stepElement.Add(new XElement("Index", step.Index));
                stepElement.Add(new XElement("TypeName", step.TypeName));
                if (step.TypeName.Equals(ThermocyclingProtocolStepType.Set))
                {
                    stepElement.Add(new XElement("Temperature", step.Temperature));
                    stepElement.Add(new XElement("Time", step.Time));
                    stepElement.Add(new XElement("TemperatureUnits", step.TemperatureUnits));
                    stepElement.Add(new XElement("TimeUnits", step.TimeUnits));
                }
                else if (step.TypeName.Equals(ThermocyclingProtocolStepType.Hold))
                {
                    stepElement.Add(new XElement("Temperature", step.Temperature));
                    stepElement.Add(new XElement("TemperatureUnits", step.TemperatureUnits));
                }
                else if (step.TypeName.Equals(ThermocyclingProtocolStepType.GoTo))
                {
                    stepElement.Add(new XElement("StepNumber", step.StepNumber));
                    stepElement.Add(new XElement("CycleCount", step.CycleCount));
                }
                rootXElement.Add(stepElement);
            }
            protocolXDocument.Add(rootXElement);
            protocolXDocument.Save(filePath);
        }

        /// <summary>
        /// Get DataGridViewComboBoxCell of protocol names
        /// </summary>
        /// <returns></returns>
        public DataGridViewComboBoxCell GetOptionNamesComboBoxCell()
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            // Get the protocol files from the Thermocycling Protocol Data Path from the configuration file
            var protocolFiles = from file in Directory.EnumerateFiles(configuration.ThermocyclingProtocolsDataPath) select file;
            // Iterate through the files and check the names
            foreach (var protocolFile in protocolFiles)
            {
                XDocument protocolXDoc = XDocument.Load(protocolFile);
                string protocolName = protocolXDoc.Root.Element("Name").Value;
                comboBoxCell.Items.Add(protocolName);
            }
            // TODO: Use the config file to set the default value
            comboBoxCell.Value = comboBoxCell.Items[0];
            return comboBoxCell;
        }

        public string GetProtocolFilePathFromProtocolName(string name)
        {
            // Get the protocol files from the Thermocycling Protocol Data Path from the configuration file
            var protocolFiles = from file in Directory.EnumerateFiles(configuration.ThermocyclingProtocolsDataPath) select file;
            // Iterate through the files and check the names
            foreach (var protocolFile in protocolFiles)
            {
                XDocument protocolXDoc = XDocument.Load(protocolFile);
                string protocolName = protocolXDoc.Root.Element("Name").Value;
                if (protocolName == name)
                {
                    return protocolFile;
                }
            }
            return null;
        }

        public ThermocyclingProtocol GetProtocolFromName(string name)
        {
            // Get the file path from the name
            string filePath = GetProtocolFilePathFromProtocolName(name);
            // Load in the protocol
            ThermocyclingProtocol protocol = LoadProtocol(filePath);
            return protocol;
        }

        public bool ProtocolNameExists(string name)
        {
            // Get the protocol files from the Thermocycling Protocol Data Path from the configuration file
            var protocolFiles = from file in Directory.EnumerateFiles(configuration.ThermocyclingProtocolsDataPath) select file;
            // Iterate through the files and check the names
            foreach (var protocolFile in protocolFiles)
            {
                XDocument protocolXDoc = XDocument.Load(protocolFile);
                string protocolName = protocolXDoc.Root.Element("Name").Value;
                if (protocolName == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
