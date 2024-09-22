using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetCoreProject.BusinessLayer.LogicRegisters
{
    internal class DefaultLogicRegister
    {
        public static void Load(IServiceCollection service)
        {
            service.AddScoped<IDefaultSqlLogic, DefaultSqlLogic>();
            service.AddScoped<IDefaultLogic, DefaultLogic>();
            //service.AddKeyedScoped<IDefaultLogic, DefaultLogic>("");
        }
    }
}
