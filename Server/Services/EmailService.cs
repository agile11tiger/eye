using MailKit.Net.Smtp;
using Microsoft.Extensions.Localization;
using MimeKit;
using System.Threading.Tasks;
namespace EyEServer.Services;

public class EmailService(IStringLocalizer<EmailService> _localizer)
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        //EmailServiceData нужно вводить самому!
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_localizer["SiteAdministration"], EmailServiceData.GMAIL_ADDRESS));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        //https://github.com/jstedfast/MailKit/blob/master/FAQ.md#SslHandshakeException
        //Todo: убрать нижнию строчку в продакшене(антивирус заменяет сертификат для проверки веб-трафика на вирусы)
        //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        client.CheckCertificateRevocation = false;
        await client.ConnectAsync("smtp.gmail.com", 465, true);
        await client.AuthenticateAsync(EmailServiceData.GMAIL_ADDRESS, EmailServiceData.GMAIL_PASSWORD);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}
