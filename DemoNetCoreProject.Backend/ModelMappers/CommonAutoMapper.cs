using AutoMapper;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.Backend.ModelMappers
{
    internal class CommonAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            configure.CreateMap(typeof(CommonOutputDto<>), typeof(CommonOutputModel<>));
            configure.CreateMap(typeof(CommonPageOutputDto<>), typeof(CommonPageOutputModel<>));
            configure.CreateMap<CommonOptionOutputDto, CommonOptionOutputModel>();
        }
    }
}
