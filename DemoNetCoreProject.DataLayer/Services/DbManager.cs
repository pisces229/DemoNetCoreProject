using DemoNetCoreProject.DataLayer.IServices;
using System.Data;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class DbManager<DbContext> : IDbManager<DbContext> where DbContext : IDbContext
    {
        private readonly DbContext _dbContext;
        public DbManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task BeginTransactionAsync(IsolationLevel isolationLevel) => _dbContext.BeginTransactionAsync(isolationLevel);
        public Task CommitAsync() => _dbContext.CommitAsync();
        public Task RollbackAsync() => _dbContext.RollbackAsync();
    }
}
