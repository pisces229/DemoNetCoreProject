using System.Net.Mail;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IMailClient
    {
        Task<bool> Run(MailMessage mailMessage);
    }
}
