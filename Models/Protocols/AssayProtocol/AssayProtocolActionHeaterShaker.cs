using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.AssayProtocol
{
    internal class AssayProtocolActionHeaterShaker : AssayProtocolAction
    {
        public const string ActionName = "Heater/Shaker";
        public double? Temperature;
        public double? RPM;
        private readonly List<string> Types = new List<string>
        {
            "Set Temperature",
            "Set RPM",
        };

        public AssayProtocolActionHeaterShaker()
        {
            Name = ActionName;
        }

        public AssayProtocolActionHeaterShaker(string type, double? temperature = null, double? rpm = null, string description = "")
        {
            Name = ActionName;
            Type = type;
            if (Type == Types[0])
            {
                Temperature = temperature;
            }
            else if (Type == Types[1])
            {
                RPM = rpm;
            }
            // Get the Action Text
            GetActionText();
            // Set the Description of the Action
            Description = description;
        }

        private void GetActionText()
        {
            // Determine the units based on the type of action
            string unit;
            double? parameter;
            if (Type == Types[0])
            {
                unit = " \u03BCC";
                parameter = Temperature;
            }
            else
            {
                unit = "";
                parameter = RPM;
            }
            if (parameter == null)
            {
                throw new Exception($"Unable to handle {Name} action of type {Type} due to null parameter");
            }
            ActionText = $"{Type} of the Heater/Shaker to {parameter}{unit}";
        }

        /// <summary>
        /// Get the Action Type from the Action Text
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTypeFromActionText()
        {
            if (ActionText.Contains(Types[0]))
            {
                return Types[0];
            }
            else if (ActionText.Contains(Types[1]))
            {
                return Types[1];
            }
            else
            {
                throw new Exception($"Unable to get the type of action from action text ({ActionText})");
            }
        }

        /// <summary>
        /// Get the Action Type from the Action Text
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTypeFromActionText(string actionText)
        {
            ActionText = actionText;
            if (ActionText.Contains(Types[0]))
            {
                return Types[0];
            }
            else if (ActionText.Contains(Types[1]))
            {
                return Types[1];
            }
            else
            {
                throw new Exception($"Unable to get the type of action from action text ({ActionText})");
            }
        }
    }
}
