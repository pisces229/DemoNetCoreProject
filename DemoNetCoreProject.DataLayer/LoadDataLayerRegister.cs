using DemoNetCoreProject.DataLayer.IRepositories;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DemoNetCoreProject.Backend")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.Batch")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.DataLayer")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.IntegrationTest")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.UnitTest")]

namespace DemoNetCoreProject.DataLayer
{
    public class LoadDataLayerRegister
    {
        public static void LoadServices(IServiceCollection service)
        {
            // Services
            service.AddScoped<DefaultSmtpClient>();
            service.AddScoped<IDapperService<DefaultDbContext>, DapperService<DefaultDbContext>>();
            service.AddScoped<IMailService<DefaultSmtpClient>, DefaultMailService<DefaultSmtpClient>>();
            // Repositorys
            service.AddScoped<IDefaultCustomerRepository, DefaultCustomerRepository>();
            // Managers
        }
    }
}
