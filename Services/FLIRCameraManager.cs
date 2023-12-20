using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpinnakerNET;
using SpinnakerNET.GenApi;

namespace Independent_Reader_GUI.Services
{
    internal class FLIRCameraManager
    {
        private bool connected;
        private bool streaming;
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;

        public FLIRCameraManager()
        {
            managedSystem = new ManagedSystem();
            cameraList = managedSystem.GetCameras();
        }

        /// <summary>
        /// Attempts to connect to the FLIR camera
        /// </summary>
        public void Connect()
        {
            try
            {
                camera = cameraList[0];
                camera.Init();
                connected = true;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                connected = false;
            }
        }

        public bool Connected
        { 
            get { return connected; } 
        }
        public bool Streaming
        { 
            get { return streaming; }
        }

        public void Disconnect()
        {
            cameraList.Clear();
            if (managedSystem != null)
            {
                managedSystem.Dispose();
            }
        }

        public void CaptureImage()
        {
            if (connected)
            {
                // TODO: Add code to capture an image
                MessageBox.Show("Capture image code has not been implemented yet", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Cannot capture image, FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public async Task CaptureImageAsync()
        {
            CaptureImage();
            await Task.Delay(1);
        }

        public void StartStream()
        {
            if (connected)
            {
                streaming = true;
                // TODO: Add code for streaming to the Imaging Pixture Box
                MessageBox.Show("Streaming code has not been implemented yet", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
            }
            else
            {
                streaming = false;
                MessageBox.Show("Cannot start stream, FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public async Task StartStreamAsync()
        {
            StartStream();
            await Task.Delay(1);
        }

        public void StopStream()
        {
            if (connected)
            {
                streaming = false;
                // TODO: Add code for stopping a stream
                MessageBox.Show("Streaming code has not been implemented yet", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                streaming = false;
                MessageBox.Show("FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
