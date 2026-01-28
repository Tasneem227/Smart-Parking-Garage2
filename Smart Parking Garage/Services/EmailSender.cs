using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Smart_Parking_Garage.Settings;
using System.Net;
using System.Net.Mail;

namespace Smart_Parking_Garage.Services;

public class EmailSender(IOptions<MailSettings> options, IConfiguration config) :IEmailSender
{
    private readonly MailSettings _Options = options.Value;
    private readonly IConfiguration _config = config;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string host = _Options.host;
        int port = Convert.ToInt32(_Options.port);
        string userName = _Options.UserName;
        string password = _Options.password;

        var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(userName, password)
        };
        using var message = new MailMessage(userName, email, subject, htmlMessage);
        message.IsBodyHtml = true;

        await client.SendMailAsync(message);

    }


}
