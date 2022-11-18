using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultRequestLogic
    {
        void Run();
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpGet(DefaultRequestLogicJsonHttpGetInputDto model);
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpPost(DefaultRequestLogicJsonHttpPostInputDto model);
        Task<CommonPagedQueryOutputDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQuery(CommonPagedQueryInputDto<DefaultRequestLogicJsonHttpPostInputDto> model);
        Task<CommonOutputDto<string>> Upload(DefaultRequestLogicUploadInputDto model);
        Task<CommonOutputDto<CommonDownloadOutputDto>> Download();
        Task<CommonOutputDto<string>> SignIn(DefaultRequestLogicSignInInputDto model);
        Task<CommonOutputDto<string>> Validate();
        Task<CommonOutputDto<string>> Refresh(string model);
        Task SignOut(string model);
    }
}
