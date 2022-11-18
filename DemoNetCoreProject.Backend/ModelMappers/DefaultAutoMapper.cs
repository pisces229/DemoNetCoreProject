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
            LoadInput(configure);
            LoadOutput(configure);
        }
        private static void LoadInput(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<DefaultSignInInputModel,
                DefaultRequestLogicSignInInputDto>();

            configure.CreateMap<DefaultJsonHttpGetInputModel,
                DefaultRequestLogicJsonHttpGetInputDto>();

            configure.CreateMap<DefaultJsonHttpPostInputModel,
                DefaultRequestLogicJsonHttpPostInputDto>();

            configure.CreateMap<CommonPagedQueryInputModel<DefaultJsonHttpPostInputModel>,
                CommonPagedQueryInputDto<DefaultRequestLogicJsonHttpPostInputDto>>();

            configure.CreateMap<DefaultUploadInputModel,
                DefaultRequestLogicUploadInputDto>()
            //.ForMember(member => member.File, option => option.Ignore());
            .ForMember(member => member.File, option => option.MapFrom(source => source.File.OpenReadStream()))
            .ForMember(member => member.FileName, option => option.MapFrom(source => source.File.FileName));
        }
        private static void LoadOutput(IMapperConfigurationExpression configure)
        {
            configure.CreateMap<DefaultRequestLogicJsonOutputDto,
                DefaultJsonHttpOutputModel>();
            configure.CreateMap<CommonOutputDto<DefaultRequestLogicJsonOutputDto>,
                CommonOutputModel<DefaultJsonHttpOutputModel>>();

            configure.CreateMap<DefaultRequestLogicJsonOutputDto,
                DefaultJsonHttpOutputModel>();
            configure.CreateMap<CommonPagedQueryOutputDto<DefaultRequestLogicJsonOutputDto>,
                CommonPagedQueryOutputModel<DefaultJsonHttpOutputModel>>();
        }
    }
}
