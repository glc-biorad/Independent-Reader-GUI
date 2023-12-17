using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpinnakerNET;
using SpinnakerNET.GenApi;

namespace Independent_Reader_GUI.Services
{
    internal class FLIRCameraService
    {
        private bool connected;
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;

        public FLIRCameraService()
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
                MessageBox.Show("FLIR camera not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connected = false;
            }
        }

        public bool Connected
        { 
            get { return connected; } 
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

        }
    }
}
