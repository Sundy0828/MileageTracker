using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mileage_Tracker.Models;
using System.Net;
using System.Net.Mail;

namespace Mileage_Tracker.Classes
{
    public static class Email
    {
        public static bool Mail(string to, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage("jerrod.sunderland@gmail.com", to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("jerrod.sunderland2@gmail.com", "ChangeMe$");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mailMessage);
                return true;
            }catch
            {
                return false;
            }
        }
    }
}