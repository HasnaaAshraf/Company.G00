using Company.G00.PL.Helpers;
using Company.G00.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.G00.PL.InterfacesHelpers
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {

        // "armmpyrixsbcudin"

    
        public void SendEmail(Email email)
        {
            // Build Message (Email)

            var mail = new MimeMessage();

            mail.Subject = email.Subject;
            mail.From.Add( new MailboxAddress(_options.Value.DisplayName,_options.Value.Email));  // By This Way => Because It List
            mail.To.Add(MailboxAddress.Parse(email.To));  // Because It List

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish Connection

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.password);

            // Send Message 

            smtp.Send(mail);

        }
    }
}
