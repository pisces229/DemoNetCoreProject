using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultRequestRepository
    {
        Task<bool> Upload(DefaultRequestRepositoryUploadInputDto model);
        CommonOutputDto<CommonDownloadOutputDto> Download();
    }
}
