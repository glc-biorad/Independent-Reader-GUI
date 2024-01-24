using Independent_Reader_GUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Models;
using Configuration = Independent_Reader_GUI.Models.Configuration;
using Independent_Reader_GUI.Utilities;

namespace Independent_Reader_GUI.Services
{
    internal class PictureBoxManager
    {

        private Configuration configuration;
        public bool MouseDrawing = false;
        private Point mouseStartPoint; 
        private Point mouseEndPoint;
        public Bitmap lineBitmap;
        public Bitmap temporaryLineBitmap;

        public PictureBoxManager(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void SetMouseStartPoint(Point startPoint)
        {
            this.mouseStartPoint = startPoint;
        }
        public void SetMouseEndPoint(Point endPoint)
        {
            this.mouseEndPoint = endPoint;
        }

        public void DrawLine(PictureBox pictureBox)
        {
            using (Graphics g = Graphics.FromImage(lineBitmap))
            {
                g.DrawLine(Pens.Red, mouseStartPoint, mouseEndPoint);
            }
            pictureBox.Invalidate(); // Refresh the picture box
        }

        public void DrawTemporaryLine(PictureBox pictureBox)
        {
            using (Font boldFont = new Font("Arial", 12, FontStyle.Bold))
            {
                using (Graphics g = Graphics.FromImage(temporaryLineBitmap))
                {
                    // Clear the temporary bitmap
                    g.Clear(Color.Transparent);
                    // Draw the current state of the original bitmap
                    g.DrawImage(lineBitmap, 0, 0);
                    // Draw user line
                    g.DrawLine(Pens.Red, mouseStartPoint, mouseEndPoint);
                    // Draw the horizontal line from the start point
                    g.DrawLine(Pens.Blue, mouseStartPoint, new Point(lineBitmap.Width, mouseStartPoint.Y));
                    // Display the angle
                    AngleTool angleTool = new AngleTool();
                    double rotationalOffsetAngle = angleTool.CalculateAngleBetweenTwoLinesGivenEndPoints(mouseStartPoint, mouseStartPoint,
                        mouseEndPoint, new Point(lineBitmap.Width, mouseStartPoint.Y));
                    Debug.WriteLine($"Line: {rotationalOffsetAngle}");
                    // Draw the angle
                    g.DrawString($"{rotationalOffsetAngle:0.00}\u00B0", boldFont, Brushes.LimeGreen, new PointF(mouseStartPoint.X, mouseStartPoint.Y));
                }
                // Update the picture box
                pictureBox.Image = temporaryLineBitmap;
            }           
        }

        public void InitializeLineBitmap(PictureBox pictureBox)
        {
            lineBitmap = new Bitmap(pictureBox.Image, pictureBox.Width, pictureBox.Height);
            temporaryLineBitmap = new Bitmap(pictureBox.Image, pictureBox.Width, pictureBox.Height);
            pictureBox.Image = lineBitmap;
        }

        public async Task<int> SaveImageWithSaveFileDialog(PictureBox pictureBox)
        {
            try
            {
                if (pictureBox != null)
                {
                    // Clone the image 
                    using (var clonedImage = new Bitmap(pictureBox.Image))
                    {
                        // Open a SaveFileDialog prompt window
                        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                        {
                            // Set the image filters
                            saveFileDialog.Filter = "Tiff Image|*.tiff|JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
                            saveFileDialog.Title = "Save Captured Image";
                            saveFileDialog.InitialDirectory = configuration.DefaultLocalImageDataPath;
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                // Get the path for the file
                                string filePath = saveFileDialog.FileName;
                                // Determine the file format based on the file extension
                                ImageFormat format = ImageFormat.Tiff; // Default to tiff
                                switch (System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower())
                                {
                                    case ".png":
                                        format = ImageFormat.Png; break;
                                    case ".jpg":
                                        format = ImageFormat.Jpeg; break;
                                    case ".gif":
                                        format = ImageFormat.Gif; break;
                                    case ".bmp":
                                        format = ImageFormat.Bmp; break;
                                }
                                // Save the image
                                clonedImage.Save(filePath, format);
                            }
                            return 0;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Unable to capture image, it appears to be a null image", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to capture image due to {ex.Message}");
                return -1;
            }
        }

        public async Task<int> SaveImage(PictureBox pictureBox, string savePath)
        {
            try
            {
                if (pictureBox != null)
                {
                    // Clone the image 
                    using (var clonedImage = new Bitmap(pictureBox.Image))
                    {
                        // Set the file extension
                        ImageFormat format = ImageFormat.Tiff;
                        // Save the image
                        clonedImage.Save(savePath, format);
                        return 0;
                    }
                }
                else
                {
                    MessageBox.Show("Unable to capture image, it appears to be a null image", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to capture image due to {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Handle the Zooming in or out of a picture box
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="trackbar"></param>
        /// <param name="e"></param>
        public void HandleZoom(PictureBox pictureBox, TrackBar trackbar, EventArgs e)
        {
            // Handle the Zoom event
            int zoomFactor = trackbar.Value;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            Image originalImage = pictureBox.Image;
            if (pictureBox.Image != null && pictureBox.Image != originalImage)
            {
                pictureBox.Dispose();
            }
            // Create a new bitmap with the new zoomed size.
        }
    }
}
