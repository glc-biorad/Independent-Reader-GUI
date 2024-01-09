using Independent_Reader_GUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage DataGridView use
    /// </summary>
    internal class DataGridViewManager
    {
        public void SetupDataGridViewWithValues(DataGridView dataGridView, List< List<string> > rowsContent)
        {
            foreach (var rowContent in rowsContent)
            {
                dataGridView.Rows.Add();
                foreach (var cellContent in rowContent)
                {
                    dataGridView.Rows.Add(cellContent);
                }
            }
        }

        /// <summary>
        /// Clear the selected cells of their values when they are not in EditMode and are not ReadOnly
        /// </summary>
        /// <param name="dataGridView"></param>
        public void ClearSelectedCellsNotInEditMode(DataGridView dataGridView)
        {
            if (!dataGridView.CurrentCell.IsInEditMode && !dataGridView.CurrentCell.ReadOnly)
            {
                foreach (DataGridViewCell selectedCell in dataGridView.SelectedCells)
                {
                    selectedCell.Value = "";
                }
            }
        }

        /// <summary>
        /// Remove a row by the row's name
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="rowName"></param>
        public void RemoveRowByName(DataGridView dataGridView, string rowName)
        {
            var row = GetRowFromName(dataGridView, rowName);
            dataGridView.Rows.Remove(row);
        }

        public List<DataGridViewRow> GetRowsByNameWhichContain(DataGridView dataGridView, string flag)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value.ToString().Contains(flag))
                {
                    rows.Add(row);
                }
            }
            return rows;
        }

        public void RemoveRowsByNameWhichContains(DataGridView dataGridView, string flag)
        {
            // Get the rows to delete
            var rows = GetRowsByNameWhichContain(dataGridView, flag);
            foreach (DataGridViewRow row in rows)
            {
                dataGridView.Rows.Remove(row);
            }
        }

        /// <summary>
        /// Obtain a row given the cell value in a particular column
        /// </summary>
        /// <param name="dataGridView">DataGridView of interest</param>
        /// <param name="name">String to search for in the particular row</param>
        /// <param name="columnIndex">Column to search for the name in (default to the first column)</param>
        /// <returns></returns>
        public DataGridViewRow? GetRowFromName(DataGridView dataGridView, string name, int columnIndex = 0)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[columnIndex].Value.ToString() == name)
                {
                    return row;
                }
            }
            return null;
        }

        /// <summary>
        /// Set the TextBox value of a DataGridView (given the row and column indices) based on an outcome given
        /// </summary>
        /// <param name="dataGridView">DataGridView to be edited</param>
        /// <param name="outcome">Outcome to check for to choose between what to put in the cell</param>
        /// <param name="valueOnOutcome">Value to input into the DataGridView cell if the outcome is successful</param>
        /// <param name="valueOnOutcomeFailed">Value to input into the DataGridView cell if the outcome is not successful</param>
        /// <param name="rowIndex">Row index for the cell to be set</param>
        /// <param name="columnIndex">Column index for the cell to be set</param>
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

        /// <summary>
        /// Set a text box cell value of a DataGridView provided the names of the column and row based on an outcome.
        /// </summary>
        /// <param name="dataGridView">DataGridView to be edited</param>
        /// <param name="outcome">Outcome to check for to choose between what to put in the cell</param>
        /// <param name="valueOnOutcome">Value to input into the DataGridView cell if the outcome is successful</param>
        /// <param name="valueOnOutcomeFailed">Value to input into the DataGridView cell if the outcome is not successful</param>
        /// <param name="rowName">Row name for the cell to be set</param>
        /// <param name="columnName">Column name for the cell to be set</param>
        public void SetTextBoxCellStringValueByColumnAndRowNamesBasedOnOutcome(DataGridView dataGridView,
            bool outcome, string valueOnOutcome, string valueOnOutcomeFailed,
            string rowName, string columnName)
        {
            int columnIndex = GetColumnIndexFromName(dataGridView, columnName);
            int rowIndex = GetRowIndexFromName(dataGridView, rowName);
            if (outcome)
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = valueOnOutcome;
            }
            else
            {
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = valueOnOutcomeFailed;
            }
        }

        /// <summary>
        /// Set a text box cell of a DataGridView provided the names of the column and row
        /// </summary>
        /// <param name="dataGridView">DataGridView to be edited</param>
        /// <param name="columnName">Column name for the cell to be set</param>
        /// <param name="rowName">Row name for the cell to be set</param>
        /// <param name="cellValue">Value to place in the DataGridView's cell</param>
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

        /// <summary>
        /// Set a text box cell of a DataGridView provided the indices of the row and column
        /// </summary>
        /// <param name="dataGridView">DataGridView to be edited</param>
        /// <param name="cellValue">Value to place in the DataGridView's cell</param>
        /// <param name="rowIndex">Row index for the cell to be set</param>
        /// <param name="columnName">Column index for the cell to be set</param>
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

        /// <summary>
        /// Get the column index provided the column's name
        /// </summary>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <param name="columnName">Column name of interest</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the row index based on the row's name
        /// </summary>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <param name="rowName">Row name of interest</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the row name provided it's index
        /// </summary>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <param name="rowIndex">Index of the row to be checked</param>
        /// <returns></returns>
        public string? GetRowNameFromIndex(DataGridView dataGridView, int rowIndex)
        {
            string? rowName = null;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Index == rowIndex)
                {
                    rowName = row.Cells[0].Value.ToString();
                    return rowName;
                }
            }
            return rowName;
        }

        /// <summary>
        /// Get the column cell value provided its Column and Row name
        /// </summary>
        /// <param name="headerText">Column header text</param>
        /// <param name="rowText">Row name for the column of interest</param>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <returns></returns>
        public string GetColumnCellValueByColumnAndRowName(string headerText, string rowText, DataGridView dataGridView)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error, was unable to find a cell value for column ({headerText}) and row ({rowText}) in {dataGridView.Name}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get the column cell value by a column name and a row index
        /// </summary>
        /// <param name="dataGridView">DataGridView to be checked</param>
        /// <param name="columnName">Column name to check for a value</param>
        /// <param name="rowIndex">Row index for a value to be checked</param>
        /// <returns></returns>
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
