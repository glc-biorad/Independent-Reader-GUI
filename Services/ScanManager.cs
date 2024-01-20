using Independent_Reader_GUI.Models;
using Independent_Reader_GUI.Models.Hardware.LED;
using Independent_Reader_GUI.Models.Hardware.Motor;
using Independent_Reader_GUI.Models.Scanning;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class ScanManager
    {
        Configuration configuration;
        MotorsManager motorsManager;
        FLIRCameraManager cameraManager;
        LEDManager ledManager;
        TECManager tecManager;
        private int msDelay = 500;
        private int positionCutoff = 500;

        public ScanManager(Configuration configuration, MotorsManager motorsManager, FLIRCameraManager cameraManager, LEDManager ledManager, TECManager tecManager)
        {
            this.configuration = configuration;
            this.motorsManager = motorsManager;
            this.cameraManager = cameraManager;
            this.ledManager = ledManager;
            this.tecManager = tecManager;
        }

        /// <summary>
        /// Scan based on ScanParameters
        /// </summary>
        /// <param name="scanParameters"></param>
        /// <returns></returns>
        public async Task Scan(ScanParameters scanParameters)
        {
            // Set the camera to IO scanning
            cameraManager.Scanning = true;
            cameraManager.IO = "Scanning";
            // Turn off TEC Fans if thermocycling protocols are not running on them
            tecManager.TurnOffFansBasedOnOutcome(!tecManager.TECA.RunningProtocol, !tecManager.TECB.RunningProtocol, !tecManager.TECC.RunningProtocol, !tecManager.TECD.RunningProtocol);
            // Create an experiment folder
            string experimentName = scanParameters.ExperimentName;
            string dir = "C:\\Users\\u112958\\source\\repos\\Independent-Reader-GUI\\ReaderImages";
            Directory.CreateDirectory($"{dir}\\{experimentName}");
            // Create a heater folder inside the experiment folder (heaterB for example)
            string heaterLetter = scanParameters.HeaterLetter;
            Directory.CreateDirectory($"{dir}\\{experimentName}\\heater{heaterLetter}");
            // Move the z motor to its position
            MotorCommand zMotorCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.MoveAsync,
                Motor = motorsManager.GetMotorByName("z"),
                PositionParameter = scanParameters.z0,
                SpeedParameter = configuration.zMotorDefaultSpeed,
            };
            motorsManager.EnqueuePriorityCommand(zMotorCommand);
            await motorsManager.GetMotorByName("z").GetPositionAsync();
            int z = Math.Abs(motorsManager.GetMotorByName("z").Position);
            while (z > scanParameters.z0 + positionCutoff || z < scanParameters.z0 - positionCutoff)
            {
                await Task.Delay(msDelay);
                z = Math.Abs(motorsManager.GetMotorByName("z").Position);
            }
            // Image based on the Samples, Assays, and Image Parameters
            for (int sampleIdx = 0; sampleIdx < scanParameters.SampleNames.Names.Count; sampleIdx++)
            {
                string sampleName = scanParameters.SampleNames.Names[sampleIdx];
                if (sampleName != "")
                {
                    // TODO: Create a folder for this same (chamber2 for example, start from 1) 
                    Directory.CreateDirectory($"{dir}\\{experimentName}\\heater{heaterLetter}\\chamber{sampleIdx+1}");
                    for (int assayIdx = 0; assayIdx < scanParameters.AssayNames.Names.Count; assayIdx++)
                    {
                        string assayName = scanParameters.AssayNames.Names[assayIdx];
                        if (assayName != "")
                        {
                            // Get the number of FOVs needed
                            int numberOfFOVs = scanParameters.SampledX / scanParameters.FOVdX;
                            for (int fovIdx = 0; fovIdx < numberOfFOVs; fovIdx++)
                            {
                                // Create the coordinate file name
                                string fileName = $"{experimentName}___heater{heaterLetter}_chamber{sampleIdx+1}_{assayName}_colFOV{fovIdx}.txt";
                                // Get the new x and y positions
                                int xPos = scanParameters.x0 - scanParameters.FOVdX * fovIdx - scanParameters.SampledX * assayIdx;
                                int yPos = scanParameters.y0 + scanParameters.dY * sampleIdx;
                                if (scanParameters.RotationalOffset > 0)
                                {
                                    if (fovIdx != 0 || assayIdx != 0)
                                    {
                                        var _ = ApplyRotationalOffset(xPos, yPos, scanParameters.RotationalOffset);
                                        xPos = (Int32)_.Item1;
                                        yPos = (Int32)_.Item2;
                                        Debug.WriteLine($"New X,y -> {xPos}, {yPos}");
                                    }
                                }                               
                                // Move to the position for this FOV
                                MotorCommand xMotorCommand = new MotorCommand
                                {
                                    Type = MotorCommand.CommandType.MoveAsync,
                                    Motor = motorsManager.GetMotorByName("x"),
                                    PositionParameter = xPos,
                                    SpeedParameter = configuration.xMotorDefaultSpeed,
                                };
                                motorsManager.EnqueuePriorityCommand(xMotorCommand);
                                MotorCommand yMotorCommand = new MotorCommand
                                {
                                    Type = MotorCommand.CommandType.MoveAsync,
                                    Motor = motorsManager.GetMotorByName("y"),
                                    PositionParameter = yPos,
                                    SpeedParameter = configuration.yMotorDefaultSpeed,
                                };
                                motorsManager.EnqueuePriorityCommand(yMotorCommand);
                                // Wait for x, y, and z to be at the initial position
                                MotorCommand getXCommand = new MotorCommand
                                {
                                    Type = MotorCommand.CommandType.GetPositionAsync,
                                    Motor = motorsManager.GetMotorByName("x"),
                                };
                                motorsManager.EnqueuePriorityCommand(getXCommand);
                                MotorCommand getYCommand = new MotorCommand
                                {
                                    Type = MotorCommand.CommandType.GetPositionAsync,
                                    Motor = motorsManager.GetMotorByName("y"),
                                };
                                motorsManager.EnqueuePriorityCommand(getYCommand);
                                int x = Math.Abs(motorsManager.GetMotorByName("x").Position);
                                int y = Math.Abs(motorsManager.GetMotorByName("y").Position);
                                while ((x < xPos - positionCutoff || x > xPos + positionCutoff) || (y < yPos - positionCutoff || y > yPos + positionCutoff))
                                {
                                    await Task.Delay(msDelay);
                                    x = Math.Abs(motorsManager.GetMotorByName("x").Position);
                                    y = Math.Abs(motorsManager.GetMotorByName("y").Position);
                                }
                                // Image all channels if desired and name the image files
                                int intensity;
                                int exposure;
                                int filterWheelPos;
                                string imagePath = $"{dir}\\{experimentName}\\heater{heaterLetter}\\chamber{sampleIdx + 1}\\FOV{fovIdx + 1}";
                                if (scanParameters.ImageBrightField)
                                {
                                    intensity = scanParameters.BrightFieldIntensity;
                                    exposure = scanParameters.BrightFieldExposure;
                                    filterWheelPos = configuration.FAMFilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"bf_{assayName}_{exposure}.tiff", 
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("HEX"));
                                }
                                if (scanParameters.ImageCy5)
                                {
                                    intensity = scanParameters.Cy5Intensity;
                                    exposure = scanParameters.Cy5Exposure;
                                    filterWheelPos = configuration.Cy5FilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"cy5_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("Cy5"));
                                }
                                if (scanParameters.ImageFAM)
                                {
                                    intensity = scanParameters.FAMIntensity;
                                    exposure = scanParameters.FAMExposure;
                                    filterWheelPos = configuration.FAMFilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"fam_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("FAM"));
                                }
                                if (scanParameters.ImageHEX)
                                {
                                    intensity = scanParameters.HEXIntensity;
                                    exposure = scanParameters.HEXExposure;
                                    filterWheelPos = configuration.HEXFilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"hex_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("HEX"));
                                }
                                if (scanParameters.ImageAtto)
                                {
                                    intensity = scanParameters.AttoIntensity;
                                    exposure = scanParameters.AttoExposure;
                                    filterWheelPos = configuration.AttoFilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"Atto_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("Atto"));
                                }
                                if (scanParameters.ImageAlexa)
                                {
                                    intensity = scanParameters.AlexaIntensity;
                                    exposure = scanParameters.AlexaExposure;
                                    filterWheelPos = configuration.AlexaFilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"alexa_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("Alexa"));
                                }
                                if (scanParameters.ImageCy5p5)
                                {
                                    intensity = scanParameters.Cy5p5Intensity;
                                    exposure = scanParameters.Cy5p5Exposure;
                                    filterWheelPos = configuration.Cy5p5FilterWheelPosition;
                                    // Image
                                    await TakeImage(imagePath, $"cy55_{assayName}_{exposure}.tiff",
                                        intensity, exposure, filterWheelPos, ledManager.GetLEDFromName("Cy5p5"));
                                }
                            }
                        }
                    }
                }
            }
            cameraManager.Scanning = false;
            if (cameraManager.Streaming)
            {
                cameraManager.IO = "Streaming";
            }
            else
            {
                cameraManager.IO = "Idle";
            }
        }

        /// <summary>
        /// Take an image and save it
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <param name="imageFileName"></param>
        /// <param name="intensity"></param>
        /// <param name="exposure"></param>
        /// <param name="filterWheelPosition"></param>
        /// <returns></returns>
        private async Task TakeImage(string imageFilePath, string imageFileName, int intensity, int exposure, int filterWheelPosition, LED led)
        {
            // Turn off all LEDs
            ledManager.TurnAllOff();
            // Turn on the LED
            LEDCommand ledCommand = new LEDCommand { Type = LEDCommand.CommandType.On, Intensity = intensity, LED = led };
            ledManager.EnqueuePriorityCommand(ledCommand);
            // Move the Filter Wheel
            MotorCommand filterWheelMoveCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.MoveAsync,
                Motor = motorsManager.GetMotorByName("Filter Wheel"),
                PositionParameter = filterWheelPosition,
                SpeedParameter = configuration.FilterWheelMotorDefaultSpeed,
            };
            motorsManager.EnqueuePriorityCommand(filterWheelMoveCommand);
            MotorCommand getFilterWheelCommand = new MotorCommand
            {
                Type = MotorCommand.CommandType.GetPositionAsync,
                Motor = motorsManager.GetMotorByName("Filter Wheel"),
            };
            motorsManager.EnqueuePriorityCommand(getFilterWheelCommand);
            int _ = Math.Abs(motorsManager.GetMotorByName("Filter Wheel").Position);
            while (_ != filterWheelPosition)
            {
                await Task.Delay(msDelay);
                _ = Math.Abs(motorsManager.GetMotorByName("Filter Wheel").Position);
            }
            // Capture the image
            Directory.CreateDirectory(imageFilePath);
            // Set Exposure in Microseconds
            //imageFileName = imageFileName.Replace(" ", "_");
            Debug.WriteLine($"Saving Image to: {imageFilePath}\\{imageFileName}");
            await cameraManager.SetExposureTime(exposure);
            await Task.Delay(100);
            await cameraManager.CaptureImageAsync($"{imageFilePath}\\{imageFileName}");
        }

        /// <summary>
        /// Apply the Rotational Offset to a coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rotationalOffsetInDegrees"></param>
        /// <returns></returns>
        private Tuple<double, double> ApplyRotationalOffset(int x, int y, double rotationalOffsetInDegrees)
        {
            // Convert angle from degrees to radians
            double theta = (Math.PI / 180) * rotationalOffsetInDegrees;
            // Define the rotation matrix
            double cosTheta = Math.Cos(theta);
            double sinTheta = Math.Sin(theta);
            double[,] R =
            {
                {cosTheta, -sinTheta },
                {sinTheta, cosTheta}
            };
            // Apply the rotation matrix to coordiante (x and y)
            double Rx = R[0, 0] * x + R[0, 1] * y;
            double Ry = R[1, 0] * (double)x + R[1, 1] * (double)y;
            Debug.WriteLine($"HERE: {x}, {y} -> {Rx}, {Ry} -> {rotationalOffsetInDegrees}");
            return Tuple.Create(Rx, Ry);
        }
    }
}
