using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
    public static class EmailService
    {
        public static void SendEmail(string emailTo, string mailbody, string subject)
        {
            //Instância classe email
            MailMessage mail = new MailMessage();
            mail.To.Add(emailTo);
            mail.From = new MailAddress("soccerplay@jgdsystems.com.br");
            mail.Subject = subject;
            mail.Body = mailbody;
            mail.IsBodyHtml = true;

            //Instância smtp do servidor, neste caso o gmail.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.umbler.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("soccerplay@jgdsystems.com.br", "ZQ,z!Q8@q3Sz");// Login e senha do e-mail.
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }

    }
}