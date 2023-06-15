namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IDbSqlBulkCopy<DB> where DB : IDbContext
    {
        Task Write<T>(List<T> datas) where T : class;
    }
}
