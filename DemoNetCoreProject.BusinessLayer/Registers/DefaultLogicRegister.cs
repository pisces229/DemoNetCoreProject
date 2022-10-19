using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.BusinessLayer.LogicRegisters
{
    internal class DefaultLogicRegister
    {
        public static void Load(IServiceCollection service)
        {
            service.AddScoped<IDefaultLogic, DefaultLogic>();
            service.AddScoped<IDefaultRequestLogic, DefaultRequestLogic>();
            service.AddScoped<IDefaultFirstLogic, DefaultFirstLogic>();
            service.AddScoped<IDefaultSecondLogic, DefaultSecondLogic>();
        }
    }
}
