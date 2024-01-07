using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;      // For BitmapData
using System.Drawing;               // For Bitmap
using System.Runtime.InteropServices; // For Marshal
using SpinnakerNET;
using SpinnakerNET.GenApi;
using System.Diagnostics;

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
        private INodeMap nodeMapTLDevice;
        private INodeMap nodeMap;

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
                // Assume one camera
                camera = cameraList[0];
                // Retrieve TL device nodemap
                nodeMapTLDevice = camera.GetTLDeviceNodeMap();
                // Initialize the camera
                camera.Init();
                // Retrieve GenICam nodemap
                nodeMap = camera.GetNodeMap();
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

        public int CaptureImage()
        {
            //
            // FIXME: This function needs to also check if streaming is already occuring, if that is the case then take a image of the screen
            //          because right now it begins acquisition and everything
            //
            if (connected)
            {
                try
                {
                    // Set acquisition mode to single frame
                    IEnum iAcquisitionMode = nodeMap.GetNode<IEnum>("AcquisitionMode");
                    if (iAcquisitionMode == null || !iAcquisitionMode.IsWritable || !iAcquisitionMode.IsReadable)
                    {
                        MessageBox.Show("Unable to capture an image because the retrieval of the AcquisitionMode node failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        camera.EndAcquisition();
                        return -1;
                    }
                    IEnumEntry iAcquisitionModeSingleFrame = iAcquisitionMode.GetEntryByName("SingleFrame");
                    if (iAcquisitionModeSingleFrame == null || !iAcquisitionModeSingleFrame.IsReadable)
                    {
                        MessageBox.Show("Unable to capture an image because the retrieval of the SingleFrame enum entry failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        camera.EndAcquisition();
                        return -1;
                    }
                    // Set symbolic from entry node as new value for enumeration node
                    iAcquisitionMode.Value = iAcquisitionModeSingleFrame.Symbolic;
                    // Start image aquisition and end it when no more images are needed
                    camera.BeginAcquisition();
                    // Create ImageProcessor instance for post processing images
                    IManagedImageProcessor processor = new ManagedImageProcessor();
                    // Set the default image processor color processing method
                    //
                    // *** NOTES ***
                    // By default, if no specific color processing algorithm is set, the image
                    // processor will default to NEAREST_NEIGHBOR method.
                    //
                    processor.SetColorProcessing(ColorProcessingAlgorithm.HQ_LINEAR);
                    //
                    // Retrieve the next image
                    //
                    // *** NOTES ***
                    // Capturing an image houses images on the camera buffer.
                    // Trying to capture an image that does not exist will
                    // hang the camera.
                    //
                    // Using-statements help ensure that images are released.
                    // If too many images remain unreleased, the buffer will
                    // fill, causing the camera to hang. Images can also be
                    // released manually by calling Release().
                    //
                    using (IManagedImage rawImage = camera.GetNextImage(1000))
                    {
                        // Ensure image completion
                        if (rawImage.IsIncomplete)
                        {
                            MessageBox.Show($"Unable to capture image due to the image being incomplete with an image state {rawImage.ImageStatus}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            camera.EndAcquisition();
                            return -1;
                        }
                        else
                        {
                            uint width = rawImage.Width;
                            uint height = rawImage.Height;
                            // Convert the image to mono 8
                            using (IManagedImage convertedImage = processor.Convert(rawImage, PixelFormatEnums.Mono8))
                            {
                                // Create a unique filename
                                // FIXME: Change where images are saved based on a location picked by the user
                                string filename = "C:\\Users\\u112958\\source\\repos\\Independent-Reader-GUI\\test.jpg";
                                // Save the image
                                convertedImage.Save(filename);
                                var bitmap = convertedImage.bitmap;
                                //BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                pictureBox.Image = convertedImage.bitmap;
                                //pictureBox.Image = Image.FromFile(filename);
                                //bitmap.UnlockBits(bmpData);
                                rawImage.Dispose();
                                convertedImage.Dispose();
                            }
                        }
                    }
                }
                catch (SpinnakerException ex)
                {
                    MessageBox.Show($"Unable to capture the image due to {ex.Message}", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    camera.EndAcquisition();
                    return -1;
                }
                // End acquisition so the device can properly clean up
                MessageBox.Show("Capture Ok");
                camera.EndAcquisition();
            }
            else
            {
                MessageBox.Show("Cannot capture image, FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            return 0;
        }

        public int SetExposureTime(double exposureTimeInMicroseconds)
        {
            try
            {
                //
                // Disable the Auto Exposure
                //
                // Get the ExposureAuto node
                IEnum iExposureAuto = nodeMap.GetNode<IEnum>("ExposureAuto");
                if (iExposureAuto == null || !iExposureAuto.IsWritable || !iExposureAuto.IsReadable)
                {
                    MessageBox.Show("Unable to set the exposure time because the retrieval of the ExposureAuto node failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
                // Get the Entry Enum
                IEnumEntry iExposureAutoOff = iExposureAuto.GetEntryByName("Off");
                if (iExposureAutoOff == null || !iExposureAutoOff.IsReadable)
                {
                    MessageBox.Show("Unable to set the exposure time because the retrieval of the Off enum entry failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
                // Set the AutoExposure to Off
                iExposureAuto.Value = iExposureAutoOff.Symbolic;
                // 
                // Set the Exposure Time
                //
                // Get the ExposureTime node
                IFloat iExposureTime = nodeMap.GetNode<IFloat>("ExposureTime");
                if (iExposureTime == null || !iExposureTime.IsWritable || !iExposureTime.IsReadable)
                {
                    MessageBox.Show("Unable to set the exposure time because the retrieval of the ExposureTime node failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
                else
                {
                    iExposureTime.Value = exposureTimeInMicroseconds;
                }
            }
            catch (SpinnakerException ex)
            {
                MessageBox.Show($"Unable to set exposure time due to {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            return 0;
        }

        public async Task CaptureImageAsync()
        {
            CaptureImage();
            await Task.Delay(100);
        }

        public async Task<int> StartStreamAsync()
        {
            if (connected)
            {
                streaming = true;
                try
                {
                    // Retrieve enumeration node from nodemap
                    IEnum iAcquisitionMode = nodeMap.GetNode<IEnum>("AcquisitionMode");
                    if (iAcquisitionMode == null || !iAcquisitionMode.IsWritable || !iAcquisitionMode.IsReadable)
                    {
                        MessageBox.Show("Unable to stream because the retrieval of the AcquisitionMode node failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        streaming = false;
                        return -1;
                    }
                    // Retrieve entry node from enumeration node
                    IEnumEntry iAcquisitionModeContinuous = iAcquisitionMode.GetEntryByName("Continuous");
                    if (iAcquisitionModeContinuous == null || !iAcquisitionModeContinuous.IsReadable)
                    {
                        MessageBox.Show("Unable to stream because the retrieval of the AcquisitionModeContinuous node entry failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        streaming = false;
                        return -1;
                    }
                    // Set acquisition mode to continuous
                    iAcquisitionMode.Value = iAcquisitionModeContinuous.Symbolic;
                    // Begin acquiring images
                    camera.BeginAcquisition();
                    // Create an ImageProcessor instance for post processing images
                    // NOTE: This can be a private instance for the class since it is also used in CaptureImage
                    IManagedImageProcessor processor = new ManagedImageProcessor();
                    // Set the default image processor color processing method
                    processor.SetColorProcessing(ColorProcessingAlgorithm.HQ_LINEAR);
                    while (streaming)
                    {
                        // Retrieve the next recieved image
                        using (IManagedImage rawImage = camera.GetNextImage(1000))
                        {
                            // Ensure image completion
                            if (rawImage.IsIncomplete)
                            {
                                Debug.WriteLine($"Raw Image from FLIR Camera is incomplete with image status {rawImage.ImageStatus}");
                            }
                            else
                            {
                                using (IManagedImage convertedImage = processor.Convert(rawImage, PixelFormatEnums.Mono8))
                                {
                                    pictureBox.Image = convertedImage.bitmap;
                                    await Task.Delay(100);
                                }
                            }
                        }
                    }
                }
                catch (SpinnakerException ex)
                {
                    MessageBox.Show($"Unable to stream due to {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    streaming = false;
                    camera.EndAcquisition();
                    return -1;
                }
            }
            else
            {
                streaming = false;
                MessageBox.Show("Cannot start stream, FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            return 0;
        }

        public async Task StopStreamAsync()
        {
            if (connected)
            {
                streaming = false;
                camera.EndAcquisition();
            }
            else
            {
                streaming = false;
                MessageBox.Show("FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
