﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Hardware.Motor
{
    internal class MotorCommand
    {
        /// <summary>
        /// Possible Motor Commands (taken from Motor model class)
        /// </summary>
        public enum CommandType
        {
            MoveAsync,
            MoveRelativeAsync,
            HomeAsync,
            GetVersionAsync,
            GetPositionAsync,
            CheckConnectionAsync,
            CheckIfHomingComplete,
        }

        public CommandType Type { get; set; }
        public int PositionParameter { get; set; }
        public int DistanceParameter { get; set; }
        public int SpeedParameter { get; set; }
        public Motor Motor { get; set; }
    }
}
