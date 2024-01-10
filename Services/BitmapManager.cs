using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;

namespace Independent_Reader_GUI.Services
{
    internal class BitmapManager
    {
        /// <summary>
        /// Apply a Resize Bilinear filter to change the size of the bitmap image
        /// </summary>
        /// <param name="originalImageBitmap"></param>
        /// <param name="width">Width of the new image</param>
        /// <param name="height">Height of the new image</param>
        /// <returns></returns>
        public Bitmap ResizeImage(Bitmap originalImageBitmap, int width, int height)
        {
            ResizeBilinear resizeFilter = new ResizeBilinear(width, height);
            Bitmap resizedImage = resizeFilter.Apply(originalImageBitmap);
            return resizedImage;
        }

        public double CalculateFocusMetric(Bitmap originalImageBitmap)
        {
            var sobelFilter = new SobelEdgeDetector();
            var edges = sobelFilter.Apply(originalImageBitmap);
            var variance = CalculateVariance(edges);
            return variance;
        }

        private double CalculateVariance(Bitmap bitmap)
        {
            double sum = 0;
            double sumSquares = 0;
            int pixelCount = 0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var gray = (bitmap.GetPixel(x, y).R + bitmap.GetPixel(x, y).G + bitmap.GetPixel(x, y).B) / 3;
                    sum += gray;
                    sumSquares += gray * gray;
                    pixelCount++;
                }
            }

            double mean = sum / pixelCount;
            return (sumSquares / pixelCount) - (mean * mean);
        }
    }
}
