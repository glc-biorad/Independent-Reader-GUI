using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class PartitionTypeOptions
    {
        private List<string> options = new List<string>();

        public PartitionTypeOptions()
        {
            options.Add(PartitionType.Microwells);
            options.Add(PartitionType.Droplets);
        }

        public DataGridViewComboBoxCell GetOptionNamesComboBoxCell()
        {
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (var option in options)
            {
                comboBoxCell.Items.Add(option);
            }
            comboBoxCell.Value = PartitionType.Default;
            return comboBoxCell;
        }
    }
}
