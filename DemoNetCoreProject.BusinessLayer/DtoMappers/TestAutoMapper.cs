using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Test;

namespace DemoNetCoreProject.BusinessLayer.DtoMappers
{
    internal class TestAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<TestLogicJsonHttpGetInputDto, TestLogicJsonOutputDto>();
            configure.CreateMap<TestLogicJsonHttpPostInputDto, TestLogicJsonOutputDto>();
        }
    }
}
