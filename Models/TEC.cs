using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
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
        private readonly APIManager apiManager;

        public TEC(int id, string name, APIManager apiManager)
        {
            this.apiManager = apiManager;
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
                await Task.Delay(5);
                if (responseValue != null)
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

        public void SetTemp()
        {
            // TODO: Implement code to set temp for the TEC
            MessageBox.Show("Code to set the temp for the TEC has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public double GetTemp()
        {
            double temp = double.NaN;
            // TODO: Implement code to get temp for the TEC
            MessageBox.Show("Code to get the temp for the TEC has not been implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return temp;
        }

    }
}
