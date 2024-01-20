using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.AssayProtocol
{
    internal class AssayProtocolActionTips
    {
        private static List<string> typeOptions = new List<string>
        {
            "Pickup",
            "Eject",
        };

        /// <summary>
        /// Get the Type Options for the Assay Protocol Action
        /// </summary>
        public static List<string> TypeOptions { get { return typeOptions; } }
    }
}
