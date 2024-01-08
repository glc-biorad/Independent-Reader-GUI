using Renci.SshNet;
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
        private string host;
        private string username;
        private string password;

        public FTPManager(string host, string username, string password)
        {
            this.host = host;
            this.username = username;
            this.password = password;
        }

        public async Task<int> UploadFile(string localFilePath, string ftpURL)
        {
            string userName = "moldx";
            string password = "cdg2023";

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

        public async Task<int> UploadFileAsync(string localFilePath, string remotePath)
        {
            // Setup the Secure File Transfer Protocol Client
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    using (var fileStream = File.OpenRead(localFilePath))
                    {
                        sftp.UploadFile(fileStream, remotePath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unable to upload {localFilePath} to {remotePath} due to {ex.Message}");
                    return -1;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
                return 0;
        }
    }
}
