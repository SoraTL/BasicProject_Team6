using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebsiteGenZ.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("hieubp301@gmail.com", "vvduzepjvxqvaded")
            };
            return client.SendMailAsync(
                new MailMessage(from: "hieubp301@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
