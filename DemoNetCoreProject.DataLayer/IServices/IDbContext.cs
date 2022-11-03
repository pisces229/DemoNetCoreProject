using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IDbContext
    {
        DatabaseFacade GetDatabase();
        Task<DbConnection> GetDbConnection();
        DbTransaction GetDbTransaction();
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
