using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Resources
{
    internal class CartridgeOptions
    {
        // FIXME: Replace this with a variable taken from a config file
        Configuration configuration;
        //private string cartridgeDataPath = "C:\\Users\\u112958\\source\\repos\\Independent-Reader-GUI\\Resources\\Cartridges\\CartridgeData.xml";
        private List<Cartridge> options = new List<Cartridge>();

        public CartridgeOptions(Configuration configuration)
        {
            this.configuration = configuration;
            ReadInCartridgeData();
        }

        /// <summary>
        /// Read in the Cartridge data from the XML file
        /// </summary>
        private void ReadInCartridgeData()
        {
            // Load in the protocol file
            XDocument xDocument = XDocument.Load(configuration.CartridgeDataPath);
            // Read in the Cartridge options
            foreach (var cartridgeNode in xDocument.Descendants("Cartridge"))
            {
                Cartridge cartridge = new Cartridge();
                cartridge.Name = cartridgeNode.Element("Name").Value;
                cartridge.PartitionType = cartridgeNode.Element("PartitionType").Value;
                cartridge.NumberofSampleChambers = int.Parse(cartridgeNode.Element("NumberofSampleChambers").Value.ToString());
                cartridge.NumberofAssayChambers = int.Parse(cartridgeNode.Element("NumberofAssayChambers").Value.ToString());
                cartridge.Length = double.Parse(cartridgeNode.Element("Length").Value.ToString());
                cartridge.LengthUnits = cartridgeNode.Element("LengthUnits").Value;
                cartridge.Width = double.Parse(cartridgeNode.Element("Width").Value.ToString());
                cartridge.WidthUnits = cartridgeNode.Element("WidthUnits").Value;
                cartridge.Height = double.Parse(cartridgeNode.Element("Height").Value.ToString());
                cartridge.HeightUnits = cartridgeNode.Element("HeightUnits").Value;
                options.Add(cartridge);
            }
        }

        public DataGridViewComboBoxCell GetOptionNamesComboBoxCell(string partitionType)
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (var option in options)
            {
                if (option.PartitionType.Equals(partitionType))
                {
                    comboBoxCell.Items.Add(option.Name);
                }
            }
            comboBoxCell.Value = comboBoxCell.Items[0];
            return comboBoxCell;
        }

        public Cartridge GetCartridgeFromName(string cartridgeName)
        {
            Cartridge cartridge = new Cartridge();
            foreach (var option in options)
            {
                if (option.Name.Equals(cartridgeName))
                {
                    cartridge.Name = option.Name;
                    cartridge.Width = option.Width;
                    cartridge.WidthUnits = option.WidthUnits;
                    cartridge.HeightUnits = option.HeightUnits;
                    cartridge.LengthUnits = option.LengthUnits;
                    cartridge.Length = option.Length;
                    cartridge.Height = option.Height;
                    cartridge.NumberofSampleChambers = option.NumberofSampleChambers;
                    cartridge.NumberofAssayChambers = option.NumberofAssayChambers;
                    cartridge.PartitionType = option.PartitionType;
                    break;
                }
            }
            return cartridge;
        }
    }
}
