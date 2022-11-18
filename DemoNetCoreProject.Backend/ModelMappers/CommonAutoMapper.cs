using AutoMapper;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.Backend.ModelMappers
{
    internal class CommonAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            LoadInput(configure);
            LoadOutput(configure);
        }
        private static void LoadInput(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<CommonPageInputModel, CommonPageInputDto>();
        }
        private static void LoadOutput(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<CommonOptionOutputDto, CommonOptionOutputModel>();
            configure.CreateMap<CommonOutputDto<string>, CommonOutputModel<string>>();
            configure.CreateMap<CommonOutputDto<int>, CommonOutputModel<int>>();
            configure.CreateMap<CommonOutputDto<long>, CommonOutputModel<long>>();
        }
    }
}
