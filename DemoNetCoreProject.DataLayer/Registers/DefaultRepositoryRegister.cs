using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.DataLayer.Registers
{
    internal class DefaultRepositoryRegister
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<IDefaultRepository, DefaultRepository>();
            services.AddScoped<IDefaultFirstRepository, DefaultFirstRepository>();
            services.AddScoped<IDefaultSecondRepository, DefaultSecondRepository>();
        }
    }
}
