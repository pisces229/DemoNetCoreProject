using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.BusinessLayer.DtoMappers
{
    internal class DefaultAutoMapper
    {
        public static void Load(IMapperConfigurationExpression configure)
        {
            // DefaultRequest
            configure.CreateMap<DefaultRequestLogicJsonHttpGetInputDto, DefaultRequestLogicJsonOutputDto>();
            configure.CreateMap<DefaultRequestLogicJsonHttpPostInputDto, DefaultRequestLogicJsonOutputDto>();
            configure.CreateMap<DefaultRequestLogicUploadInputDto, DefaultRequestRepositoryUploadInputDto>();
            // DefaultFirstLogicInputDto
            configure.CreateMap<DefaultFirstLogicInputDto, DefaultSecondLogicInputDto>();
            configure.CreateMap<DefaultFirstLogicInputDto, DefaultFirstRepositoryInputDto>();
            // DefaultFirstLogicOutputDto
            // ...
            // DefaultSecondLogicInputDto
            configure.CreateMap<DefaultSecondLogicInputDto, DefaultSecondRepositoryInputDto>();
            // DefaultSecondLogicOutputDto
            configure.CreateMap<DefaultSecondLogicOutputDto, DefaultFirstLogicOutputDto>();
            // DefaultFirstRepositoryOutputDto
            configure.CreateMap<DefaultFirstRepositoryOutputDto, DefaultFirstLogicOutputDto>();
            // DefaultSecondRepositoryOutputDto
            configure.CreateMap<DefaultSecondRepositoryOutputDto, DefaultSecondLogicOutputDto>();
        }
    }
}
