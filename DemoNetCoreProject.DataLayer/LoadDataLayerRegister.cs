using AutoMapper;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Registers;
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
        public static void LoadAutoMappers(IMapperConfigurationExpression configure)
        {

        }
        public static void LoadServices(IServiceCollection service)
        {
            // Service
            service.AddScoped<IDbManager<DefaultDbContext>, DbManager<DefaultDbContext>>();
            service.AddScoped<IDapperService<DefaultDbContext>, DapperService<DefaultDbContext>>();
            service.AddScoped<IMailClient, MailClient>();
            service.AddScoped<IDefaultDataProtector, DefaultDataProtector>();
            // Repositories
            DefaultDbRepositoryRegister.Load(service);
            DefaultRepositoryRegister.Load(service);
        }
    }
}
