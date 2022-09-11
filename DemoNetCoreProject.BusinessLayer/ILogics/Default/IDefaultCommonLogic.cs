using DemoNetCoreProject.BusinessLayer.Dtos.Default;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultCommonLogic
    {
        Task<DefaultCommonLogicOutputDto> Run(DefaultCommonLogicInputDto model);
    }
}
