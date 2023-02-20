using System.Threading.Tasks;

namespace Web.Api.Services.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toName, string toEmail, string subject, string body);
    }
}