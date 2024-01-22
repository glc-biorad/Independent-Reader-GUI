using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Consumables
{
    internal class DeckConsumableOptions
    {
        private static List<string> options = new List<string>
        {
            "Reagent Cartridge",
            "Assay Strip",
            "Tip Box",
            "Reader Tray",
            "DNA Quant Strip",
            "32 Deep Well",
            "RNA Heater",
            "Chiller",
            "Mag Separator",
            "Pre-Amp Thermocycler",
            "Tip Transfer Tray",
            "Lid Tray",
            "Droplet Generation Station",
            "Sample Loading",
        };

        private static List<string> optionsWithTrays = new List<string>
        {
            "Reagent Cartridge",
            "Assay Strip",
            "Tip Box",
            "DNA Quant Strip",
            "RNA Heater",
            "Droplet Generation Station",
            "Sample Loading",
        };

        private static List<string> optionsWithReaderTrays = new List<string>
        {
            "Reader Tray",
        };

        private static List<string> optionsWith1Column = new List<string>
        {
            "Assay Strips",
            "RNA Heater",
            "Sample Loading",
        };

        private static List<string> optionsWithNoColumns = new List<string>
        {
            "Lid Tray",
        };

        private static List<string> optionsWith3Columns = new List<string>
        {
            "Droplet Generation Station",
            "Reading Tray",
        };

        private static List<string> optionsWith4Columns = new List<string>
        {
            "32 Deep Well",
        };

        private static List<string> optionsWith8Columns = new List<string>
        {
            "Tip Transfer Tray",
        };

        private static List<string> optionsWith12Columns = new List<string>
        {
            "Reagent Cartridge",
            "Pre-Amp Thermocycler",
            "Tip Box",
            "Chiller",
            "Mag Separator",
        };

        public static List<string> Options { get { return options; } }

        public static List<string> GetTrayOptions(string consumableName)
        {
            if (optionsWithTrays.Contains(consumableName))
            {
                return TrayOptions.Options;
            }
            else if (optionsWithReaderTrays.Contains(consumableName))
            {
                return TrayOptions.ReaderOptions;
            }
            else
            {
                return TrayOptions.NoOptions;
            }
        }

        public static List<string> GetColumnOptions(string consumableName)
        {
            if (optionsWithNoColumns.Contains(consumableName))
            {
                return ColumnOptions.NoOptions;
            }
            else if (optionsWith1Column.Contains(consumableName))
            {
                return ColumnOptions.OneOption;
            }
            else if (optionsWith3Columns.Contains(consumableName))
            {
                return ColumnOptions.ThreeOptions;
            }
            else if (optionsWith4Columns.Contains(consumableName))
            {
                return ColumnOptions.FourOptions;
            }
            else if (optionsWith8Columns.Contains(consumableName))
            {
                return ColumnOptions.EightOptions;
            }
            else
            {
                return ColumnOptions.Options;
            }
        }
    }
}
