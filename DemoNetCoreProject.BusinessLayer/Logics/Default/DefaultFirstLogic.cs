using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultFirstLogic : IDefaultFirstLogic
    {
        private readonly ILogger<DefaultFirstLogic> _logger;
        private readonly IDefaultSecondLogic _defaultSecondLogic;
        private readonly IDefaultFirstRepository _defaultFirstRepository;
        private readonly IMapper _mapper;
        public DefaultFirstLogic(ILogger<DefaultFirstLogic> logger,
            IDefaultSecondLogic defaultSecondLogic,
            IDefaultFirstRepository defaultFirstRepository,
            IMapper mapper)
        {
            _logger = logger;
            _defaultSecondLogic = defaultSecondLogic;
            _defaultFirstRepository = defaultFirstRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponseDto<DefaultFirstLogicOutputDto>> Run(DefaultFirstLogicInputDto model)
        {
            var result = new CommonResponseDto<DefaultFirstLogicOutputDto>()
            {
                Success = true
            };
            _logger.LogInformation($"DefaultFirstLogicInputDto:{JsonSerializer.Serialize(model)}");
            await _defaultFirstRepository.Run(_mapper.Map<DefaultFirstLogicInputDto, DefaultFirstRepositoryInputDto>(model));
            result.Data = _mapper.Map<DefaultSecondLogicOutputDto, DefaultFirstLogicOutputDto>(
                await _defaultSecondLogic.Run(
                    _mapper.Map<DefaultFirstLogicInputDto, DefaultSecondLogicInputDto>(model)));
            _logger.LogInformation($"DefaultFirstLogicOutputDto:{JsonSerializer.Serialize(result.Data)}");
            return await Task.FromResult(result);
        }
    }
}