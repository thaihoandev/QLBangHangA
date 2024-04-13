using System.Net;
using System.Net.Mail;

namespace QLBangHangA.Extentions
{
    public class SendMail
    {
        public static bool SendEmail(string to, string subject, string body, bool isHtml = false, string attachFile = "")
        {
            try
            {
                MailMessage msg = new MailMessage(ContactSender.emailSender, to, subject, body);
                msg.IsBodyHtml = isHtml;
                using (var client = new SmtpClient(ContactSender.hostEmail, ContactSender.portEmail))
                {
                    client.EnableSsl = true;
                    if(!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        msg.Attachments.Add(attachment);
                    }
                    NetworkCredential credential = new NetworkCredential(ContactSender.emailSender, ContactSender.passwordSender);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.Send(msg);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
