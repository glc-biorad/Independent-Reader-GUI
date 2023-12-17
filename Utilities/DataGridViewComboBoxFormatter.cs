using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class DataGridViewComboBoxFormatter
    {
        public DataGridViewCell GetCellValueComboBox(string cellValue)
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            if (cellValue == "Heater")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "A",
                    "B",
                    "C",
                    "D"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue == "Image Before")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue == "Image During (FOV)")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue == "Image During (Assay)")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue == "Image During (Sample)")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue == "Image After")
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else if (cellValue.Contains("Image in"))
            {
                comboBoxCell.Items.AddRange(new object[]
                {
                    "Yes",
                    "No"
                });
                comboBoxCell.Value = comboBoxCell.Items[0];
            }
            else
            {
                return null;
            }
            return comboBoxCell;
        }
    }
}
