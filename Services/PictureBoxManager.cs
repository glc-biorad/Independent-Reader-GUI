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

namespace Independent_Reader_GUI.Services
{
    internal class PictureBoxManager
    {

        private Configuration configuration;

        public PictureBoxManager(Configuration configuration)
        {
            this.configuration = configuration;
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
    }
}
