using System.Net.Mail;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IMailService<T> where T : SmtpClient
    {
        Task<bool> Run(MailMessage mailMessage);
    }
}
