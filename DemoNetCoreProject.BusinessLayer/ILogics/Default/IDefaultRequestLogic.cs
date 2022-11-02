using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultRequestLogic
    {
        void Run();
        Task<CommonOutputDto<string>> SignIn(DefaultRequestLogicSignInInputDto model);
        Task<CommonOutputDto<string>> Validate();
        Task<CommonOutputDto<string>> Refresh(string model);
        Task SignOut(string model);
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpGet(DefaultRequestLogicJsonHttpGetInputDto model);
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpPost(DefaultRequestLogicJsonHttpPostInputDto model);
        Task<CommonPagedResultDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQuery(CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto> model);
        Task<CommonOutputDto<string>> Upload(DefaultRequestLogicUploadInputDto model);
        Task<CommonOutputDto<CommonDownloadDto>> Download();
    }
}
