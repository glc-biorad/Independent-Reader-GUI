using Independent_Reader_GUI.Services;
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
        public string ParentModule = "Reader";
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
        private string temperatureControlled = "Off";
        private string fanControlled = "Disabled";
        private string objectUpperErrorThreshold = "?";
        private string objectLowerErrorThreshold = "?";
        private string sinkUpperErrorThreshold = "?";
        private string sinkLowerErrorThreshold = "?";
        private string temperatureIsStable = "?";
        private string fanTargetTemperature = "?";
        private int timeout;
        private int msDelay = 50;
        private double previousObjectTemperature = double.MinValue;
        private string previousFanControl = "Disabled";
        public bool RunningProtocol = false;
        public List<Tuple<DateTime, double>> ObjectTemperatures = new List<Tuple<DateTime, double>>();
        public List<Tuple<DateTime, double>> SinkTemperatures = new List<Tuple<DateTime, double>>();
        public List<Tuple<DateTime, double>> TargetTemperatures = new List<Tuple<DateTime, double>>();
        public List<Tuple<DateTime, double>> FanSpeeds = new List<Tuple<DateTime, double>>();
        public string ErrorMessage = "";
        public int ErrorNumber = 0;
        public string ErrorDescription = "No Error";
        public bool InErrorState = false;

        public TEC(int id, string name, APIManager apiManager, Configuration configuration)
        {
            this.apiManager = apiManager;
            this.configuration = configuration;
            this.id = id;
            this.name = name;
            timeout = configuration.TECWaitTimeoutInSeconds;
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

        public string ObjectUpperErrorThreshold
        {
            get { return objectUpperErrorThreshold;  }
        }

        public string ObjectLowerErrorThreshold
        {
            get { return objectLowerErrorThreshold; }
        }

        public string SinkUpperErrorThreshold
        {
            get { return sinkUpperErrorThreshold; }
        }

        public string SinkLowerErrorThreshold
        {
            get { return sinkLowerErrorThreshold; }
        }

        public string TemperatureIsStable
        {
            get { return temperatureIsStable; }
        }

        public string TemperatureControlled
        {
            get { return temperatureControlled; }
        }

        public double PreviousObjectTemperature
        {
            get { return previousObjectTemperature; }
        }

        public string FanControlled
        {
            get { return fanControlled; }
        }

        public string FanTargetTemperature
        {
            get { return fanTargetTemperature; }
        }

        /// <summary>
        /// Determine if the Fan Control has changed.
        /// </summary>
        /// <returns></returns>
        public bool FanControlChanged()
        {
            if (fanControlled != previousFanControl)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                await Task.Delay(msDelay);
                if (responseValue != null && responseValue != 0)
                {
                    connected = true;
                }
                else
                {
                    Debug.WriteLine($"{name}: CheckConnectionAsync -> response = {responseValue}");
                    connected = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{name}: CheckConnectionAsync -> {ex.Message}");
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
            await Task.Delay(msDelay);
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
                Debug.WriteLine($"{name}: GetBoardAddressAsync -> {ex.Message}");
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
                await Task.Delay(msDelay);
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

        public async Task ResetAsync()
        {
            // Assume motor is connected
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            object? resp = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                // Generate a API Motor Request
                APITECRequest apiTECRequest = new APITECRequest(name: name);
                // Send the request to the API
                resp = await apiManager.PostAsync<APITECRequest, APIResponse>(
                    $"http://127.0.0.1:8000/tec/meerstetter/reset/?heater=Heater%20{name.Last()}",
                    apiTECRequest);
                await Task.Delay(msDelay);
            }
            stopwatch.Stop();
        }

        /// <summary>
        /// Set the timeout for reattempting failed set or get requests
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        public void SetTimeout(int timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Set Object Temperature of the TEC
        /// </summary>
        /// <param name="temperature">Temperature to set the TEC to</param>
        /// <param name="attemptTimeout">Timeout to reattempt failed set requests</param>
        /// <returns></returns>
        public async Task SetObjectTemperature(double temperature)
        {
            // Assume motor is connected
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            object? resp = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                // Generate a API Motor Request
                APITECRequest apiTECRequest = new APITECRequest(name: name, value: temperature);
                // Send the request to the API
                resp = await apiManager.PostAsync<APITECRequest, APIResponse>(
                    $"http://127.0.0.1:8000/tec/object-temperature/?heater=Heater%20{name.Last()}&setpoint={temperature}",
                    apiTECRequest);
                await Task.Delay(msDelay);
            }
            stopwatch.Stop();            
            targetObjectTemperature = temperature.ToString();
        }

        public async Task<int> GetObjectTemperature()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int error = -1;
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/object-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
                    resp = data.Response?.Replace("\r", "");
                    error = 0;
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
            // Set the previous temperature
            try
            {
                previousObjectTemperature = double.Parse(actualObjectTemperature);
            }
            catch
            {
                previousObjectTemperature = double.MinValue;
            }
            actualObjectTemperature = value;
            return error;
        }

        public async Task SetFanTargetTemperature(double temperature)
        {
            // Assume motor is connected
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            object? resp = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                // Generate a API Motor Request
                APITECRequest apiTECRequest = new APITECRequest(name: name, value: temperature);
                // Send the request to the API
                resp = await apiManager.PostAsync<APITECRequest, APIResponse>(
                    $"http://127.0.0.1:8000/tec/target-fan-temperature/?heater=Heater%20{name.Last()}&setpoint={temperature}",
                    apiTECRequest);
                await Task.Delay(msDelay);
            }
            stopwatch.Stop();
            fanTargetTemperature = temperature.ToString();
        }

        public async Task<int> GetFanTargetTemperature()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int error = -1;
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/target-fan-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
                    resp = data.Response?.Replace("\r", "");
                    error = 0;
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
            fanTargetTemperature = value;
            return error;
        }

        public async Task SetFanControl(string status)
        {
            // Assume motor is connected
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            object? resp = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                // Generate a API Motor Request
                APITECRequest apiTECRequest = new APITECRequest(name: name, strValue: status);
                // Send the request to the API
                resp = await apiManager.PostAsync<APITECRequest, APIResponse>(
                    $"http://127.0.0.1:8000/tec/fan-control/?heater=Heater%20{name.Last()}&status={status}",
                    apiTECRequest);
                await Task.Delay(msDelay);
            }
            stopwatch.Stop();
            previousFanControl = fanControlled;
            fanControlled = status;
        }

        public async Task GetFanControl()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/fan-control/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            if (value == "0")
            {
                value = "Disabled";
            }
            else if (value == "1")
            {
                value = "Enabled";
            }
            else
            {
                value = "?";
            }
            previousFanControl = fanControlled;
            fanControlled = value;
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/sink-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/target-object-temperature/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-output-current/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-output-voltage/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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

        public async Task GetDeviceStatus()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/device-status/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            if (value == "Error")
            {
                ErrorMessage = "Error";
                InErrorState = true;
                
            }
            else
            {
                InErrorState = false;
            }
            deviceStatus = value;
        }

        /// <summary>
        /// Get the Error Number on the TEC
        /// </summary>
        /// <returns></returns>
        public async Task GetErrorNumber()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/error-number/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            int value;
            if (int.TryParse(response, out _))
            {
                value = int.Parse(response);
            }
            else
            {
                value = -1;
            }
            ErrorNumber = value;
        }

        /// <summary>
        /// Get the Error Description on the TEC
        /// </summary>
        /// <returns></returns>
        public async Task GetErrorDescription()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/error-description/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            if (response == null)
            {
                value = "?";
            }
            else
            {
                value = response;
            }
            ErrorDescription = value;
        }

        public async Task GetRelativeCoolingPower()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/relative-cooling-power/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/actual-fan-speed/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/current-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
                    resp = data.Response?.Replace("\r", "");
                    Debug.WriteLine(data.Response?.Replace("\r", ""));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{name}: GetCurrentErrorThreshold -> {ex.Message}");
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
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/voltage-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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

        public async Task GetObjectUpperErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/object-upper-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            objectUpperErrorThreshold = value;
        }

        public async Task GetObjectLowerErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/object-lower-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            objectLowerErrorThreshold = value;
        }

        public async Task GetSinkUpperErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/sink-upper-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            sinkUpperErrorThreshold = value;
        }

        public async Task GetSinkLowerErrorThreshold()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/sink-lower-error-threshold/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            sinkLowerErrorThreshold = value;
        }

        public async Task GetTemperatureIsStable()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/temperature-is-stable/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            temperatureIsStable = value;
        }

        public async Task GetTemperatureControl()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/temperature-control/?heater=Heater%20{name.Last()}");
                    await Task.Delay(msDelay);
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
            temperatureControlled = value;
        }

        public async Task SetTemperatureControl(int state)
        {
            // Assume tec is connected
            // Convert the state (0 or 1) to the status (Off or On)
            string status = (state == 0) ? "Off" : "On";
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            APIResponse data = new APIResponse();
            string resp = "None";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (resp == "None" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                // Generate a API Motor Request
                APITECRequest apiTECRequest = new APITECRequest(name: name, value: state);
                // Send the request to the API
                await apiManager.PostAsync<APITECRequest, APIResponse>(
                    $"http://127.0.0.1:8000/tec/temperature-control/?heater=Heater%20{name.Last()}&status={status}",
                    apiTECRequest);
                await Task.Delay(msDelay);
            }
            stopwatch.Stop();
            temperatureControlled = status;
        }
    }
}
