using AutoMapper;
using DemoNetCoreProject.Backend.DtoMappers;

namespace DemoNetCoreProject.Backend
{
    public class LoadBackendRegister
    {
        public static void LoadAutoMappers(IMapperConfigurationExpression configure)
        {
            CommonAutoMapper.Load(configure);
            DefaultAutoMapper.Load(configure);
        }
    }
}
