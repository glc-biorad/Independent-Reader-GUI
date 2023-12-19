using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private TimeSpan waitTimeSpan;
        private const int checkInTimeInMilliseconds = 200;

        public Motor(string name, int address, double timeoutInMilliseconds = 2000) 
        {
            this.name = name;
            this.address = address;
            this.waitTimeSpan = TimeSpan.FromMilliseconds(timeoutInMilliseconds);
            // Check the connection for the motor
            CheckConnection();
        }

        public void Move(int positionInMicrosteps, int speedInMicrostepsPerSecond)
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

        public async Task MoveAsync(int positionInMicrosteps, int speedInMicrostepsPerSecond)
        {
            // Check the motor is connected
            CheckConnection();
            if (connected)
            {
                if (homed)
                {
                    // TODO: Implement code for moving the motor using the API
                    MessageBox.Show("Code to move the motor has yet to be implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (!homed || (stopwatch.Elapsed < waitTimeSpan))
                    {
                        MessageBox.Show(stopwatch.Elapsed.ToString());
                        await Task.Delay(checkInTimeInMilliseconds);
                    }
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

        public async Task HomeAsync()
        {
            // Check the motor is connected
            CheckConnection();
            if (connected)
            {
                // TODO: Implement code for homing the motor using the API
                MessageBox.Show("Code to home the motor has yet to be implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (!homed && (stopwatch.Elapsed < waitTimeSpan))
                {
                    // TODO: Check if the motor has been homed
                    await Task.Delay(checkInTimeInMilliseconds);
                }
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

        public bool CheckIfHomingComplete()
        {
            bool homingCompleted = false;
            // TODO: Implement code to check if homing is completed
            return homingCompleted;
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
