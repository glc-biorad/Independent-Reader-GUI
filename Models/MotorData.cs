using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    /// <summary>
    /// Motor Data with all defaults set to "?" to represent
    /// a disconnected motor.
    /// </summary>
    internal class MotorData
    {
        public string Name { get; set; } = "?";
        public string IO { get; set; } = "?";
        public string State { get; set; } = "?";
        public string Steps { get; set; } = "?";
        public string Speed { get; set; } = "?";
        public string Home { get; set; } = "?";
    }
}
