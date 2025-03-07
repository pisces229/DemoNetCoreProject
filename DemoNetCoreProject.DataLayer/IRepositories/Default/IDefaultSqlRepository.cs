namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDefaultSqlRepository
    {
        Task<int?> MaxRow();
        Task RunDapperQuery();
        Task RunDapperExecuteScalar();
        Task RunDapperQueryMultiple();
        Task RunDapperExecuteReader();
        Task RunDapperPagedQuery();
        Task RunDapperExcute();
        Task RunSqlCondition();
    }
}
