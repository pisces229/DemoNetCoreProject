using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultFirstLogic
    {
        Task<CommonOutputDto<DefaultFirstLogicOutputDto>> Run(DefaultFirstLogicInputDto model);
    }
}
