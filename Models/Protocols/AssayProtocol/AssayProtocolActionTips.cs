using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Models.Consumables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.AssayProtocol
{
    internal class AssayProtocolActionTips : AssayProtocolAction
    {
        private static List<string> typeOptions = new List<string>
        {
            "Pickup",
            "Eject",
        };
        public string Tip;
        public string Tray;
        public int Column;

        /// <summary>
        /// Get the Type Options for the Assay Protocol Action
        /// </summary>
        public static List<string> TypeOptions { get { return typeOptions; } }

        public AssayProtocolActionTips()
        {
            //
        }

        public AssayProtocolActionTips(string type, string tip, string tray, int column, string description, bool displayDescription)
        {
            Type = type;
            Tip = tip;
            Tray = tray;
            Column = column;
            Description = description;
            DisplayDescription = displayDescription;
            ActionText = TranscribeActionText(this);
        }

        public AssayProtocolActionTips(string actionText)
        {
            // Parse the Action Text.
            AssayProtocolActionTips action = new AssayProtocolActionTips();
            action = ParseActionText(actionText);
            Type = action.Type;
            Tip = action.Tip;
            Tray = action.Tray;
            Column = action.Column;
            Description = action.Description;
            DisplayDescription = action.DisplayDescription;
            ActionText = action.ActionText;
        }

        /// <summary>
        /// Parse Action Text to obtain the AssayProtocolActionTips instance
        /// </summary>
        /// <param name="actionText"></param>
        /// <returns></returns>
        private AssayProtocolActionTips ParseActionText(string actionText)
        {
            AssayProtocolActionTips action = new AssayProtocolActionTips();
            Type = GetTypeFromActionText(actionText);
            Tip = GetTipFromActionText(actionText);
            Tray = GetTrayFromActionText(actionText);
            Column = int.Parse(GetColumnFromActionText(actionText));
            Description = "";
            DisplayDescription = false;
            return action;
        }

        /// <summary>
        /// Transcribe Action data into the Action Text
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string TranscribeActionText(AssayProtocolActionTips action)
        {
            string qualifier;
            if (action.Type == TypeOptions[0])
            {
                qualifier = "from";
            }
            else
            {
                qualifier = "into";
            }
            string actionText = $"{action.Type} {action.Tip} tips {qualifier} Tip Box tray {action.Tray} column {action.Column}";
            return actionText;
        }

        /// <summary>
        /// Get the Action Type from the provided Action Text.
        /// </summary>
        /// <param name="actionText">Provided Action Text</param>
        /// <returns></returns>
        /// <exception cref="AssayProtocolActionTypeNotFoundException"></exception>
        private string GetTypeFromActionText(string actionText)
        {
            foreach (string type in TypeOptions)
            {
                if (actionText.Contains(type))
                {
                    return type;
                }
            }
            throw new AssayProtocolActionParseException(actionText, typeOptions, typeof(AssayProtocolActionTips));
        }

        /// <summary>
        /// Get the Tip option from the Action Text
        /// </summary>
        /// <param name="actionText">Provided Action Text</param>
        /// <returns></returns>
        /// <exception cref="AssayProtocolActionParseException"></exception>
        private string GetTipFromActionText(string actionText)
        {
            foreach (string tip in TipOptions.Options)
            {
                if (actionText.Contains(tip))
                {
                    return tip;
                }
            }
            throw new AssayProtocolActionParseException(actionText, TipOptions.Options, typeof(AssayProtocolActionTips));
        }

        /// <summary>
        /// Get the Tray option from the Action Text
        /// </summary>
        /// <param name="actionText">Provided Action Text</param>
        /// <returns></returns>
        /// <exception cref="AssayProtocolActionParseException"></exception>
        private string GetTrayFromActionText(string actionText)
        {
            foreach (string tray in TrayOptions.Options)
            {
                if (actionText.Contains(tray))
                {
                    return tray;
                }
            }
            throw new AssayProtocolActionParseException(actionText, TrayOptions.Options, typeof(AssayProtocolActionTips));
        }

        /// <summary>
        /// Get the Column option from the Action Text
        /// </summary>
        /// <param name="actionText">Provided Action Text</param>
        /// <returns></returns>
        /// <exception cref="AssayProtocolActionParseException"></exception>
        private string GetColumnFromActionText(string actionText)
        {
            foreach (string column in ColumnOptions.Options)
            {
                if (actionText.Contains(column))
                {
                    return column;
                }
            }
            throw new AssayProtocolActionParseException(actionText, ColumnOptions.Options, typeof(AssayProtocolActionTips));
        }
    }
}
