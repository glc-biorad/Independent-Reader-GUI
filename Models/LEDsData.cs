using Independent_Reader_GUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    /// <summary>
    /// LEDs Data is based on a property and each LED has a value 
    /// for this property.
    /// </summary>
    internal class LEDsData
    {
        public string PropertyName { get; set; } = "?";
        public string ValueCy5 { get; set; } = "?";
        public string ValueFAM { get; set; } = "?";
        public string ValueHEX { get; set; } = "?";
        public string ValueAtto { get; set; } = "?";
        public string ValueAlexa { get; set; } = "?";
        public string ValueCy5p5 { get; set; } = "?";
        public DataGridViewComboBoxCell IOCy5ComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOFAMComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOHEXComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOAttoComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOAlexaComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOCy5p5ComboBoxCell = new DataGridViewComboBoxCell();

        public LEDsData()
        {
            IOCy5ComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOFAMComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOHEXComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOAttoComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOAlexaComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOCy5p5ComboBoxCell.Items.AddRange(new object[] { "Off", "On" });
            IOCy5ComboBoxCell.Value = IOCy5ComboBoxCell.Items[0];
            IOFAMComboBoxCell.Value = IOFAMComboBoxCell.Items[0];
            IOHEXComboBoxCell.Value = IOHEXComboBoxCell.Items[0];
            IOAttoComboBoxCell.Value = IOAttoComboBoxCell.Items[0];
            IOAlexaComboBoxCell.Value = IOAlexaComboBoxCell.Items[0];
            IOCy5p5ComboBoxCell.Value = IOCy5p5ComboBoxCell.Items[0];
        }
    }
}
