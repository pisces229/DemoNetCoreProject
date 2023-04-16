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
            configure.CreateMap<DefaultLogicFromFormInputDto, 
                DefaultRepositoryUploadInputDto>();

        }
        private static void LoadOutput(IMapperConfigurationExpression configure)
        {
            // DefaultRequest
        }
    }
}
