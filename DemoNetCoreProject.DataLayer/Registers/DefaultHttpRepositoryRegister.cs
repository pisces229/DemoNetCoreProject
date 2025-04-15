using DemoNetCoreProject.DataLayer.IRepositories.Http;
using DemoNetCoreProject.DataLayer.Repositories.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.DataLayer.Registers
{
    public class DefaultHttpRepositoryRegister
    {
        public static void Load(IServiceCollection service)
        {
            service.AddScoped<IDefaultHttpRepository, DefaultHttpRepository>();
        }
    }
}
