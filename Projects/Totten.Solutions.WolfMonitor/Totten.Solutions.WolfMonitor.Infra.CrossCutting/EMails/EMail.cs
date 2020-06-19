using System.Net.Mail;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.EMails
{
    public static class EMail
    {
        public static void Send(string title, string body, string toEmail, string senderName, string myEmail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(myEmail, "");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress(myEmail, senderName);
            mail.From = new MailAddress(myEmail);
            mail.To.Add(new MailAddress(toEmail));
            mail.Subject = title;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch
            {
            }
            finally
            {
                mail = null;
            }
        }
    }
}
