using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultSecondLogic : IDefaultSecondLogic
    {
        private readonly ILogger<DefaultSecondLogic> _logger;
        private readonly IDefaultCommonLogic _defaultCommonLogic;
        public DefaultSecondLogic(ILogger<DefaultSecondLogic> logger,
            IDefaultCommonLogic defaultCommonLogic)
        {
            _logger = logger;
            _defaultCommonLogic = defaultCommonLogic;
        }
        public async Task<CommonResponseDto<DefaultSecondLogicOutputDto>> Run(DefaultSecondLogicInputDto model)
        {
            var result = new CommonResponseDto<DefaultSecondLogicOutputDto>()
            {
                Success = true,
                Data = new DefaultSecondLogicOutputDto()
                {
                    Value = "[DefaultSecondLogic.Run]"
                }
            };
            _logger.LogInformation("DefaultSecondLogic.Run");
            var commonRunResult = await _defaultCommonLogic.Run(new DefaultCommonLogicInputDto()
            {
                Value = model.Value,
            });
            result.Data.Value += commonRunResult.Value;
            return await Task.FromResult(result);
        }
    }
}
