﻿using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class TEC
    {
        private int id = int.MinValue;
        private string name = string.Empty;
        private bool connected;
        private Configuration configuration;
        private readonly APIManager apiManager;
        private string actualObjectTemperature = "?";
        private string actualSinkTemperature = "?";
        private string targetObjectTemperature = "?";
        private string actualOutputCurrent = "?";
        private string actualOutputVoltage = "?";
        private string relativeCoolingPower = "?";
        private string actualFanSpeed = "?";
        private string currentErrorThreshold = "?";
        private string voltageErrorThreshold = "?";
        private string deviceStatus = "?";

        public TEC(int id, string name, APIManager apiManager, Configuration configuration)
        {
            this.apiManager = apiManager;
            this.configuration = configuration;
            this.id = id;
            this.name = name;
        }

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public bool Connected
        {
            get { return connected; }
        }

        public string ActualObjectTemperature
        {
            get { return actualObjectTemperature; }
        }

        public string ActualSinkTemperature
        {
            get { return actualSinkTemperature; }
        }

        public string TargetObjectTemperature
        {
            get { return targetObjectTemperature; }
        }

        public string ActualOutputCurrent
        {
            get { return actualOutputCurrent;  }
        }

        public string ActualOutputVoltage
        {
            get { return actualOutputVoltage; }
        }

        public string RelativeCoolingPower
        {
            get { return relativeCoolingPower; }
        }

        public string ActualFanSpeed
        {
            get { return actualFanSpeed; }
        }

        public string CurrentErrorThreshold
        {
            get { return currentErrorThreshold; }
        }

        public string VoltageErrorThreshold
        {
            get { return voltageErrorThreshold; }
        }

        public string DeviceStatus
        {
            get { return deviceStatus; }
        }

        public void CheckConnection()
        {
            Task.Run(async () => await CheckConnectionAsync()).Wait();
        }

        /// <summary>
        /// Check the connection of the TEC by testing a response
        /// </summary>
        /// <returns></returns>
        public async Task CheckConnectionAsync()
        {
            try
            {
                var responseValue = await GetBoardAddressAsync();
                await Task.Delay(500);
                if (responseValue != null && responseValue != 0)
                {
                    connected = true;
                }
                else
                {
                    connected = false;
                }
            }
            catch (Exception ex)
            {
                connected = false;
            }
        }

        /// <summary>
        /// Get the TEC board address
        /// </summary>
        /// <returns>address of the TEC</returns>
        public async Task<int?> GetBoardAddressAsync()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            var data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/device-address/?heater=Heater%20{name.Last()}");
            await Task.Delay(5);
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the Device Address from the response
            int? deviceAddress;
            try
            {
                deviceAddress = int.Parse(response);
            }
            catch (Exception ex)
            {
                // FIXME: Handle no response
                deviceAddress = null;
                MessageBox.Show($"Could not retreive Device Address for {name}, got response: {response}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return deviceAddress;
        }

        /// <summary>
        /// Get the Firmware version loaded onto the TEC board
        /// </summary>
        /// <returns>The firmware version as a string</returns>
        public async Task<string> GetVersionAsync()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data;
            try
            {
                data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/firmware-version/?heater=Heater%20{name.Last()}");
                await Task.Delay(5);
            }
            catch (Exception ex)
            {
                data = new APIResponse();
            }
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the Firmware Version from the response
            string firmwareVersion;
            firmwareVersion = response;
            if (firmwareVersion == string.Empty)
            {
                firmwareVersion = "?";
            }
            return firmwareVersion;
        }

        public void Reset()
        {
            // TODO: Implement code to reset the TEC
            MessageBox.Show("Code to reset the TEC has not been implmented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public async Task SetObjectTemperature(double temperature)
        {
            // Check the motor is connected
            await CheckConnectionAsync();
            await Task.Delay(500);
            if (connected)
            {
                if (true)
                {
                    // Generate a API Motor Request
                    APITECRequest apiTECRequest = new APITECRequest(name: name, value: temperature);
                    // Send the request to the API
                    await apiManager.PostAsync<APITECRequest, APIResponse>(
                        $"http://127.0.0.1:8000/tec/object-temperature/?heater=Heater%20{name.Last()}&setpoint={temperature}",
                        apiTECRequest);
                    await Task.Delay(5);
                }
            }
            targetObjectTemperature = temperature.ToString();
        }

        public async Task GetObjectTemperature()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/object-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            actualObjectTemperature = value;
        }

        /// <summary>
        /// Get the Sink Temperature for the TEC
        /// </summary>
        /// <returns>Sets the sink temperature variable for the TEC</returns>
        public async Task GetSinkTemperature()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/sink-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            actualSinkTemperature = value;
        }

        public async Task GetTargetObjectTemperature()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/target-object-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            targetObjectTemperature = value;
        }

        public async Task GetActualOutputCurrent()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-output-current/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            actualOutputCurrent = value;
        }

        public async Task GetActualOutputVoltage()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-output-voltage/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            actualOutputVoltage = value;
        }

        public async Task GetRelativeCoolingPower()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/relative-cooling-power/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            relativeCoolingPower = value;
        }

        public async Task GetActualFanSpeed()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-fan-speed/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            actualFanSpeed = value;
        }

        public async Task GetCurrentErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/current-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            currentErrorThreshold = value;
        }

        public async Task GetVoltageErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < configuration.TECWaitTimeoutInSeconds)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/voltage-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(500);
                    resp = data.Response?.Replace("\r", "");

                }
                catch (Exception ex)
                {
                    data = new APIResponse();
                }
            }
            stopwatch.Stop();
            // TODO: Check the submodule id and the module id
            // TODO: Replace this section with a class or method internal to APIResponse to check the APIResponse output
            #region
            int? sid = data.SubmoduleID;
            int? mid = data.ModuleID;
            int? duration_us = data.DurationInMicroSeconds;
            string? message = data.Message;
            string? response = data.Response?.Replace("\r", "");
            #endregion
            // Obtain the value from the response
            string value;
            value = response;
            if (value == string.Empty)
            {
                value = "?";
            }
            voltageErrorThreshold = value;
        }
    }
}
