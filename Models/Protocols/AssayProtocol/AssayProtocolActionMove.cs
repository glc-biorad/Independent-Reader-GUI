using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Models.Consumables;
using iText.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.AssayProtocol
{
    internal class AssayProtocolActionMove : AssayProtocolAction
    {
        private static List<string> typeOptions = new List<string>
        {
            "Known",
            "Named",
            "Absolute",
            "Relative",
        };
        string Tip;
        string CoordinateName;
        string ConsumableName;
        string Tray;
        int Column;
        int X;
        int Y;
        int Z;
        bool UseDripPlate;

        public static List<string> TypeOptions
        {
            get { return typeOptions; }
        }

        public AssayProtocolActionMove()
        {
            //
        }

        public AssayProtocolActionMove(string type, string tip, string coordinateName, string consumableName, string tray, int column,
            int x, int y, int z, bool useDripPlate, string description, bool displayDescription)
        {
            Type = type;
            Tip = tip;
            CoordinateName = coordinateName;
            ConsumableName = consumableName;
            Tray = tray;
            Column = column;
            X = x;
            Y = y;
            Z = z;
            UseDripPlate = useDripPlate;
            Description = description;
            DisplayDescription = displayDescription;
        }

        public AssayProtocolActionMove(string actionText)
        {
            // Parse the Action Text
            AssayProtocolActionMove action = ParseActionText(actionText);
            Type = action.Type;
            Tip = action.Tip;
            CoordinateName = action.CoordinateName;
            ConsumableName = action.ConsumableName;
            Tray = action.Tray;
            Column = action.Column;
            X = action.X; 
            Y = action.Y; 
            Z = action.Z;
            UseDripPlate = action.UseDripPlate;
            Description = action.Description;
            DisplayDescription = action.DisplayDescription;
        }

        public string TranscribeActionText(AssayProtocolActionMove action)
        {
            // Transcribe the Action Text from the Action
            string actionText = $"{action.Type} move to ";
            if (action.Type == typeOptions[0])
            {
                actionText = TranscribeKnownMoveActionText(action.ConsumableName, action.Tray, action.Column, action.Tip, action.UseDripPlate);
            }
            else if (action.Type == typeOptions[1])
            {
                actionText = TranscribeNamedMoveActionText(action.CoordinateName, action.Tip, action.UseDripPlate);
            }
            else if (action.Type == typeOptions[2])
            {
                actionText = TranscribeAbsoluteMoveActionText(action.X, action.Y, action.Z, action.Tip, action.UseDripPlate);
            }
            else if (action.Type == typeOptions[3])
            {
                actionText = TranscribeRelativeMoveActionText(action.X, action.Y, action.Z);
            }
            string d = $"Relative move to the left by {X}";
            return actionText;
        }

        /// <summary>
        /// Transcibe the Action Text for a Known Move type of Assay Protocol Action Tips action
        /// </summary>
        /// <param name="consumableName"></param>
        /// <param name="tray"></param>
        /// <param name="column"></param>
        /// <param name="tip"></param>
        /// <param name="useDripPlate"></param>
        /// <returns></returns>
        private string TranscribeKnownMoveActionText(string consumableName, string tray, int column, string tip, bool useDripPlate)
        {
            string actionText = $"Known move to {consumableName}";
            // Add the tray
            if (TrayOptions.Options.Contains(tray))
            {
                actionText += $" tray {tray}";
            }
            // Add the column 
            if (ColumnOptions.Options.Contains(column.ToString()))
            {
                actionText += $" column {column}";
            }
            // Add the tip
            if (tip == TipOptions.NoTips)
            {
                actionText += $" without tips";
            }
            else
            {
                actionText += $" with {tip} tips";
            }
            // Add the drip plate
            if (useDripPlate)
            {
                actionText += $" using drip plate";
            }
            return actionText;
        }

        private string TranscribeNamedMoveActionText(string coordinateName, string tip, bool useDripPlate)
        {
            string actionText = $"Named move to {coordinateName}";
            // Add Tips
            if (tip == TipOptions.NoTips)
            {
                actionText += " without tips";
            }
            else
            {
                actionText += $" with {tip} tips";
            }
            // Add drip plate
            if (useDripPlate)
            {
                actionText += " using the drip plate";
            }
            return actionText;
        }

        private string TranscribeAbsoluteMoveActionText(int X, int Y, int Z, string tip, bool useDripPlate)
        {
            string actionText = $"Absolute move to {X}, {Y}, {Z}";
            // Add tips
            if (tip == TipOptions.NoTips)
            {
                actionText += " without tips";
            }
            else
            {
                actionText += $" with {tip} tips";
            }
            // Add drip plate
            if (useDripPlate)
            {
                actionText += " using the drip plate";
            }
            return actionText;
        }

        private string TranscribeRelativeMoveActionText(int X, int Y, int Z)
        {
            string actionText = "Relative move to the";
            // Add X
            if (X > 0)
            {
                actionText += $" left by {X}";
            }
            else if (X < 0)
            {
                actionText += $" right by {X}";
            }
            // Add Y
            if (Y > 0)
            {
                actionText += $" forward by {Y}";
            }
            else if (Y  < 0)
            {
                actionText += $" backward by {Y}";
            }
            // Add Z
            if (Z > 0)
            {
                actionText += $" up by {Z}";
            }
            else if (Z < 0)
            {
                actionText += $" down by {Z}";
            }
            return actionText;
        }

        private AssayProtocolActionMove ParseActionText(string actionText)
        {            
            AssayProtocolActionMove action = new AssayProtocolActionMove();
            // Get the Type of Action
            action.Type = GetTypeFromActionText(actionText);
            if (action.Type == typeOptions[0])
            {
                action = ParseKnownActionText(actionText);
            }
            else if (action.Type == typeOptions[1])
            {
                action = ParseNamedActionText(actionText);
            }
            else if (action.Type == typeOptions[2])
            {
                action = ParseAbsoluteActionText(actionText);
            }
            else if (action.Type == typeOptions[3])
            {
                action = ParseRelativeActionText(actionText);
            }
            else
            {
                throw new AssayProtocolActionParseException(actionText, typeOptions, typeof(AssayProtocolActionMove));
            }
            return action;
        }

        private AssayProtocolActionMove ParseKnownActionText(string actionText)
        {
            AssayProtocolActionMove action = new AssayProtocolActionMove();
            action.Type = GetTypeFromActionText(actionText);
            action.ConsumableName = "?";
            // Get the Consumable Name
            foreach (string option in DeckConsumableOptions.Options)
            {
                if (actionText.Contains(option))
                {
                    action.ConsumableName = option;
                }
            }
            if (action.ConsumableName == "?")
            {
                throw new AssayProtocolActionParseException(actionText, DeckConsumableOptions.Options, typeof(AssayProtocolActionMove));
            }
            // Get the Tray
            action.Tray = GetTrayFromActionText(actionText, action.ConsumableName);
            // Get the Column
            action.Column = GetColumnFromActionText(actionText, action.ConsumableName);
            // Get the Tip
            action.Tip = GetTipFromActionText(actionText);
            // Get the UseDripPlate
            if (actionText.Contains("drip plate"))
            {
                action.UseDripPlate = true;
            }
            else
            {
                action.UseDripPlate = false;
            }
            return action;
        }

        private AssayProtocolActionMove ParseNamedActionText(string actionText)
        {
            AssayProtocolActionMove action = new AssayProtocolActionMove();
            return action;
        }

        private AssayProtocolActionMove ParseAbsoluteActionText(string actionText)
        {
            AssayProtocolActionMove action = new AssayProtocolActionMove();
            action.Type = GetTypeFromActionText(actionText);
            action.Tip = GetTipFromActionText(actionText);
            string[] splitActionText = actionText.Split(",");
            action.X = int.Parse(splitActionText[0].Split(" ").Last());
            action.Y = int.Parse(splitActionText[1]);
            action.Z = int.Parse(splitActionText[2].Split(" ").First());
            return action;
        }

        private AssayProtocolActionMove ParseRelativeActionText(string actionText)
        {
            AssayProtocolActionMove action = new AssayProtocolActionMove();
            return action;
        }

        private string GetTrayFromActionText(string actionText, string consumableName)
        {
            string tray = "?";
            foreach (string option in DeckConsumableOptions.GetTrayOptions(consumableName))
            {
                if (actionText.Contains($"tray {option}"))
                {
                    tray = option;
                }
            }
            if (tray == "?")
            {
                throw new AssayProtocolActionParseException(actionText, DeckConsumableOptions.GetTrayOptions(consumableName), typeof(AssayProtocolActionMove));
            }
            return tray;
        }

        private int GetColumnFromActionText(string actionText, string consumableName)
        {
            int column = 0;
            foreach (string option in DeckConsumableOptions.GetColumnOptions(consumableName))
            {
                if (actionText.Contains($"column {option}"))
                {
                    column = int.Parse(option);
                }
            }
            return column;
        }

        public string GetTypeFromActionText(string actionText)
        {
            foreach (string option in typeOptions)
            {
                if (actionText.Contains(option))
                {
                    return option;
                }
            }
            throw new AssayProtocolActionParseException(actionText, typeOptions, typeof(AssayProtocolActionMove));
        }

        public string GetTipFromActionText(string actionText)
        {
            foreach (string option in TipOptions.Options) 
            {
                if (actionText.Contains(option))
                {
                    return option;
                }
            }
            if (actionText.Contains("without"))
            {
                return TipOptions.NoTips;
            }
            throw new AssayProtocolActionParseException(actionText, TipOptions.Options, typeof(AssayProtocolActionMove));
        }

    }
}
