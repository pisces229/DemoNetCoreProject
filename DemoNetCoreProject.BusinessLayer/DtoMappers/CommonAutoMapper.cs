using AutoMapper;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.DtoMappers
{
    internal class CommonAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            configure.CreateMap(typeof(CommonOutputDto<>), typeof(CommonOutputDto<>));
            configure.CreateMap(typeof(CommonPageOutputDto<>), typeof(CommonPageOutputDto<>));
        }
    }
}
