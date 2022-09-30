using DemoNetCoreProject.DataLayer.Dtos.Default;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    internal class DefaultSecondRepository : IDefaultSecondRepository
    {
        private readonly ILogger<DefaultSecondRepository> _logger;
        public DefaultSecondRepository(ILogger<DefaultSecondRepository> logger)
        {
            _logger = logger;
        }
        public async Task<DefaultSecondRepositoryOutputDto> Run(DefaultSecondRepositoryInputDto model)
        {
            _logger.LogInformation($"DefaultSecondRepositoryInputDto:{JsonSerializer.Serialize(model)}");
            var result = new DefaultSecondRepositoryOutputDto() { Value = "DefaultSecondRepository.Run" };
            _logger.LogInformation($"DefaultSecondRepositoryOutputDto:{JsonSerializer.Serialize(result)}");
            return await Task.FromResult(result);
        }
    }
}
