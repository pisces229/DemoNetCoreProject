using DemoNetCoreProject.BusinessLayer.ILogics.Test;
using DemoNetCoreProject.BusinessLayer.Logics.Test;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.BusinessLayer.LogicRegisters
{
    internal class TestLogicRegister
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<ITestLogic, TestLogic>();
        }
    }
}
