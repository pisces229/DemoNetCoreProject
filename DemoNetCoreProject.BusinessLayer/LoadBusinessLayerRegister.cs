using AutoMapper;
using DemoNetCoreProject.BusinessLayer.DtoMappers;
using DemoNetCoreProject.BusinessLayer.LogicRegisters;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("DemoNetCoreProject.Backend")]
//[assembly: InternalsVisibleTo("DemoNetCoreProject.Btach")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.IntegrationTest")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.UnitTest")]

namespace DemoNetCoreProject.BusinessLayer
{
    public class LoadBusinessLayerRegister
    {
        public static void LoadAutoMappers(IMapperConfigurationExpression configure)
        {
            DefaultAutoMapper.Load(configure);
        }
        public static void LoadServices(IServiceCollection service)
        {
            DefaultLogicRegister.Load(service);
        }
    }
}
