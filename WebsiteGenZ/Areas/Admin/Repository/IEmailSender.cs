using System.Net.Mail;
using System.Net;

namespace WebsiteGenZ.Areas.Admin.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
