using AutoMapper;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;

namespace DemoNetCoreProject.Backend.Profiles
{
    internal class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            // FromBody
            CreateMap<DefaultFromBodyInputModel, DefaultLogicFromBodyInputDto>();
            // FromForm
            CreateMap<DefaultFromFormInputModel, DefaultLogicFromFormInputDto>()
            .ForMember(member => member.File, option => option.MapFrom(source => source.File.OpenReadStream()));
            // FromQuery
            CreateMap<DefaultFromQueryInputModel, DefaultLogicFromQueryInputDto>();
            // PageQuery
            CreateMap<DefaultPageQueryBindInputModel, DefaultLogicPageQueryInputDto>();
            CreateMap<DefaultPageQueryJsonInputModel, DefaultLogicPageQueryInputDto>();
            CreateMap<DefaultLogicPageQueryOutputDto, DefaultPageQueryOutputModel>();
            // DefaultSign
            CreateMap<DefaultSignInInputModel, DefaultLogicSignInInputDto>();
        }
    }
}
