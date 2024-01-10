using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace Independent_Reader_GUI.Services
{
    internal class EmailManager
    {
        private SmtpClient smtpClient;
        private string fromAddress = "gabriel_lopez-candales@bio-rad.com";

        public EmailManager()
        {
            smtpClient = new SmtpClient("smtp.bio-rad.com")
            {
                Port = 587, // Standard port for SMTP
                Credentials = new NetworkCredential(fromAddress, "???"),
                EnableSsl = true, // Enable SSL for secure email sending
            };
        }

        public void SendEmail(string toAddress)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Test Email",
                Body = "This is a test email from C#",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toAddress);

            // Send the email
            try
            {
                smtpClient.Send(mailMessage);
                Debug.WriteLine("Email sent successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in sending email due to {ex.Message}");
            }
        }
    }
}
