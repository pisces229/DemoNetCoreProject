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
            // DefaultSign
            configure.CreateMap<DefaultSignInInputModel, DefaultRequestLogicSignInInputDto>();
            // DefaultJson
            configure.CreateMap<DefaultJsonHttpGetInputModel, DefaultRequestLogicJsonHttpGetInputDto>();
            configure.CreateMap<DefaultJsonHttpPostInputModel, DefaultRequestLogicJsonHttpPostInputDto>();
            configure.CreateMap<DefaultRequestLogicJsonOutputDto, DefaultJsonHttpOutputModel>();
            // DefaultUpload
            configure.CreateMap<DefaultUploadInputModel, DefaultRequestLogicUploadInputDto>()
            //.ForMember(member => member.File, option => option.Ignore());
            .ForMember(member => member.File, option => option.MapFrom(source => source.File.OpenReadStream()))
            .ForMember(member => member.FileName, option => option.MapFrom(source => source.File.FileName));
            // PagedQuery
            configure.CreateMap<DefaultPagedQueryGetInputModel, DefaultRequestLogicPagedQueryGetInputDto>();
        }
    }
}
