using Independent_Reader_GUI.Exceptions;
using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
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
        private int timeout = 10;
        private int msDelay = 50;
        public LED(string name, Configuration configuration, APIManager apiManager)
        {
            // TODO: pass an object of the LED board to this class for a connection
            this.apiManager = apiManager;
            this.configuration = configuration;
            this.name = name;
            SetIDFromName();
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

        public async Task On(int intensity)
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
                this.Intensity = intensity;
                this.IO = "On";
            }
            else
            {
                MessageBox.Show("Cannot turn on " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw new LEDNotConnectedException();
            }
        }

        public async Task Off()
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
                this.Intensity = 0;
                this.IO = "Off";
            }
            else
            {
                MessageBox.Show("Cannot turn off " + name + ", LEDs are not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw new LEDNotConnectedException();
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
