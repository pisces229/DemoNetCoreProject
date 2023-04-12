using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultRequestLogic
    {
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpGet(DefaultRequestLogicJsonHttpGetInputDto model);
        Task<CommonOutputDto<DefaultRequestLogicJsonOutputDto>> JsonHttpPost(DefaultRequestLogicJsonHttpPostInputDto model);
        Task<CommonPagedQueryOutputDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQueryGet(DefaultRequestLogicPagedQueryGetInputDto model);
        Task<CommonPagedQueryOutputDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQueryPost(CommonPagedQueryInputDto<DefaultRequestLogicJsonHttpPostInputDto> model);
        Task<CommonOutputDto<string>> Upload(DefaultRequestLogicUploadInputDto model);
        Task<CommonOutputDto<CommonDownloadOutputDto>> Download();
        Task<CommonOutputDto<string>> SignIn(DefaultRequestLogicSignInInputDto model);
        Task<CommonOutputDto<string>> Validate();
        Task<CommonOutputDto<string>> Refresh(string model);
        Task<CommonOutputDto<string>> SignOut(string model);
    }
}
