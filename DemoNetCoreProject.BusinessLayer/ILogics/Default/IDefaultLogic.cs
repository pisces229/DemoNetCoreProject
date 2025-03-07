using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultLogic
    {
        Task Run();
        Task<CommonOutputDto<string>> FromBody(DefaultLogicFromBodyInputDto inputDto);
        Task<CommonOutputDto<string>> FromForm(DefaultLogicFromFormInputDto inputDto);
        Task<CommonOutputDto<string>> FromQuery(DefaultLogicFromQueryInputDto inputDto);
        Task<CommonOutputDto<CommonPageOutputDto<DefaultLogicPageQueryOutputDto>>> PageQuery(DefaultLogicPageQueryInputDto inputDto);
        Task<CommonOutputDto<CommonDownloadOutputDto>> Download();
        Task<CommonOutputDto<string>> SignIn(DefaultLogicSignInInputDto model);
        Task<CommonOutputDto<string>> Validate();
        Task<CommonOutputDto<string>> Refresh(string model);
        Task<CommonOutputDto<string>> SignOut(string model);
    }
}
