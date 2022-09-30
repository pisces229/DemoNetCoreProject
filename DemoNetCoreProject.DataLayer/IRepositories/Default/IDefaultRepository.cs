using System;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultRepository
    {
        Task<int?> MaxRow();
        Task RunDapperQuery();
        Task RunDapperExecuteScalar();
        Task RunDapperQueryMultiple();
        Task RunDapperExecuteReader();
        Task RunDapperPagedQuery();
        Task RunSqlCondition();
    }
}
