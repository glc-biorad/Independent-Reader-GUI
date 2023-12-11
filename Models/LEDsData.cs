using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    /// <summary>
    /// LEDs Data is based on a property and each LED has a value 
    /// for this property.
    /// </summary>
    internal class LEDsData
    {
        public string PropertyName { get; set; } = "?";
        public string ValueCy5 { get; set; } = "?";
        public string ValueFAM { get; set; } = "?";
        public string ValueHEX { get; set; } = "?";
        public string ValueAtto { get; set; } = "?";
        public string ValueAlexa { get; set; } = "?";
        public string ValueCy5p5 { get; set; } = "?";
    }
}
