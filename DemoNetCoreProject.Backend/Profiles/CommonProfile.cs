using AutoMapper;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.Backend.Profiles
{
    internal class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(CommonOutputDto<>), typeof(CommonOutputModel<>));
            CreateMap(typeof(CommonPageOutputDto<>), typeof(CommonPageOutputModel<>));
            CreateMap<CommonOptionOutputDto, CommonOptionOutputModel>();
        }
    }
}
