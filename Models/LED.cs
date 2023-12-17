using Independent_Reader_GUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class LED
    {
        private string name = string.Empty;
        private int id = int.MinValue;
        public string IO = string.Empty;
        public bool Connected;
        public int Intensity = int.MinValue;
        public int Exposure = int.MinValue;
        private Configuration configuration;

        public LED(string name, Configuration configuration, bool connected)
        {
            // TODO: pass an object of the LED board to this class for a connection
            this.Connected = connected;
            this.configuration = configuration;
            this.name = name;
            SetIDFromName();
        }

        private void SetIDFromName()
        {
            if (name.Equals("Cy5"))
            {
                id = configuration.Cy5ID;
            }
            else if (name.Equals("FAM"))
            {
                id = configuration.FAMID;
            }
            else if (name.Equals("HEX"))
            {
                id = configuration.HEXID;
            }
            else if (name.Equals("Atto"))
            {
                id = configuration.AttoID;
            }
            else if (name.Equals("Alexa"))
            {
                id = configuration.AlexaID;
            }
            else if (name.Equals("Cy5.5"))
            {
                id = configuration.Cy5p5ID;
            }
            else
            {
                MessageBox.Show(name + " is not a valid LED name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void On()
        {
            if (Connected)
            {
                // TODO: Implenet code to turn on the LEDs
                MessageBox.Show("Code to turn on the LEDs has not yet been implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cannot turn on " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new LEDNotConnectedException();
            }
        }

        public void Off()
        {
            if (Connected)
            {
                // TODO: Implenet code to turn off the LEDs
                MessageBox.Show("Code to turn off the LEDs has not yet been implemented", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //MessageBox.Show("Cannot turn off " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new LEDNotConnectedException();
            }
        }

        public string Name 
        { 
            get { return name; } 
        }

        public int ID
        {
            get { return id; }
        }
    }
}
