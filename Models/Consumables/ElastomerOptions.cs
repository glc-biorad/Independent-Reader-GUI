using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class ElastomerOptions
    {
        private Configuration configuration;
        private string elastomerDataPath = string.Empty;
        private List<Elastomer> options = new List<Elastomer>();

        public ElastomerOptions(Configuration configuration)
        {
            this.configuration = configuration;
            elastomerDataPath = configuration.ElastomerDataPath;
            ReadInElastomerData();
        }

        private void ReadInElastomerData()
        {
            // Load in the protocol file
            XDocument xDocument = XDocument.Load(elastomerDataPath);
            // Read in the Elastomer options
            foreach (var elastomerNode in xDocument.Descendants("Elastomer"))
            {
                Elastomer elastomer = new Elastomer();
                elastomer.Name = elastomerNode.Element("Name").Value;
                elastomer.ThermalCoefficient = double.Parse(elastomerNode.Element("ThermalCoefficient").Value.ToString());
                elastomer.ThermalCoefficientUnits = elastomerNode.Element("ThermalCoefficientUnits").Value.ToString();
                elastomer.ShoreHardness = int.Parse(elastomerNode.Element("ShoreHardness").Value.ToString());
                elastomer.Thickness = double.Parse(elastomerNode.Element("Thickness").Value.ToString());
                elastomer.ThicknessUnits = elastomerNode.Element("ThicknessUnits").Value.ToString();
                elastomer.Color = elastomerNode.Element("Color").Value.ToString();
                elastomer.Film = elastomerNode.Element("Film").Value.ToString();
                elastomer.Mold = elastomerNode.Element("Mold").Value.ToString();
                elastomer.PreparedBy = elastomerNode.Element("PreparedBy").Value.ToString();
                options.Add(elastomer);
            }
        }

        /// <summary>
        /// Return the option names
        /// </summary>
        /// <returns>List of option names</returns>
        public List<string> Names()
        {
            List<string> names = new List<string>();
            foreach (var option in options)
            {
                names.Add(option.Name);
            }
            return names;
        }

        public void Update()
        {
            options.Clear();
            ReadInElastomerData();
        }

        public void SaveNewElastomer(Elastomer elastomer)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            XDocument elastomerXDocument = XDocument.Load(configuration.ElastomerDataPath);
            XElement newElastomerXElement = new XElement("Elastomer");
            if (elastomerXDocument.Root != null)
            {
                elastomerXDocument.Root.Add(newElastomerXElement);
                XElement nameXElement = new XElement("Name", elastomer.Name);
                XElement thermalCoefficientXElement = new XElement("ThermalCoefficient", elastomer.ThermalCoefficient);
                XElement shoreHardnessXElement = new XElement("ShoreHardness", elastomer.ShoreHardness);
                XElement thicknessXElement = new XElement("Thickness", elastomer.Thickness);
                XElement moldXElement = new XElement("Mold", elastomer.Mold);
                XElement filmXElement = new XElement("Film", elastomer.Film);
                XElement preparedByXElement = new XElement("PreparedBy", elastomer.PreparedBy);
                XElement colorXElement = new XElement("Color", elastomer.Color);
                XElement thermalCoefficientUnitsXElement = new XElement("ThermalCoefficientUnits", elastomer.ThermalCoefficientUnits);
                XElement thicknessUnitsXElement = new XElement("ThicknessUnits", elastomer.ThicknessUnits);
                newElastomerXElement.Add(nameXElement);
                newElastomerXElement.Add(thermalCoefficientXElement);
                newElastomerXElement.Add(thermalCoefficientUnitsXElement);
                newElastomerXElement.Add(shoreHardnessXElement);
                newElastomerXElement.Add(thicknessXElement);
                newElastomerXElement.Add(thicknessUnitsXElement);
                newElastomerXElement.Add(colorXElement);
                newElastomerXElement.Add(filmXElement);
                newElastomerXElement.Add(moldXElement);
                newElastomerXElement.Add(preparedByXElement);
                elastomerXDocument.Save(configuration.ElastomerDataPath);
                options.Add(elastomer);
            }
        }

        public void DeleteElastomerByName(string name)
        {
            Elastomer? elastomer = GetElastomerFromName(name);
            if (elastomer != null)
            {
                XDocument elastomerXDocument = XDocument.Load(configuration.ElastomerDataPath);
                var elastomerXElements = elastomerXDocument.Root?.Elements();
                XElement? elementToRemove = null;
                foreach (XElement elastomerXElement in elastomerXElements)
                {
                    if (elastomerXElement.Element("Name").Value == name)
                    {
                        elementToRemove = elastomerXElement;
                    }
                }
                if (elementToRemove != null)
                {
                    elementToRemove.Remove();
                }
                elastomerXDocument.Save(configuration.ElastomerDataPath);
                // Reread the elastomers into the options
                Update();
            }
        }

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

        public List<Elastomer> Options
        {
            get { return options; }
        }

        public Elastomer? GetElastomerFromName(string name)
        {
            foreach (var option in options)
            {
                if (option.Name == name)
                {
                    return option;
                }
            }
            return null;
        }

        public DataGridViewComboBoxCell GetOptionNamesComboBoxCell()
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (var option in options)
            {
                comboBoxCell.Items.Add(option.Name);
            }
            comboBoxCell.Value = configuration.DefaultElastomer;
            return comboBoxCell;
        }
    }
}
