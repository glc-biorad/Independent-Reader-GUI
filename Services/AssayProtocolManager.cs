using Independent_Reader_GUI.Models.Protocols.AssayProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Services
{
    internal class AssayProtocolManager
    {
        public AssayProtocol LoadProtocol(string filePath)
        {
            AssayProtocol protocol = new AssayProtocol();
            // Load in the protocol file
            XDocument protocolXDocument = XDocument.Load(filePath);
            protocol.SetName(protocolXDocument.Root.Element("Name").Value);
            // Get the Action Node
            XElement actionNode = protocolXDocument.Root.Element("Actions");
            // Read in the Actions
            foreach (XElement nameNode in actionNode.Nodes())
            {
                // Initialize the Assay Protocol Action
                AssayProtocolAction _ = new AssayProtocolAction();
                // Get the Action index
                _.ID = int.Parse(nameNode.Element("ID").Value);
                // Get the Action Type
                _.Type = nameNode.Element("Type").Value;
                // Get the Action Action Text
                _.ActionText = nameNode.Element("ActionText").Value;
                // Get the Action Description
                _.Description = nameNode.Element("Description").Value;
                // Get the Action based on the Action Text
                var action = _.GetAssayProtocolActionFromActionText();
                // Add the Action
                protocol.AddAction(action);
            }
            return protocol;
        }
    }
}
