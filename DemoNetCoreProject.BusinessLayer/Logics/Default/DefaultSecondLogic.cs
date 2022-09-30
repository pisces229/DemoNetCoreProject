using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultSecondLogic : IDefaultSecondLogic
    {
        private readonly ILogger<DefaultSecondLogic> _logger;
        private readonly IDefaultSecondRepository _defaultSecondRepository;
        private readonly IMapper _mapper;
        public DefaultSecondLogic(ILogger<DefaultSecondLogic> logger,
            IDefaultSecondRepository defaultSecondRepository,
            IMapper mapper)
        {
            _logger = logger;
            _defaultSecondRepository = defaultSecondRepository;
            _mapper = mapper;
        }
        public async Task<DefaultSecondLogicOutputDto> Run(DefaultSecondLogicInputDto model)
        {
            _logger.LogInformation($"DefaultSecondLogicInputDto:{JsonSerializer.Serialize(model)}");
            var result = _mapper.Map<DefaultSecondRepositoryOutputDto, DefaultSecondLogicOutputDto>(
                await _defaultSecondRepository.Run(
                    _mapper.Map<DefaultSecondLogicInputDto, DefaultSecondRepositoryInputDto>(model)));
            _logger.LogInformation($"DefaultSecondLogicOutputDto:{JsonSerializer.Serialize(result)}");
            return await Task.FromResult(result);
        }
    }
}
