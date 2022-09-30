using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class DbManager<DB> : IDbManager<DB> where DB : IDbContext
    {
        private readonly ILogger<DbManager<DB>> _logger;
        private readonly DB _dbContext;
        public DbManager(ILogger<DbManager<DB>> logger,
            DB dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
        public Task BeginTransactionAsync(IsolationLevel isolationLevel) => _dbContext.BeginTransactionAsync(isolationLevel);
        public Task CommitAsync() => _dbContext.CommitAsync();
        public Task RollbackAsync() => _dbContext.RollbackAsync();
    }
}
