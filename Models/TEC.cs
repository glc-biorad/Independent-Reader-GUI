﻿using Independent_Reader_GUI.Services;
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

        public async Task CheckConnectionAsync()
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

        public async Task<string> GetVersionAsync()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            var data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/tec/firmware-version/?heater=Heater%20{name.Last()}");
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
            // Obtain the Firmware Version from the response
            string firmwareVersion;
            try
            {
                firmwareVersion = response;
            }
            catch (Exception ex)
            {
                // FIXME: Handle no response
                firmwareVersion = "?";
                MessageBox.Show($"Could not retreive Firmware Version for {name}, got response: {response}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
