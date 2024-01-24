using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Models.API;
using Independent_Reader_GUI.Services;
using Independent_Reader_GUI.Utilities;
using SpinnakerNET.GenApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Hardware.LED
{
    internal class LED
    {
        private string name = string.Empty;
        private int id = int.MinValue;
        public string IO = string.Empty;
        public bool connected;
        public int Intensity = int.MinValue;
        public int Exposure = int.MinValue;
        private Configuration configuration;
        private APIManager apiManager;
        // FIXME: Replace with a timeout from the configuration file;
        private static int timeout = 3000;
        private int msDelay = 50;

        public LED(string name, Configuration configuration, APIManager apiManager)
        {
            // TODO: pass an object of the LED board to this class for a connection
            this.apiManager = apiManager;
            this.configuration = configuration;
            this.name = name;
            SetIDFromName();
        }

        public static int Timeout
        {
            get { return timeout; }
        }

        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Get the connection string for a DataGridView 
        /// </summary>
        /// <returns>"C" for Connected and "N" for Not Connected</returns>
        public string GetState()
        {
            if (connected)
            {
                return "C";
            }
            else
            {
                return "N";
            }
        }

        private void SetIDFromName()
        {
            if (name.Equals("Cy5"))
            {
                id = configuration.Cy5ID;
            }
            else if (name.Equals("FAM"))
            {
                id = configuration.FAMID;
            }
            else if (name.Equals("HEX"))
            {
                id = configuration.HEXID;
            }
            else if (name.Equals("Atto"))
            {
                id = configuration.AttoID;
            }
            else if (name.Equals("Alexa"))
            {
                id = configuration.AlexaID;
            }
            else if (name.Equals("Cy5.5"))
            {
                id = configuration.Cy5p5ID;
            }
            else
            {
                MessageBox.Show(name + " is not a valid LED name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Check the connection of the LED board by testing a response
        /// </summary>
        /// <returns></returns>
        public async Task CheckConnectionAsync()
        {
            try
            {
                var responseValue = await GetVersionAsync();
                await Task.Delay(msDelay);
                if (responseValue != null && responseValue != "?")
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
        /// Get the Firmware version loaded onto the LED board
        /// </summary>
        /// <returns>The firmware version as a string</returns>
        public async Task<string> GetVersionAsync()
        {
            if (apiManager.Connected)
            {
                // TODO: Replace the endpoint with a private const from the configuration XML data file
                APIResponse data;
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/led/version/");
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
                // Obtain the Version from the response
                string version;
                version = response;
                if (version == string.Empty)
                {
                    version = "?";
                }
                return version;
            }
            else
            {
                Logger.LogWarning($"Unable to get {name} version because the API is not connected.");
                return "?";
            }
        }

        public async Task<int> GetIntensity()
        {
            if (apiManager.Connected)
            {
                // TODO: Replace the endpoint with a private const from the configuration XML data file
                APIResponse data;
                try
                {
                    data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/led/status/?channel={id}");
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
                // Obtain the intensity from the response
                int intensity = 0;
                if (response == string.Empty || response == null)
                {
                    intensity = 0;
                    Intensity = intensity;
                }
                else
                {
                    string val = response;
                    intensity = (int)double.Parse(val);
                    Intensity = intensity;
                }
                if (Intensity > 0)
                {
                    IO = "On";
                }
                else
                {
                    IO = "Off";
                }
                return intensity;
            }
            else
            {
                Logger.LogWarning($"Unable to get {name} intensity because the API is not connected.");
                return -1;
            }
        }

        public async Task On(int intensity)
        {
            if (apiManager.Connected)
            {
                if (connected)
                {
                    // TODO: Replace the endpoint with a private const from the configuration XML data file
                    APIResponse data = new APIResponse();
                    object? resp = null;
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
                    {
                        // Generate a API LED Request
                        APILEDRequest apiLEDRequest = new APILEDRequest(id: id, value: intensity);
                        // Send the request to the API
                        resp = await apiManager.PostAsync<APILEDRequest, APIResponse>(
                            $"http://127.0.0.1:8000/led/on/?channel={id}&intensity={intensity}",
                            apiLEDRequest);
                        await Task.Delay(msDelay);
                    }
                    stopwatch.Stop();
                    Intensity = intensity;
                    if (intensity > 0)
                    {
                        IO = "On";
                    }
                    else
                    {
                        IO = "Off";
                    }
                }
                else
                {
                    MessageBox.Show("Cannot turn on " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.LogWarning($"Unable to turn on {name} because the LED is not connected.");
                }
            }            
            else
            {
                Logger.LogWarning($"Unable to turn on {name} because the API is not connected.");
            }
        }

        public async Task Off()
        {
            if (apiManager.Connected)
            {
                if (connected)
                {
                    // TODO: Replace the endpoint with a private const from the configuration XML data file
                    APIResponse data = new APIResponse();
                    object? resp = null;
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (resp == null && stopwatch.Elapsed.TotalSeconds < timeout)
                    {
                        // Generate a API LED Request
                        APILEDRequest apiLEDRequest = new APILEDRequest(id: id, value: null);
                        // Send the request to the API
                        resp = await apiManager.PostAsync<APILEDRequest, APIResponse>(
                            $"http://127.0.0.1:8000/led/off/?channel={id}",
                            apiLEDRequest);
                        await Task.Delay(msDelay);
                    }
                    stopwatch.Stop();
                    Intensity = 0;
                    IO = "Off";
                }
                else
                {
                    MessageBox.Show("Cannot turn off " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.LogWarning($"Unable to turn off {name} because the LED is not connected.");
                }
            }
            else
            {
                Logger.LogWarning($"Unable to turn off {name} because the API is connected.");
            }
        }

        public string Name
        {
            get { return name; }
        }

        public int ID
        {
            get { return id; }
        }
    }
}
