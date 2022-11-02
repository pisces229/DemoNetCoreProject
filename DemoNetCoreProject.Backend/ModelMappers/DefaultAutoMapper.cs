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
            configure.CreateMap<DefaultSignInModel, DefaultRequestLogicSignInInputDto>();
            configure.CreateMap<DefaultJsonHttpGetModel, DefaultRequestLogicJsonHttpGetInputDto>();
            configure.CreateMap<DefaultJsonHttpPostModel, DefaultRequestLogicJsonHttpPostInputDto>();
            configure.CreateMap<CommonPagedQueryModel<DefaultJsonHttpPostModel>, CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto>>();
            configure.CreateMap<DefaultUploadModel, DefaultRequestLogicUploadInputDto>()
                //.ForMember(member => member.File, option => option.Ignore());
                .ForMember(member => member.File, option => option.MapFrom(source => source.File.OpenReadStream()))
                .ForMember(member => member.FileName, option => option.MapFrom(source => source.File.FileName));
        }
    }
}
