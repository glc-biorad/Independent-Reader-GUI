using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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

        /// <summary>
        /// Determines if a name is in the options list or not 
        /// </summary>
        /// <param name="name">name of the cartridge to be tested</param>
        /// <returns>a bool value, true if it exists, false otherwise</returns>
        public bool NameInOptions(string name)
        {
            bool nameFound = false;
            foreach (var option in options)
            {
                if (option.Name == name)
                {
                    nameFound = true;
                }
            }
            return nameFound;
        }

        public void SaveNewCartridge(Cartridge cartridge)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            XDocument cartridgeXDocument = XDocument.Load(configuration.CartridgeDataPath);
            XElement newCartridgeXElement = new XElement("Cartridge");
            if (cartridgeXDocument.Root != null)
            {
                cartridgeXDocument.Root.Add(newCartridgeXElement);
                XElement nameXElement = new XElement("Name", cartridge.Name);
                XElement partitionTypeXElement = new XElement("PartitionType", cartridge.PartitionType);
                XElement numberofSampleChambersXElement = new XElement("NumberofSampleChambers", cartridge.NumberofSampleChambers);
                XElement numberofAssayChambersXElement = new XElement("NumberofAssayChambers", cartridge.NumberofAssayChambers);
                XElement lengthXElement = new XElement("Length", cartridge.Length);
                XElement lengthUnitsXElement = new XElement("LengthUnits", cartridge.LengthUnits);
                XElement widthXElement = new XElement("Width", cartridge.Width);
                XElement widthUnitsXElement = new XElement("WidthUnits", cartridge.WidthUnits);
                XElement heightXElement = new XElement("Height", cartridge.Height);
                XElement heightUnitsXElement = new XElement("HeightUnits", cartridge.HeightUnits);
                newCartridgeXElement.Add(nameXElement);
                newCartridgeXElement.Add(partitionTypeXElement);
                newCartridgeXElement.Add(numberofSampleChambersXElement);
                newCartridgeXElement.Add(numberofAssayChambersXElement);
                newCartridgeXElement.Add(lengthXElement);
                newCartridgeXElement.Add(lengthUnitsXElement);
                newCartridgeXElement.Add(widthXElement);
                newCartridgeXElement.Add(widthUnitsXElement);
                newCartridgeXElement.Add(heightXElement);
                newCartridgeXElement.Add(heightUnitsXElement);
                cartridgeXDocument.Save(configuration.CartridgeDataPath);
                options.Add(cartridge);
            }
        }

        public void UpdateCartridge(string oldName, Cartridge cartridge)
        {
            // Update the XML
            XDocument cartridgeXDocument = XDocument.Load(configuration.CartridgeDataPath);
            var cartridgeXElements = cartridgeXDocument.Root?.Elements();
            XElement? elementToUpdate = null;
            foreach (XElement cartridgeXElement in cartridgeXElements)
            {
                if (cartridgeXElement.Element("Name").Value == oldName)
                {
                    elementToUpdate = cartridgeXElement;
                    elementToUpdate.Element("Name").Value = cartridge.Name;
                    elementToUpdate.Element("NumberofSampleChambers").Value = cartridge.NumberofSampleChambers.ToString();
                    elementToUpdate.Element("NumberofAssayChambers").Value = cartridge.NumberofAssayChambers.ToString();
                    elementToUpdate.Element("Length").Value = cartridge.Length.ToString();
                    elementToUpdate.Element("Width").Value = cartridge.Width.ToString();
                    elementToUpdate.Element("Height").Value = cartridge.Height.ToString();
                    cartridgeXDocument.Save(configuration.CartridgeDataPath);
                    break;
                }              
            }
            Update();
        }

        public void DeleteCartridgeByName(string name)
        {
            Cartridge? cartridge = GetCartridgeFromName(name);
            if (cartridge != null)
            {
                XDocument cartridgeXDocument = XDocument.Load(configuration.CartridgeDataPath);
                var cartridgeXElements = cartridgeXDocument.Root?.Elements();
                XElement? elementToRemove = null;
                foreach (XElement cartridgeXElement in cartridgeXElements)
                {
                    if (cartridgeXElement.Element("Name").Value == name)
                    {
                        elementToRemove = cartridgeXElement;
                    }
                }
                if (elementToRemove != null)
                {
                    elementToRemove.Remove();
                }
                cartridgeXDocument.Save(configuration.CartridgeDataPath);
                // Reread the cartridges into the options
                Update();
            }
        }

        /// <summary>
        /// Update the Cartridge Options by clearing then rereading in the data
        /// </summary>
        public void Update()
        {
            options.Clear();
            ReadInCartridgeData();
        }

        public List<Cartridge> Options
        {
            get { return options; }
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

        public Cartridge? GetCartridgeFromName(string cartridgeName)
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
                    return cartridge;
                }
            }
            return null;
        }
    }
}
