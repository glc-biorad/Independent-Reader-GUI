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

        public void SetTextBoxCellStringValueByColumnandRowNames(DataGridView dataGridView,
            string columnName, string rowName, string cellValue)
        {
            int columnIndex = GetColumnIndexFromName(dataGridView, columnName);
            int rowIndex = GetRowIndexFromName(dataGridView, rowName);
            try
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = cellValue;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void SetTextBoxCellStringValueByIndices(DataGridView dataGridView,
            string cellValue, int rowIndex, int columnIndex)
        {
            dataGridView.Rows[rowIndex].Cells[columnIndex].Value = cellValue;
        }

        public void SetTextBoxCellStringValueByRowIndexandColumnName(DataGridView dataGridView,
            string cellValue, int rowIndex, string columnName)
        {
            int columnIndex = GetColumnIndexFromName(dataGridView, columnName);
            try
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = cellValue;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public int GetColumnIndexFromName(DataGridView dataGridView, string columnName)
        {
            int columnIndex = -1;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText == columnName)
                {
                    columnIndex = column.Index;
                }
            }
            return columnIndex;
        }

        public int GetRowIndexFromName(DataGridView dataGridView, string rowName)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value.ToString() == rowName)
                {
                    rowIndex = row.Index;
                }
            }
            return rowIndex;
        }

        /// <summary>
        /// Get the Column Name (HeaderText) from the column index
        /// </summary>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <param name="columnIndex">Column index for the column of interest</param>
        /// <returns>HeaderText for the column, otherwise null if nothing found</returns>
        public string? GetColumnNameFromIndex(DataGridView dataGridView, int columnIndex)
        {
            string? columnName = null;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Index == columnIndex)
                {
                    columnName = column.HeaderText;
                    return columnName;
                }
            }
            return columnName;
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

        public string? GetColumnCellValueByColumnNameAndRowIndex(DataGridView dataGridView, string columnName, int rowIndex)
        {
            int columnIndex = GetColumnIndexFromName(dataGridView, columnName);
            try
            {
                return dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
