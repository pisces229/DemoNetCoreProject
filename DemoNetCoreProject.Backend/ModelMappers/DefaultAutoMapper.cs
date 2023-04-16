using AutoMapper;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.Backend.ModelMappers
{
    internal class DefaultAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            // FromBody
            configure.CreateMap<DefaultFromBodyInputModel, DefaultLogicFromBodyInputDto>();
            // FromForm
            configure.CreateMap<DefaultFromFormInputModel, DefaultLogicFromFormInputDto>()
            .ForMember(member => member.File, option => option.MapFrom(source => source.File.OpenReadStream()));
            // FromQuery
            configure.CreateMap<DefaultFromQueryInputModel, DefaultLogicFromQueryInputDto>();
            // PageQuery
            configure.CreateMap<DefaultPageQueryBindInputModel, DefaultLogicPageQueryInputDto>();
            configure.CreateMap<DefaultPageQueryJsonInputModel, DefaultLogicPageQueryInputDto>();
            configure.CreateMap<DefaultLogicPageQueryOutputDto, DefaultPageQueryOutputModel>();
            // DefaultSign
            configure.CreateMap<DefaultSignInInputModel, DefaultLogicSignInInputDto>();
        }
    }
}
