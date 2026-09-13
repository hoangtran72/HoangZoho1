using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HoangZoho1.Helpers
{
    public class EmailHelpers
    {

        public static async Task SendEmail(EmailContent emailContent)
        {
            try
            {
                using MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(emailContent.SmtpServer);

                mail.From = new MailAddress(emailContent.Email);
                mail.IsBodyHtml = true;
                foreach (var address in emailContent.Clients.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }
                mail.Subject = emailContent.Subject;
                mail.Body = emailContent.Body;

                SmtpServer.Port = emailContent.SmtpPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailContent.Email, emailContent.Password);
                SmtpServer.EnableSsl = true;

                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
