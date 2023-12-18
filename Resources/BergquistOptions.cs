using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Independent_Reader_GUI.Resources
{
    internal class BergquistOptions
    {
        private Configuration configuration;
        private string bergquistDataPath = string.Empty;
        private List<Bergquist> options = new List<Bergquist>();

        public BergquistOptions(Configuration configuration)
        {
            this.configuration = configuration;
            bergquistDataPath = configuration.BergquistDataPath;
            ReadInBergquistData();
        }

        private void ReadInBergquistData()
        {
            // Load in the protocol file
            XDocument xDocument = XDocument.Load(bergquistDataPath);
            // Read in the Bergquist options
            foreach (var bergquistNode in xDocument.Descendants("Bergquist"))
            {
                Bergquist bergquist = new Bergquist();
                bergquist.Name = bergquistNode.Element("Name").Value;
                bergquist.ThermalCoefficient = double.Parse(bergquistNode.Element("ThermalCoefficient").Value.ToString());
                bergquist.ShoreHardness = int.Parse(bergquistNode.Element("ShoreHardness").Value.ToString());
                bergquist.Thickness = double.Parse(bergquistNode.Element("Thickness").Value.ToString());
                options.Add(bergquist);
            }
        }

        public DataGridViewComboBoxCell GetOptionNamesComboBoxCell()
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (var option in options)
            {
                comboBoxCell.Items.Add(option.Name);
            }
            comboBoxCell.Value = comboBoxCell.Items[0];
            return comboBoxCell;
        }
    }
}
