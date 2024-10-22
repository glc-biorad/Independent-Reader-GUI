﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class TipOptions
    {
        private const string noTips = "None";

        private static List<string> options = new List<string>
        {
            "1 mL",
            "200 \u03BCL",
            "50 \u03BCL",
            noTips,
        };

        public static string NoTips
        {
            get { return noTips; }
        }

        public static List<string> Options { get {  return options; } }
    }
}
