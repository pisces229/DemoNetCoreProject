using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.Repositories.Db;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.DataLayer.Registers
{
    internal class DefaultDbRepositoryRegister
    {
        public static void Load(IServiceCollection service)
        {
            service.AddScoped<IDefaultPersonDbRepository, DefaultPersonDbRepository>();
            service.AddScoped<IDefaultAddressDbRepository, DefaultAddressDbRepository>();
        }
    }
}
