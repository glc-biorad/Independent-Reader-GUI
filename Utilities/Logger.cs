using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Utilities
{
    internal class Logger
    {
        public static void LogError(string message, Exception ex = null)
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("MM/dd/yyyy");
            string time = now.ToString("hh:mm:ss tt");
            string errorMessage = $"[ERROR] {time} - {date}: {message}";
            if (ex != null)
            {
                errorMessage += $"\n\tException: {ex.Message}\n\tStack Trace: {ex.StackTrace}";
            }
            Debug.WriteLine(errorMessage);
        }

        public static void LogWarning(string message)
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("MM/dd/yyyy");
            string time = now.ToString("hh:mm:ss tt");
            string warningMessage = $"[WARNING] {time} - {date}: {message}";
            Debug.WriteLine(warningMessage);
        }

        public static void LogInfo(string message)
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("MM/dd/yyyy");
            string time = now.ToString("hh:mm:ss tt");
            string infoMessage = $"[INFO] {time} - {date}: {message}";
            Debug.WriteLine(infoMessage);
        }
    }
}
