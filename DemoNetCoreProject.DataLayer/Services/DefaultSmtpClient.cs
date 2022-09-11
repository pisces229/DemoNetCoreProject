using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class DefaultSmtpClient : SmtpClient
    {
        public DefaultSmtpClient(IConfiguration configuration)
            : base()
        {
            //configuration.GetValue<string>("");
            //configuration.GetValue<int>("");
            Host = "localhost";
            Port = 22;
        }
    }
}
