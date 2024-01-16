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
        public bool Scanning;
        private ManagedSystem? managedSystem = null;
        private ManagedCameraList cameraList = new ManagedCameraList();
        private IManagedCamera? camera = null;
        private PictureBox pictureBox;
        private INodeMap nodeMapTLDevice;
        private INodeMap nodeMap;
        private Bitmap currentImageBitmap;
        private int exposure = 12; // minimum allowed
        private AutoFocusManager autoFocusManager = new AutoFocusManager();
        private BitmapManager bitmapManager = new BitmapManager();
        public string IO = "Idle";

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

        /// <summary>
        /// Set the exposure during imaging
        /// </summary>
        /// <param name="exposureTimeInMicroseconds"></param>
        /// <returns></returns>
        public async Task<int> SetExposureTime(double exposureTimeInMicroseconds)
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
                    try
                    {
                        iExposureTime.Value = exposureTimeInMicroseconds;
                        exposure = int.Parse(exposureTimeInMicroseconds.ToString());
                    }
                    catch (Exception ex)
                    {
                        iExposureTime.Value = exposure;
                    }
                }
            }
            catch (SpinnakerException ex)
            {
                MessageBox.Show($"Unable to set exposure time due to {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            return 0;
        }

        public async Task CaptureImageAsync(string path)
        {
            IO = "Capturing";
            // Clone the image 
            using (var currentImage = currentImageBitmap)
            {
                // Save the image
                currentImage.Save(path);
            }
            IO = "Idle";
        }

        public async Task<int> StartStreamAsync()
        {
            if (connected)
            {
                streaming = true;
                IO = "Streaming";
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
                    IManagedImageProcessor processor = new ManagedImageProcessor();
                    // Set the default image processor color processing method
                    processor.SetColorProcessing(ColorProcessingAlgorithm.HQ_LINEAR);
                    while (streaming)
                    {
                        try
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
                                        //currentImageBitmap = await autoFocusManager.SharpenImage(convertedImage.bitmap);
                                        currentImageBitmap = convertedImage.bitmap;
                                        //Debug.WriteLine($"Blurriness Metric: {bitmapManager.CalculateFocusMetric(currentImageBitmap)}");
                                        try
                                        {                                            
                                            pictureBox.Image = currentImageBitmap;
                                        }
                                        catch (Exception ex)
                                        {
                                            currentImageBitmap = currentImageBitmap;
                                        }                                        
                                        await Task.Delay(100);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"WARNING: FLIR Camera streaming issue {ex.Message}, attempting to continue streaming.");
                        }
                    }
                }
                catch (SpinnakerException ex)
                {
                    MessageBox.Show($"Unable to stream due to {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    streaming = false;
                    IO = "Idle";
                    camera.EndAcquisition();
                    return -1;
                }
            }
            else
            {
                streaming = false;
                MessageBox.Show("Cannot start stream, FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IO = "Idle";
                return -1;
            }
            IO = "Idle";
            return 0;
        }

        public Bitmap CurrentImage
        {
            get { return currentImageBitmap; }
        }

        public async Task StopStreamAsync()
        {
            if (connected)
            {
                streaming = false;
                IO = "Idle";
                camera.EndAcquisition();
            }
            else
            {
                streaming = false;
                IO = "Idle";
                MessageBox.Show("FLIR camera is not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void HandleFastTickEvent(DataGridViewManager dataGridViewManager, DataGridView homeCameraDataGridView)
        {
            // Check the IO for the camera
            dataGridViewManager.SetTextBoxCellStringValueByRowIndexandColumnName(homeCameraDataGridView, IO, 0, "IO");
        }
    }
}
