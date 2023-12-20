using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class DataGridViewCellColorizer
    {
        public void SetCellColorBasedOnOutcome(bool outcome, 
            DataGridView dataGridView, 
            Color colorOutcomeSuccess, Color colorOutcomeFailed, 
            string textOnOutcomeSuccess, string textOnOutcomeFailed, 
            int rowIndex, int columnIndex)
        {
            if (outcome)
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = textOnOutcomeSuccess;
                dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor = colorOutcomeSuccess;
                dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = Color.White;
            }
            else
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = textOnOutcomeFailed;
                dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor = colorOutcomeFailed;
                dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = Color.White;
            }
        }
    }
}
