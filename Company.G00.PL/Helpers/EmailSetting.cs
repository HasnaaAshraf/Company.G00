using System.Net;
using System.Net.Mail;

namespace Company.G00.PL.Helpers
{
    public static class EmailSetting
    {
        public static bool SendEmail (Email email)
        {

            // Mail Server : Gmail

            // SMTP => Semple Mail Transfer Protocol (To Send Mails)

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true; // If I Want To Encapsolate In Later Time 

                // hvrylyccamyuroql

                // Take Gmail And Fake Password Of (Sender):
                client.Credentials = new NetworkCredential("hasnaaashraf2000@gmail.com", "hvrylyccamyuroql");

                client.Send("hasnaaashraf2000@gmail.com", email.To, email.Subject, email.Body);

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
       
    }
}
