using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultRepository
    {
        Task<bool> Upload(DefaultRepositoryUploadInputDto model);
        CommonOutputDto<CommonDownloadOutputDto> Download();
    }
}
