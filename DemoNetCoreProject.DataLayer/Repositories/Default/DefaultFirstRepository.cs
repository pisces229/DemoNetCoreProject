using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    internal class DefaultFirstRepository : IDefaultFirstRepository
    {
        private readonly ILogger<DefaultFirstRepository> _logger;
        public DefaultFirstRepository(ILogger<DefaultFirstRepository> logger)
        {
            _logger = logger;
        }
        public async Task<DefaultFirstRepositoryOutputDto> Run(DefaultFirstRepositoryInputDto model)
        {
            _logger.LogInformation($"DefaultFirstRepositoryInputDto:{JsonSerializer.Serialize(model)}");
            var result = new DefaultFirstRepositoryOutputDto() { Value = "DefaultFirstRepository.Run" };
            _logger.LogInformation($"DefaultFirstRepositoryOutputDto:{JsonSerializer.Serialize(result)}");
            return await Task.FromResult(result);
        }
    }
}
