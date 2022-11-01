using AutoMapper;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.Backend.DtoMappers
{
    internal class CommonAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<CommonPageModel, CommonPageDto>();
        }
    }
}
