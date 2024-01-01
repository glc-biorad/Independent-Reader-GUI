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

        /// <summary>
        /// Convert a time given the units to microseconds
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeUnits"></param>
        /// <returns></returns>
        public int ConvertTimeToMilliseconds(int time, string timeUnits)
        {
            if (timeUnits == Microseconds)
            {
                return time / 1000;
            }
            else if (timeUnits == Milliseconds)
            {
                return time;
            }
            else if (timeUnits == Seconds)
            {
                return time * 1000;
            }
            else if (timeUnits == Minutes)
            {
                return time * 60 * 1000;
            }
            else if (timeUnits == Hours)
            {
                return time * 60 * 60 * 1000;
            }
            return -1;
        }
    }
}
