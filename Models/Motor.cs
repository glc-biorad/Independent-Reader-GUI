using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Motor
    {
        private string name = string.Empty;
        private int address = int.MinValue;
        public string IO = string.Empty;
        private bool connected = false;
        private bool homed = false;

        public Motor(string name, int address) 
        {
            this.name = name;
            this.address = address;
            // Check the connection for the motor
            CheckConnection();
        }

        public void Move(int positioninMicrosteps, int speedInMicrostepsPerSecond)
        {
            // Check the motor is connected
            CheckConnection();
            if (connected)
            {
                if (homed)
                {
                    // TODO: Implement code for moving the motor using the API
                    MessageBox.Show("Code to move the motor has yet to be implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void Home()
        {
            // Check the motor is connected
            CheckConnection();
            if (connected)
            {
                // TODO: Implement code for homing the motor using the API
                MessageBox.Show("Code to home the motor has yet to be implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void CheckConnection()
        {
            // TODO: Implement code for checking if the motor is connected (check version from the API and check for successful response)
            connected = false;
            //if (true)
            //{
            //    connected = true;
            //}
            //else
            //{
            //    connected = false;
            //}
        }

        public string Name
        {
            get { return name; }
        }

        public bool Connected
        {
            get { return connected; }
        }

        public bool Homed
        {
            get { return homed; }
        }
    }
}
