using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Resources
{
    internal class TimeUnit
    {
        public const string Default = "seconds";
        public const string Seconds = "seconds";
        public const string Minutes = "minutes";
        public const string Hours = "hours";
        public const string Milliseconds = "milliseconds";
        public const string Microseconds = "\u03BCs";

        public double ConvertMicrosecondsToSeconds(double microseconds)
        {
            double seconds = microseconds / 1000000;
            return seconds;
        }
    }
}
