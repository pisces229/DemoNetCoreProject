using DemoNetCoreProject.DataLayer.IServices;
using System.Net.Mail;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class DefaultMailService<T> : IMailService<T>, IDisposable where T : SmtpClient
    {
        private readonly T _SmtpClient;
        public DefaultMailService(T smtpClient)
        {
            _SmtpClient = smtpClient;
        }
        public async Task<bool> Run(MailMessage mailMessage)
        {
            var result = false;
            try
            {
                await _SmtpClient.SendMailAsync(mailMessage);
                result = true;
            }
            finally
            {
                mailMessage.Dispose();
            }
            return result;
        }
        public void Dispose()
        {
            _SmtpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
