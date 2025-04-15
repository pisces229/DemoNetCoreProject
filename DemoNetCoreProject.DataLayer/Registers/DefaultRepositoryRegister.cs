using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.DataLayer.Registers
{
    public class DefaultRepositoryRegister
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<IDefaultSqlRepository, DefaultSqlRepository>();
            services.AddScoped<IDefaultRepository, DefaultRepository>();
        }
    }
}
