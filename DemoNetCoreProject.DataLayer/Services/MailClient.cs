using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class MailClient : IMailClient, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        public MailClient(IConfiguration configuration)
        {
            //configuration.GetValue<string>("Host");
            //configuration.GetValue<int>("Port");
            _smtpClient = new SmtpClient()
            {
                Host = "localhost",
                Port = 22,
            };
        }
        public async Task<bool> Run(MailMessage mailMessage)
        {
            var result = false;
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
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
            _smtpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
