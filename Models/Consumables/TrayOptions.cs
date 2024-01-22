using AForge.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class TrayOptions
    {
        private static List<string> options = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
        };

        private static List<string> readerOptions = new List<string>
        {
            "AB",
            "CD",
        };

        private static List<string> noOptions = new List<string>
        {
            "",
        };

        public static List<string> ReaderOptions { get { return readerOptions; } }
        public static List<string> NoOptions { get { return noOptions; } }

        public static List<string> Options { get { return options; } }
    }
}
