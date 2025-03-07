using AutoMapper;
using DemoNetCoreProject.BusinessLayer.LogicRegisters;
using DemoNetCoreProject.BusinessLayer.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("DemoNetCoreProject.Backend")]
//[assembly: InternalsVisibleTo("DemoNetCoreProject.Batch")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.IntegrationTest")]
[assembly: InternalsVisibleTo("DemoNetCoreProject.UnitTest")]

namespace DemoNetCoreProject.BusinessLayer
{
    public class LoadBusinessLayerRegister
    {
        public static IEnumerable<Profile> Profiles() => new Profile[]
        {
            new CommonProfile(),
            new DefaultProfile(),
        };
        public static void LoadServices(IServiceCollection service)
        {
            DefaultLogicRegister.Load(service);
        }
    }
}
