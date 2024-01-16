using Independent_Reader_GUI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class Motor
    {
        private readonly APIManager apiManager;
        private string name = string.Empty;
        private int address = int.MinValue;
        public string IO = string.Empty;
        private bool connected = false;
        private bool homed = false;
        private int position = int.MinValue;
        private int speed = int.MinValue;
        private TimeSpan waitTimeSpan;
        private const int checkInTimeInMilliseconds = 200;
        private const int msDelay = 50;
        private string version = "?";

        public Motor(string name, int address, APIManager apiManager, double timeoutInMilliseconds = 3000) 
        {
            this.name = name;
            this.address = address;
            this.waitTimeSpan = TimeSpan.FromMilliseconds(timeoutInMilliseconds);
            this.apiManager = apiManager;
        }

        public async Task MoveAsync(int positionInMicrosteps, int speedInMicrostepsPerSecond)
        {
            // Check the motor is connected
            //await CheckConnectionAsync();
            //await Task.Delay(5);
            if (connected)
            {
                //if (homed)
                if (true)
                {
                    // Generate a API Motor Request
                    APIMotorRequest apiMotorRequest = new APIMotorRequest(id: address, positionInMicrosteps, speedInMicrostepsPerSecond);
                    // Send the request to the API
                    await apiManager.PostAsync<APIMotorRequest, APIResponse>(
                        $"http://127.0.0.1:8000/reader/axis/move/{address}?position=-{apiMotorRequest.postion}&velocity={apiMotorRequest.velocity}", 
                        apiMotorRequest);
                    await Task.Delay(msDelay);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (!homed && (stopwatch.Elapsed < waitTimeSpan))
                    {
                        // Check in every so often
                        await Task.Delay(checkInTimeInMilliseconds);
                    }
                    position = positionInMicrosteps;
                }
            }
        }

        public async Task MoveRelativeAsync(int distanceInMicrosteps, int speedInMicrostepsPerSecond)
        {
            if (connected)
            {
                //if (homed)
                if (true)
                {
                    // Generate a API Motor Request
                    APIMotorRequest apiMotorRequest = new APIMotorRequest(id: address, distance: distanceInMicrosteps, velocity: speedInMicrostepsPerSecond);
                    // Send the request to the API
                    await apiManager.PostAsync<APIMotorRequest, APIResponse>(
                        $"http://127.0.0.1:8000/reader/axis/jog/{address}?distance={apiMotorRequest.distance}&velocity={apiMotorRequest.velocity}",
                        apiMotorRequest);
                    await Task.Delay(msDelay);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (!homed && (stopwatch.Elapsed < waitTimeSpan))
                    {
                        // Check in every so often
                        await Task.Delay(checkInTimeInMilliseconds);
                    }
                    position += distanceInMicrosteps;
                }
            }
        }

        public async Task HomeAsync()
        {
            // Check the motor is connected
            //await CheckConnectionAsync();
            //await Task.Delay(5);
            if (connected)
            {
                // Generate a API Motor Request
                APIMotorRequest apiMotorRequest = new APIMotorRequest(id: address);
                // Send the request to the API
                await apiManager.PostAsync<APIMotorRequest, APIResponse>($"http://127.0.0.1:8000/reader/axis/home/{address}", apiMotorRequest);
                await Task.Delay(msDelay);
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (!homed && (stopwatch.Elapsed < waitTimeSpan))
                {
                    // TODO: Check if the motor has been homed
                    var position = await GetPositionAsync();
                    await Task.Delay(msDelay);
                    if (position != null)
                    {
                        if (position == 0)
                        {
                            homed = true;
                        }                        
                        else
                        {
                            homed = false;
                        }
                    }
                    else if (position == null)
                    {
                        // TODO: Replace with something better (a custom exception MotorResponseException?)
                        //throw new ApplicationException($"Error in homing the {name} motor, recieved a null response.");
                        MessageBox.Show($"Failed to home {name} motor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Check in every so often
                    await Task.Delay(checkInTimeInMilliseconds);
                }
            }
        }

        public async Task<string?> GetVersionAsync()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            try
            {
                var data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/reader/axis/version/{address}");
                await Task.Delay(msDelay);
                version = data.Response;
                return data.Response;
            }
            catch (Exception ex)
            {
                version = "?";
                return "?";
            }
        }

        public async Task<int> GetPositionAsync()
        {
            // TODO: Replace the endpoint with a private const from the configuration XML data file
            var data = await apiManager.GetAsync<APIResponse>($"http://127.0.0.1:8000/reader/axis/position/{address}");
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
            // Obtain the position from the response
            int position;
            try
            {
                position = int.Parse(response.Split(",").Last());
            }
            catch (Exception ex)
            {
                // FIXME: Handle null position
                position = int.MinValue;
                //MessageBox.Show($"Get position for {name} motor was unsuccessful", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.position = position;
            return position;
        }

        public int Position
        {
            get { return position;  }
        }

        public void CheckConnection()
        {
            Task.Run(async () => await CheckConnectionAsync()).Wait();
        }

        public async Task CheckConnectionAsync()
        {
            // TODO: Implement code for checking if the motor is connected (check version from the API and check for successful response)
            var responseValue = await GetPositionAsync();
            await Task.Delay(msDelay);
            if (responseValue != null)
            {
                connected = true;
            }
            else
            {
                connected = false;
            }
        }

        public async Task<bool> CheckIfHomingComplete()
        {
            bool homingCompleted = false;
            // TODO: Implement code to check if homing is completed
            return homingCompleted;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Connected
        {
            get { return connected; }
        }

        public bool Homed
        {
            get { return homed; }
        }

        public string Version
        {
            get { return version; }
        }
    }
}
