using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class Samples
    {
        public List<string> Names;

        public Samples()
        {
            Names = new List<string>();
        }

        public void Add(string name)
        {
            Names.Add(name);
        }
    }
}
