using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultRequestLogic
    {
        Task<CommonResponseDto<string>> SignIn(DefaultRequestLogicSignInInputDto model);
        Task<CommonResponseDto<string>> Validate();
        Task<CommonResponseDto<string>> Refresh(string model);
        Task SignOut(string model);
        Task<CommonResponseDto<DefaultRequestLogicJsonOutputDto>> JsonHttpGet(DefaultRequestLogicJsonHttpGetInputDto model);
        Task<CommonResponseDto<DefaultRequestLogicJsonOutputDto>> JsonHttpPost(DefaultRequestLogicJsonHttpPostInputDto model);
        Task<CommonPagedResultDto<DefaultRequestLogicJsonOutputDto>> CommonPagedQuery(CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto> model);
        Task<CommonResponseDto<string>> Upload(DefaultRequestLogicUploadInputDto model);
        Task<CommonResponseDto<CommonDownloadDto>> Download();
    }
}
