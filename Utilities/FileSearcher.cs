using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class FileSearcher
    {
        private const string defaultInitialDirectory = "C:\\";
        private const string defaultFilter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        /// <summary>
        /// Get the file path for a file to be loaded
        /// </summary>
        /// <param name="initialDirectory">Initial directory to put the open dialog window in</param>
        /// <param name="filter">Filter for the files in this directory</param>
        /// <returns>The file path of the file to be loaded</returns>
        public string GetLoadFilePath(string initialDirectory = defaultInitialDirectory, string filter = defaultFilter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = initialDirectory;
                openFileDialog.Filter = filter;
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the file path for a file to be saved
        /// </summary>
        /// <param name="initialDirectory">Initial directory to put the open dialog window in</param>
        /// <param name="filter">Filter for the files in this directory</param>
        /// <returns>The file path of the file to be saved</returns>
        public string GetSaveFilePath(string initialDirectory = defaultInitialDirectory, string filter = defaultFilter)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = initialDirectory;
                saveFileDialog.Filter = filter;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
            }
            return null;
        }
    }
}
