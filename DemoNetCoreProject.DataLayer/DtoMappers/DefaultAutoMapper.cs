using AutoMapper;

namespace DemoNetCoreProject.DataLayer.DtoMappers
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
        }
        private static void LoadOutput(IMapperConfigurationExpression configure)
        {
        }
    }
}
