namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultSqlLogic
    {
        Task RunDbRepositoryQuery();
        Task RunDbRepositoryCreate();
        Task RunDbRepositoryModify();
        Task RunDbRepositoryRemove();
        Task RunDbRepositoryPagedQuery();
        Task RunDapperQuery();
        Task RunDapperExecuteScalar();
        Task RunDapperQueryMultiple();
        Task RunDapperExecuteReader();
        Task RunDapperPagedQuery();
        Task RunSqlCondition();
    }
}
