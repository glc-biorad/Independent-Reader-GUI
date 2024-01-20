using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Hardware.Chassis
{
    internal class ChassisCommand
    {
        /// <summary>
        /// Possible Chassis Commands (taken from Chassis model class)
        /// </summary>
        public enum CommandType
        {
            SetPowerRelayState,
        }

        public CommandType Type { get; set; }
        public int ID { get; set; }
        public string RelayState { get; set; }
        public Chassis Chassis { get; set; }
    }
}
