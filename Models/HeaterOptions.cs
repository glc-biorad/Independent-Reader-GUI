using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class HeaterOptions
    {
        public DataGridViewComboBoxCell Options = new DataGridViewComboBoxCell();
        private const string a = "A";
        private const string b = "B";
        private const string c = "C";
        private const string d = "D";

        public HeaterOptions()
        {
            Options.Items.Add(A);
            Options.Items.Add(B);
            Options.Items.Add(C);
            Options.Items.Add(D);
        }

        public string A
        {
            get { return a;  }
        }

        public string B
        {
            get { return b; }
        }

        public string C
        {
            get { return c; }
        }

        public string D
        {
            get { return d; }
        }
    }
}
