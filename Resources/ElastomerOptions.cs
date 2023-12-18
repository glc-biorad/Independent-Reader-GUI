using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Resources
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
            ReadInBergquistData();
        }

        private void ReadInBergquistData()
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
                elastomer.PolyesterFilm = elastomerNode.Element("PolyesterFilm").Value.ToString();
                elastomer.Mold = elastomerNode.Element("Mold").Value.ToString();
                elastomer.PreparedBy = elastomerNode.Element("PreparedBy").Value.ToString();
                options.Add(elastomer);
            }
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
