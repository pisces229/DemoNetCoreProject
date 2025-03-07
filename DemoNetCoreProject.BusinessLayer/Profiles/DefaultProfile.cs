using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.BusinessLayer.Profiles
{
    internal class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<DefaultLogicFromFormInputDto, DefaultRepositoryUploadInputDto>();
        }
    }
}
