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
        private object[] useOptions = new object[] { "Yes", "No" };
        private object[] ioOptions = new object[] { "Off", "On" };
        public string PropertyName { get; set; } = "?";
        public string ValueCy5 { get; set; } = "?";
        public string ValueFAM { get; set; } = "?";
        public string ValueHEX { get; set; } = "?";
        public string ValueAtto { get; set; } = "?";
        public string ValueAlexa { get; set; } = "?";
        public string ValueCy5p5 { get; set; } = "?";
        public DataGridViewComboBoxCell UseCy5ComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell UseFAMComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell UseHEXComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell UseAttoComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell UseAlexaComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell UseCy5p5ComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOCy5ComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOFAMComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOHEXComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOAttoComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOAlexaComboBoxCell = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell IOCy5p5ComboBoxCell = new DataGridViewComboBoxCell();

        public LEDsData()
        {
            UseCy5ComboBoxCell.Items.AddRange(useOptions);
            UseFAMComboBoxCell.Items.AddRange(useOptions);
            UseHEXComboBoxCell.Items.AddRange(useOptions);
            UseAttoComboBoxCell.Items.AddRange(useOptions);
            UseAlexaComboBoxCell.Items.AddRange(useOptions);
            UseCy5p5ComboBoxCell.Items.AddRange(useOptions);
            // FIXME: Use configuration as a parameter and set the user based defaults for these options
            UseCy5ComboBoxCell.Value = UseCy5ComboBoxCell.Items[0];
            UseFAMComboBoxCell.Value = UseFAMComboBoxCell.Items[0];
            UseHEXComboBoxCell.Value = UseHEXComboBoxCell.Items[0];
            UseAttoComboBoxCell.Value = UseAttoComboBoxCell.Items[0];
            UseAlexaComboBoxCell.Value = UseAlexaComboBoxCell.Items[0];
            UseCy5p5ComboBoxCell.Value = UseCy5p5ComboBoxCell.Items[0];

            IOCy5ComboBoxCell.Items.AddRange(ioOptions);
            IOFAMComboBoxCell.Items.AddRange(ioOptions);
            IOHEXComboBoxCell.Items.AddRange(ioOptions);
            IOAttoComboBoxCell.Items.AddRange(ioOptions);
            IOAlexaComboBoxCell.Items.AddRange(ioOptions);
            IOCy5p5ComboBoxCell.Items.AddRange(ioOptions);
            // FIXME: Use configuration as a parameter and set the user based defaults for these options
            IOCy5ComboBoxCell.Value = IOCy5ComboBoxCell.Items[0];
            IOFAMComboBoxCell.Value = IOFAMComboBoxCell.Items[0];
            IOHEXComboBoxCell.Value = IOHEXComboBoxCell.Items[0];
            IOAttoComboBoxCell.Value = IOAttoComboBoxCell.Items[0];
            IOAlexaComboBoxCell.Value = IOAlexaComboBoxCell.Items[0];
            IOCy5p5ComboBoxCell.Value = IOCy5p5ComboBoxCell.Items[0];
        }
    }
}
