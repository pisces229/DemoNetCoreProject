using DemoNetCoreProject.BusinessLayer.Dtos.Test;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Test
{
    public interface ITestLogic
    {
        Task<CommonResponseDto<string>> SignIn(TestLogicSignInInputDto model);
        Task<CommonResponseDto<string>> Validate();
        Task<CommonResponseDto<string>> Refresh(string model);
        Task SignOut(string model);
        Task<CommonResponseDto<TestLogicJsonOutputDto>> JsonHttpGet(TestLogicJsonHttpGetInputDto model);
        Task<CommonResponseDto<TestLogicJsonOutputDto>> JsonHttpPost(TestLogicJsonHttpPostInputDto model);
        Task<CommonPagedResultDto<TestLogicJsonOutputDto>> CommonPagedQuery(CommonPagedQueryDto<TestLogicJsonHttpPostInputDto> model);
        Task<CommonResponseDto<string>> Upload(TestLogicUploadInputDto model);
        Task<CommonResponseDto<CommonDownloadDto>> Download();
    }
}
