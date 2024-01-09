using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Assays
    {
        public List<string> Names;

        public Assays()
        {
            Names = new List<string>();
        }

        public void Add(string name)
        {
            Names.Add(name);
        }
    }
}
