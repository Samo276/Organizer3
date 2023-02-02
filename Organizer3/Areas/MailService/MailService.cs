using MailKit.Net.Smtp;
using MimeKit;
using Organizer3.Areas.MailService;

namespace Organizer3.Areas.MailService
{
    public class MailService
    {
        //
        //configure sender email settings
        // use https://ethereal.email/create to create dummy smtp server and email for testing
        //
        private string emailAdress = "sage.howe52@ethereal.email";
        private string emailPassword = "9uNR3edZQwSnYVfZg9";
        private string smtpAdress = "smtp.ethereal.email";
        private int smtpPort = 587;
        
        public void SendMail(string emailSubject,string emailBody, string sendTo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailAdress));
            email.To.Add(MailboxAddress.Parse(sendTo));
            email.Subject = emailSubject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = emailBody };

            using (var smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(smtpAdress, smtpPort, MailKit.Security.SecureSocketOptions.Auto);
                smtp.Authenticate(emailAdress, emailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

        }
    }
}
