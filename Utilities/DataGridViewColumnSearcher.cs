using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class DataGridViewColumnSearcher
    {
        public int GetColumnIndexFromHeaderText(string headerText, DataGridView dataGridView)
        {
            int columnIndex = -1;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText.Equals(headerText))
                {
                    columnIndex = column.Index;
                    break;
                }
            }
            return columnIndex;
        }
    }
}
