using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class TEC
    {
        private int id = int.MinValue;
        private string name = string.Empty;
        public bool Connected;

        public TEC(int id, string name)
        {
            this.id = id;
            this.name = name;
            if (id == GetBoardAddress())
            {
                Connected = true;
            }
            else
            {
                Connected = false;
                //MessageBox.Show(name.ToUpper() + " is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.name = name;
        }

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public int GetBoardAddress()
        {
            int boardAddress = int.MinValue;
            // TODO: Implement code to get the address for this TEC board (use to test connection)
            return boardAddress;
        }

        public void Reset()
        {
            // TODO: Implement code to reset the TEC
            MessageBox.Show("Code to reset the TEC has not been implmented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetTemp()
        {
            // TODO: Implement code to set temp for the TEC
            MessageBox.Show("Code to set the temp for the TEC has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public double GetTemp()
        {
            double temp = double.NaN;
            // TODO: Implement code to get temp for the TEC
            MessageBox.Show("Code to get the temp for the TEC has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return temp;
        }

    }
}
