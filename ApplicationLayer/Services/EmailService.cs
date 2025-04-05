using ApplicationLayer.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApplicationLayer.Services;

public class EmailService(IOptions<MailSettings> mailSetting, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSettings _mailSetting = mailSetting.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSetting.Mail),
            Subject = subject,
        };

        message.To.Add(MailboxAddress.Parse(email));
        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        _logger.LogInformation("Sending email to {email}", email);
        smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
        await smtp.SendAsync(message);
        smtp.Disconnect(true);
    }
}
