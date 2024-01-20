using Independent_Reader_GUI.Models.API;
using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models.Hardware.Chassis
{
    internal class Chassis
    {
        private readonly APIManager apiManager;
        private string version;
        private int msDelay = 50;

        public Chassis(APIManager apiManager)
        {
            this.apiManager = apiManager;
        }

        /// <summary>
        /// Set the Power Relay State
        /// </summary>
        /// <param name="id">ID for the Power Relay</param>
        /// <param name="state">State of the relay (true for on, false for off)</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SetPowerRelayStatus(int id, string state)
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            try
            {
                // Generate a API Chassis Request
                APIChassisRequest apiChassisRequest = new APIChassisRequest(id: id, value: state);
                // Send the request to the API
                await apiManager.PostAsync<APIChassisRequest, APIResponse>(
                    $"http://127.0.0.1:8000/chassis/relay/{id}?state={apiChassisRequest.value}",
                    apiChassisRequest);
                await Task.Delay(msDelay);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to set Power Relay to {state} for Power Relay {id} due to {ex.Message}.");
            }
        }
    }
}
