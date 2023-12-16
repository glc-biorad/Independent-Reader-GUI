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
        // FIXME: Replace this with a variable taken from a config file
        private string bergquistDataPath = "C:\\Users\\u112958\\source\\repos\\Independent-Reader-GUI\\Resources\\Bergquist\\BergquistData.xml";
        private List<Bergquist> list = new List<Bergquist>();

        public BergquistOptions()
        {
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
                list.Add(bergquist);
            }
        }

        public List<string> ListNames()
        {
            List<string> names = new List<string>();
            foreach (var item in list)
            {
                names.Add(item.Name);
            }
            return names;
        }
    }
}
