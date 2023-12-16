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
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;

        public FLIRCameraService()
        {
            MessageBox.Show("1");
            managedSystem = new ManagedSystem();
            MessageBox.Show("2");
            cameraList = managedSystem.GetCameras();
            MessageBox.Show("3");
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
