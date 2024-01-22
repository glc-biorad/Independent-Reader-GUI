using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class ColumnOptions
    {
        private static List<string> options = new List<string>
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
        };

        private static List<string> noOptions = new List<string>
        {
            "",
        };

        private static List<string> oneOption = new List<string>
        {
            "1",
        };

        private static List<string> threeOptions = new List<string>
        {
            "1",
            "2",
            "3",
        };

        private static List<string> fourOptions = new List<string>
        {
            "1",
            "2",
            "3",
            "4",
        };

        private static List<string> eightOptions = new List<string>
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
        };

        public static List<string> NoOptions {  get { return noOptions; } }
        public static List<string> OneOption { get { return oneOption; } }
        public static List<string> ThreeOptions { get { return threeOptions; } }
        public static List<string> FourOptions {  get { return fourOptions; } }
        public static List<string> EightOptions { get { return eightOptions; } }

        public static List<string> Options { get {  return options; } }

        /// <summary>
        /// Get the first N options
        /// </summary>
        /// <param name="N">Number of options to take</param>
        /// <returns></returns>
        public static List<string> GetNOptions(int N)
        {
            return options.Take(N).ToList();
        }
    }
}
