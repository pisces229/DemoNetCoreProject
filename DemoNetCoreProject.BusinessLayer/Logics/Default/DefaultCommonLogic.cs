using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultCommonLogic : IDefaultCommonLogic
    {
        private readonly ILogger<DefaultCommonLogic> _logger;
        public DefaultCommonLogic(ILogger<DefaultCommonLogic> logger) 
        {
            _logger = logger;
        }
        public async Task<DefaultCommonLogicOutputDto> Run(DefaultCommonLogicInputDto model)
        {
            var result = new DefaultCommonLogicOutputDto
            {
                Value = "[DefaultCommonLogic.Run]"
            };
            _logger.LogInformation("DefaultCommonLogic.Run");
            return await Task.FromResult(result);
        }
    }
}
