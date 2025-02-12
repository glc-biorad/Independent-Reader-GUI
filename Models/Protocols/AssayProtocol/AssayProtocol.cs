﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Protocols.AssayProtocol
{
    internal class AssayProtocol
    {
        public string Name;
        public string Version;
        public string Author;
        public DateTime DateCreated;
        public string Date;
        public string Time;
        public List<AssayProtocolAction> Actions;

        public AssayProtocol()
        {
            Actions = new List<AssayProtocolAction>();
        }

        public AssayProtocol(string name, string version, string author, List<AssayProtocolAction> actions)
        {
            DateCreated = DateTime.Now;
            Date = DateCreated.ToString("MM/dd/yyyy");
            Time = DateCreated.ToString("hh:mm:ss tt");
            Author = author;
            Name = name;
            Version = version; // automated versioning handled at the UI
            Actions = actions;
        }

        /// <summary>
        /// Set the Name of the Assay Protocol
        /// </summary>
        /// <param name="name">Name of the Assay Protocol</param>
        public void SetName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add an Assay Protocol Action to the Action list
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(AssayProtocolAction action)
        {
            Actions.Add(action);
        }
    }
}
