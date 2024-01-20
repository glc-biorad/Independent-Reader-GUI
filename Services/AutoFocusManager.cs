using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;
using Independent_Reader_GUI.Models.Hardware.Motor;

namespace Independent_Reader_GUI.Services
{
    internal class AutoFocusManager
    {
        private BitmapManager bitmapManager;
        private const double resizeWidthRatio = 0.1;
        private const double resizeHeightRatio = 0.1;

        public AutoFocusManager()
        {
            bitmapManager = new BitmapManager();
        }

        public async Task<Bitmap> AutoFocus(MotorsManager motorManager, FLIRCameraManager cameraManager, Bitmap bitmap, int dz = 1000)
        {
            double focusMetricOriginal;
            double focusMetricUp;
            double focusMetricDown;
            // Calculate a focus metric 
            focusMetricOriginal = bitmapManager.CalculateFocusMetric(bitmap);
            // Move the camera up
            MotorCommand upCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.MoveRelativeAsync,
                DistanceParameter = dz,
                SpeedParameter = 200000
            };
            motorManager.EnqueuePriorityCommand(upCommand);
            // Capture the image
            var upImage = cameraManager.CurrentImage;
            // Calcualte the focus metric
            focusMetricDown = bitmapManager.CalculateFocusMetric(upImage);
            // Move the camera down twice
            MotorCommand downCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.MoveRelativeAsync,
                DistanceParameter = -dz*2,
                SpeedParameter = 200000
            };
            motorManager.EnqueuePriorityCommand(downCommand);
            // Capture the image
            var downImage = cameraManager.CurrentImage;
            // Calculate the focus metric
            focusMetricDown = bitmapManager.CalculateFocusMetric(downImage);
            // Return the image with the best focus metric
            return downImage;
        }

        /// <summary>
        /// Apply a GaussianSharpen Filter to a Bitmap image
        /// </summary>
        /// <param name="originalImage">Original bitmap image</param>
        /// <returns>New image after application of the Gaussian Sharpening Filter</returns>
        public async Task<Bitmap> SharpenImage(Bitmap originalImage)
        {
            // Setup the Sharpening Filter
            GaussianSharpen sharpeningFilter = new GaussianSharpen(sigma: 3, size: 5);
            // Resize the image
            Bitmap smallImage = bitmapManager.ResizeImage(originalImage, (Int32)(originalImage.Width * resizeWidthRatio), (Int32)(originalImage.Height * resizeHeightRatio)); 
            // Apply the Sharpening Filter
            Bitmap sharpenedImage = sharpeningFilter.Apply(smallImage);
            // Resize the image
            Bitmap normalImage = bitmapManager.ResizeImage(sharpenedImage, (Int32)(originalImage.Width), (Int32)(originalImage.Height));
            return normalImage;
        }
    }
}
