using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultFirstLogic : IDefaultFirstLogic
    {
        private readonly ILogger<DefaultFirstLogic> _logger;
        private readonly IDefaultCommonLogic _defaultCommonLogic;
        public DefaultFirstLogic(ILogger<DefaultFirstLogic> logger,
            IDefaultCommonLogic defaultCommonLogic)
        {
            _logger = logger;
            _defaultCommonLogic = defaultCommonLogic;
        }
        public async Task<CommonResponseDto<DefaultFirstLogicOutputDto>> Run(DefaultFirstLogicInputDto model)
        {
            var result = new CommonResponseDto<DefaultFirstLogicOutputDto>()
            {
                Success = true,
                Data = new DefaultFirstLogicOutputDto()
                { 
                    Value = "[DefaultFirstLogic.Run]"
                }
            };
            _logger.LogInformation("DefaultFirstLogic.Run");
            var commonRunResult = await _defaultCommonLogic.Run(new DefaultCommonLogicInputDto()
            {
                Value = model.Value,
            });
            result.Data.Value += commonRunResult.Value;
            return await Task.FromResult(result);
        }
    }
}