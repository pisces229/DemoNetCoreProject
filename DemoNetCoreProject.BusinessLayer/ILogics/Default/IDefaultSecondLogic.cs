using DemoNetCoreProject.BusinessLayer.Dtos.Default;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultSecondLogic
    {
        Task<DefaultSecondLogicOutputDto> Run(DefaultSecondLogicInputDto model);
    }
}
