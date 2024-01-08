using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class FTPManager
    {
        public async Task<int> UploadFile(string localFilePath, string ftpURL)
        {
            string userName = "u112958";
            string password = "biorad2023";

            // Setup the FTP Web Request for this FTP
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpURL);

            // Set the request method
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Setup request credentials
            request.Credentials = new NetworkCredential(userName, password);

            // Prep the file bytes
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(localFilePath))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }
            request.ContentLength = fileContents.Length;

            // Transfer the file contents
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            // Check the status of the FTP
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Debug.WriteLine($"File Transfer Protocol Status: {response.StatusDescription}, {response.StatusCode.ToString()}");
            }
            return 0;
        }

        public async Task<int> DownloadFile(string ftpURL, string localFilePath)
        {
            return 0;
        }
    }
}
