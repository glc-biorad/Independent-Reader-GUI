using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class LEDCommand
    {
        /// <summary>
        /// Possible LED Commands (taken from LED model class)
        /// </summary>
        public enum CommandType
        {
            CheckConnectionAsync,
            GetVersionAsync,
            On,
            Off,
            GetIntensity,
        }

        public CommandType Type { get; set; }
        public int Intensity { get; set; }
        public LED LED { get; set; }
    }
}
