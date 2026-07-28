using InvestmentResearch.Model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace InvestmentResearch.Service;

public class SmtpEmailService : IEmailService
{
    private readonly AppSettings _appSettings;

    public SmtpEmailService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var fromAddress = string.IsNullOrWhiteSpace(_appSettings.FromEmail) ? to : _appSettings.FromEmail;

        using var message = new MailMessage();
        message.From = new MailAddress(fromAddress);
        message.To.Add(new MailAddress(to));
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var client = new SmtpClient(_appSettings.SmtpHost, _appSettings.SmtpPort)
        {
            EnableSsl = _appSettings.SmtpUseSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
        };

        if (!string.IsNullOrWhiteSpace(_appSettings.SmtpUser))
        {
            client.Credentials = new NetworkCredential(_appSettings.SmtpUser, _appSettings.SmtpPass);
        }

        await client.SendMailAsync(message);
    }
}
