using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class AssayProtocolAction
    {
        public string Name;
        public string Type;
        public int ID;
        public string ActionText;
        public string Description;

        private Dictionary<string, Type> dict = new Dictionary<string, Type>
        {
            // Assay Protocol Action HeaterShaker
            {"Set Temperature of the Heater/Shaker" , typeof(AssayProtocolActionHeaterShaker)},
            {"Set RPM of the Heater/Shaker" , typeof(AssayProtocolActionHeaterShaker)},
        };

        public void SetID(int id)
        {
            ID = id;
        }

        public AssayProtocolAction GetAssayProtocolActionFromActionText()
        {
            // Parse the Action Text
            foreach (KeyValuePair<string, Type> pair in dict)
            {
                if (ActionText.Contains(pair.Key))
                {
                    // Initialize the Action
                    if (pair.Value == typeof(AssayProtocolActionHeaterShaker))
                    {
                        // Initialize the Action
                        var action = new AssayProtocolActionHeaterShaker();
                        // Get the Type from the Action Text
                        action.Type = action.GetTypeFromActionText(ActionText);
                        // Get the Name from the Action Text
                        action.Name = AssayProtocolActionHeaterShaker.ActionName;
                        return action;
                    }
                }
            }
            throw new Exception($"Unable to get an Assay Protocol Action from {ActionText}");
        }

        public AssayProtocolAction GetAssayProtocolActionFromActionText(string actionText)
        {
            // Parse the Action Text
            foreach (KeyValuePair<string, Type> pair in dict) 
            {
                if (actionText.Contains(pair.Key))
                {
                    // Initialize the Action
                    if (pair.Value == typeof(AssayProtocolActionHeaterShaker))
                    {
                        // Initialize the Action
                        var action = new AssayProtocolActionHeaterShaker();
                        // Get the Type from the Action Text
                        action.Type = action.GetTypeFromActionText(actionText);
                        // Get the Name from the Action Text
                        action.Name = AssayProtocolActionHeaterShaker.ActionName;
                        return action;
                    }
                }
            }
            throw new Exception($"Unable to get an Assay Protocol Action from {actionText}");
        }
    }
}
