using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class DataGridViewContentDeleter
    {
        /// <summary>
        /// Delete selected rows for a given DataGridView.
        /// </summary>
        /// <param name="dataGridView">DataViewGrid of focus</param>
        public void DeleteSelectedRows(DataGridView dataGridView)
        {
            if (dataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell selectedCell in dataGridView.SelectedCells)
                {
                    dataGridView.Rows.RemoveAt(selectedCell.RowIndex);
                }
            }
            else
            {
                MessageBox.Show("No rows selected", "Information");
            }
        }
    }
}
