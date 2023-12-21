using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class DataGridViewManager
    {
        public void SetTextBoxCellStringValueByIndicesBasedOnOutcome(DataGridView dataGridView, 
            bool outcome, string valueOnOutcome, string valueOnOutcomeFailed, 
            int rowIndex, int columnIndex)
        {
            if (outcome)
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = valueOnOutcome;
            }
            else
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = valueOnOutcomeFailed;
            }
        }

        public string GetColumnCellValueByColumnAndRowName(string headerText, string rowText, DataGridView dataGridView)
        {
            // Get the column index by the header text
            int columnIndex = -1;
            int rowIndex = -1;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText == headerText)
                {
                    columnIndex = column.Index;
                }
            }
            // Get the row index by the row text
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value == rowText)
                {
                    rowIndex = row.Index;
                }
            }
            // Get the cell value
            return dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString();
        }
    }
}
