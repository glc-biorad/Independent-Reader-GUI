﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class HeaterOptions
    {
        public DataGridViewComboBoxCell Options = new DataGridViewComboBoxCell();
        
        public HeaterOptions()
        {
            Options.Items.Add("A");
            Options.Items.Add("B");
            Options.Items.Add("C");
            Options.Items.Add("D");
        }
    }
}
