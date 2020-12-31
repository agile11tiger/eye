using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace EyE.Server.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //EmailServiceData нужно вводить самому!
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", EmailServiceData.GmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                //https://github.com/jstedfast/MailKit/blob/master/FAQ.md#SslHandshakeException
                //Todo: убрать нижнию строчку в продакшене(антивирус заменяет сертификат для проверки веб-трафика на вирусы)
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(EmailServiceData.GmailAddress, EmailServiceData.GmailPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
