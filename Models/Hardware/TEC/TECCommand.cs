using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Hardware.TEC
{
    internal class TECCommand
    {
        /// <summary>
        /// Possible TEC Commands (taken from TEC model class)
        /// </summary>
        public enum CommandType
        {
            CheckConnectionAsync,
            GetBoardAddressAsync,
            GetVersionAsync,
            ResetAsync,
            GetObjectTemperature,
            SetObjectTemperature,
            GetSinkTemperature,
            GetTargetObjectTemperature,
            GetActualOutputCurrent,
            GetActualOutputVoltage,
            GetRelativeCoolingPower,
            GetActualFanSpeed,
            GetCurrentErrorThreshold,
            GetVoltageErrorThreshold,
            GetObjectUpperErrorThreshold,
            GetObjectLowerErrorThreshold,
            GetSinkUpperErrorThreshold,
            GetSinkLowerErrorThreshold,
            GetTemperatureIsStable,
            GetTemperatureControl,
            SetTemperatureControl,
            GetFanControl,
            SetFanControl,
            GetFanTargetTemperature,
            SetFanTargetTemperature,
            GetDeviceStatus,
            GetErrorNumber,
            GetErrorDescription,
        }

        public CommandType Type { get; set; }
        public object Parameter { get; set; }
        public TEC TEC { get; set; }
    }
}
