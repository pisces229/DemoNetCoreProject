using AutoMapper;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Profiles;
using DemoNetCoreProject.DataLayer.Registers;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DemoNetCoreProject.Backend")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.Batch")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.IntegrationTest")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.UnitTest")]

namespace DemoNetCoreProject.DataLayer
{
    public class LoadDataLayerRegister
    {
        public static IEnumerable<Profile> Profiles() => new Profile[]
        {
            new CommonProfile(),
            new DefaultProfile(),
        };
        public static void LoadServices(IServiceCollection service)
        {
            // Service
            service.AddScoped<IDbManager<DefaultDbContext>, DbManager<DefaultDbContext>>();
            service.AddScoped<IDapperService<DefaultDbContext>, DapperService<DefaultDbContext>>();
            service.AddScoped<IFileManager, FileManager>();
            service.AddScoped<IMailClient, MailClient>();
            // Repositories
            DefaultDbRepositoryRegister.Load(service);
            DefaultHttpRepositoryRegister.Load(service);
            DefaultRepositoryRegister.Load(service);
        }
    }
}
