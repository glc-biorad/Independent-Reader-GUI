using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SpinnakerNET;
using SpinnakerNET.GenApi;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the FLIR Camera
    /// </summary>
    internal class FLIRCameraManager
    {
        private bool connected;
        private bool streaming;
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;
        private PictureBox pictureBox;

        public FLIRCameraManager(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
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
                try
                {
                    // FIXME: This section of code does not result in an image filling in the ImagingPictureBox
                    camera.BeginAcquisition();
                    using (IManagedImage rawImage = camera.GetNextImage())
                    {
                        if (rawImage.IsIncomplete)
                        {
                            throw new Exception("Image incomplete during image capture");
                        }

                        // Convert the raw image to BitmapSource
                        IManagedImageProcessor processor = new ManagedImageProcessor();
                        using (IManagedImage convertedImage = processor.Convert(rawImage, PixelFormatEnums.Mono8))
                        {
                            Bitmap bitmap = convertedImage.bitmap;
                            //BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                            //Marshal.Copy(convertedImage.DataPtr, bmpData.Scan0, 0, convertedImage.DataSize);
                            //bitmap.UnlockBits(bmpData);
                            pictureBox.Image = bitmap;
                        }
                    }
                }
                finally
                {
                    camera.EndAcquisition();
                }
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
