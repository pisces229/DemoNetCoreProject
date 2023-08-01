using AutoMapper;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.Profiles
{
    internal class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(CommonOutputDto<>), typeof(CommonOutputDto<>));
            CreateMap(typeof(CommonPageOutputDto<>), typeof(CommonPageOutputDto<>));
        }
    }
}
