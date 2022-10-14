using System.Data;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IDbManager<DB> where DB : IDbContext
    {
        Task<int> SaveChangesAsync();
        void EntityDetached();
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
