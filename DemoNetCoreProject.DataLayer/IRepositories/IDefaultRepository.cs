using System;

namespace DemoNetCoreProject.DataLayer.IRepositories
{
    public interface IDefaultRepository
    {
        Task<int?> MaxRow();
    }
}
