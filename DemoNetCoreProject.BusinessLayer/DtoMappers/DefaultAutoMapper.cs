using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.BusinessLayer.DtoMappers
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
            // DefaultRequest
            configure.CreateMap<DefaultRequestLogicJsonHttpGetInputDto, 
                DefaultRequestLogicJsonOutputDto>();
            configure.CreateMap<DefaultRequestLogicJsonHttpPostInputDto, 
                DefaultRequestLogicJsonOutputDto>();
            configure.CreateMap<DefaultRequestLogicUploadInputDto, 
                DefaultRequestRepositoryUploadInputDto>();
            // DefaultFirst
            configure.CreateMap<DefaultFirstLogicInputDto, 
                DefaultSecondLogicInputDto>();
            configure.CreateMap<DefaultFirstLogicInputDto, 
                DefaultFirstRepositoryInputDto>();
            // DefaultSecond
            configure.CreateMap<DefaultSecondLogicInputDto, 
                DefaultSecondRepositoryInputDto>();
        }
        private static void LoadOutput(IMapperConfigurationExpression configure)
        {
            // DefaultRequest
            // ...
            // DefaultFirst
            // ...
            // DefaultSecond
            configure.CreateMap<DefaultSecondLogicOutputDto, 
                DefaultFirstLogicOutputDto>();
            configure.CreateMap<DefaultFirstRepositoryOutputDto, 
                DefaultFirstLogicOutputDto>();
            configure.CreateMap<DefaultSecondRepositoryOutputDto, 
                DefaultSecondLogicOutputDto>();
        }
    }
}
