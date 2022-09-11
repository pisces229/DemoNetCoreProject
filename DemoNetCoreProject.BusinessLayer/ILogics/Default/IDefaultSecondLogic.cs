using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultSecondLogic
    {
        Task<CommonResponseDto<DefaultSecondLogicOutputDto>> Run(DefaultSecondLogicInputDto model);
    }
}
