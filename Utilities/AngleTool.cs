using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class AngleTool
    {
        public double CalculateAngleBetweenTwoLinesGivenEndPoints(Point startPoint1, Point startPoint2, Point endPoint1, Point endPoint2)
        {
            // Calculate the slope of the two lines
            double m1 = (double)(endPoint1.Y - startPoint1.Y) / (endPoint1.X - startPoint1.X);
            double m2 = (double)(endPoint2.Y - startPoint2.Y) / (endPoint2.X - startPoint2.X);
            // Calculate the angle between these two lines
            double thetaRadians = Math.Atan(Math.Abs((m2 - m1) / (1 + m1 * m2)));
            double thetaDegrees = thetaRadians * (180.0 / Math.PI);
            return thetaDegrees;
        }
    }
}
