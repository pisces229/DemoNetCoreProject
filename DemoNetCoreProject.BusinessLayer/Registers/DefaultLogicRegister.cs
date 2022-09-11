using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.BusinessLayer.LogicRegisters
{
    internal class DefaultLogicRegister
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<IDefaultLogic, DefaultLogic>();
            services.AddScoped<IDefaultCommonLogic, DefaultCommonLogic>();
            services.AddScoped<IDefaultFirstLogic, DefaultFirstLogic>();
            services.AddScoped<IDefaultSecondLogic, DefaultSecondLogic>();
        }
    }
}
