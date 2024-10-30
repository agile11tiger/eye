using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
namespace EyEServer.Services.Email;

public class EmailService(IOptions<PettyBotEmailData> pettyBotEmailData)
{
    private readonly PettyBotEmailData _pettyBotEmailData = pettyBotEmailData.Value;

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(IdentityResource.SiteAdministration, _pettyBotEmailData.Email));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        using var client = new SmtpClient();
        //https://github.com/jstedfast/MailKit/blob/master/FAQ.md#SslHandshakeException
        //Todo: убрать нижнию строчку в продакшене(антивирус заменяет сертификат для проверки веб-трафика на вирусы)
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        client.CheckCertificateRevocation = false;
        await client.ConnectAsync("smtp.gmail.com", 465, true);
        //https://support.google.com/accounts/answer/185833?visit_id=01698939719730-5663282511215850580&p=InvalidSecondFactor&rd=1
        await client.AuthenticateAsync(_pettyBotEmailData.Email, _pettyBotEmailData.ApplicationPassword);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}
