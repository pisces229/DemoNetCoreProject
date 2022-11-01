using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Dtos.Default;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultRequestRepository
    {
        Task Upload(DefaultRequestRepositoryUploadInputDto model);
        CommonDownloadDto Download();
    }
}
