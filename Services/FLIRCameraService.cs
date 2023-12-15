using SpinnakerNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpinnakerNET;
using SpinnakerNET.GenApi;
using Spinnaker;

namespace Independent_Reader_GUI.Services
{
    internal class FLIRCameraService
    {
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;

        public FLIRCameraService()
        {
            managedSystem = new ManagedSystem();
            cameraList = managedSystem.GetCameras(); 
            // Ensure a camera is found 
            if (cameraList.Count == 0)
            {
                MessageBox.Show("FLIR camera not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Camera found");
            }
        }
        public void Connect()
        {
            camera = cameraList[0];
            camera.Init();
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
