using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;

namespace DemoNetCoreProject.DataLayer.Services
{
    public partial class DefaultDbContext : DbContext, IDbContext
    {
        public int ConnectionTimeout { get; protected set; }
        protected DbTransaction? DbTransaction { get; set; }
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
            : base(options)
        {
        }
        #region IDbContext
        public override int SaveChanges()
        {
            EntityDefaultProperty();
            var result = base.SaveChanges();
            EntityDetached();
            return result;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EntityDefaultProperty();
            var result = await base.SaveChangesAsync(cancellationToken);
            EntityDetached();
            return result;
        }
        private void EntityDefaultProperty()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                //switch (entrie.State)
                //{
                //    case EntityState.Added:
                //        entry.Property("CREATE_USER").CurrentValue = _userService.Userid();
                //        entry.Property("CREATE_DATETIME").CurrentValue = DateTime.Now;
                //        entry.Property("UPDATE_USER").CurrentValue = _userService.Userid();
                //        entry.Property("UPDATE_DATETIME").CurrentValue = DateTime.Now;
                //        entry.Property("PROG_CD").CurrentValue = _userService.Proid();
                //        break;
                //    case EntityState.Modified:
                //        entry.Property("UPDATE_USER").CurrentValue = _userService.Userid();
                //        entry.Property("UPDATE_DATETIME").CurrentValue = DateTime.Now;
                //        entry.Property("PROG_CD").CurrentValue = _userService.Proid();
                //        break;
                //}
            }
        }
        private void EntityDetached()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State != EntityState.Detached))
            {
                entry.State = EntityState.Detached;
            }
        }
        public DatabaseFacade GetDatabase() => Database;
        public async Task<DbConnection> GetDbConnection()
        {
            await OpenConnection();
            return Database.GetDbConnection();
        }
        private async Task OpenConnection()
        {
            if (!Database.IsInMemory())
            {
                if (Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    await Database.OpenConnectionAsync();
                }
            }
        }
        public DbTransaction GetDbTransaction() => DbTransaction!;
        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (!Database.IsInMemory())
            {
                if (!Database.IsInMemory())
                {
                    await OpenConnection();
                    DbTransaction = await Database.GetDbConnection().BeginTransactionAsync(isolationLevel);
                    await Database.UseTransactionAsync(DbTransaction);
                }
            }
        }
        public void Commit()
        {
            if (!Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Commit();
                    DbTransaction = null;
                }
            }
        }
        public async Task CommitAsync()
        {
            if (!Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    await DbTransaction.CommitAsync();
                    DbTransaction = null;
                }
            }
        }
        public void Rollback()
        {
            if (!Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Rollback();
                    DbTransaction = null;
                }
            }
        }
        public async Task RollbackAsync()
        {
            if (!Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    await DbTransaction.RollbackAsync();
                    DbTransaction = null;
                }
            }
        }
        public override void Dispose()
        {
            try
            {
                Rollback();
            }
            catch
            {
                // pass
            }
            base.Dispose();
            GC.SuppressFinalize(this);
        }
        public override async ValueTask DisposeAsync()
        {
            try
            {
                await RollbackAsync();
            }
            catch
            {
                // pass
            }
            await base.DisposeAsync();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
