using DemoNetCoreProject.DataLayer.Entities;
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
        #region DbSet
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            #region Builder Entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Row);

                entity.ToTable("Customer");

                entity.HasIndex(e => e.Id, "IDX__Customer__Id")
                    .IsUnique();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(50);
            });
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        protected void UpdateInfomation()
        {
            foreach (var entrie in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                //switch (entrie.State)
                //{
                //    case EntityState.Added:
                //        entrie.Property("CREATE_USER").CurrentValue = _userService.Userid();
                //        entrie.Property("CREATE_DATETIME").CurrentValue = DateTime.Now;
                //        entrie.Property("UPDATE_USER").CurrentValue = _userService.Userid();
                //        entrie.Property("UPDATE_DATETIME").CurrentValue = DateTime.Now;
                //        entrie.Property("PROG_CD").CurrentValue = _userService.Proid();
                //        break;
                //    case EntityState.Modified:
                //        entrie.Property("UPDATE_USER").CurrentValue = _userService.Userid();
                //        entrie.Property("UPDATE_DATETIME").CurrentValue = DateTime.Now;
                //        entrie.Property("PROG_CD").CurrentValue = _userService.Proid();
                //        break;
                //}
            }
        }
        #region Expansion
        public override int SaveChanges()
        {
            UpdateInfomation();
            return base.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            UpdateInfomation();
            return base.SaveChangesAsync();
        }
        public DatabaseFacade GetDatabase() => this.Database;
        public async Task<DbConnection> GetDbConnection()
        {
            await OpenConnection();
            return this.Database.GetDbConnection();
        }
        private async Task OpenConnection()
        {
            if (!this.Database.IsInMemory())
            {
                if (this.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    await this.Database.OpenConnectionAsync();
                }
            }
        }
        public DbTransaction GetDbTransaction() => this.DbTransaction!;
        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (!this.Database.IsInMemory())
            {
                if (!this.Database.IsInMemory())
                {
                    await OpenConnection();
                    DbTransaction = await this.Database.GetDbConnection().BeginTransactionAsync(isolationLevel);
                    await this.Database.UseTransactionAsync(DbTransaction);
                }
            }
        }
        public async Task CommitAsync()
        {
            if (!this.Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    await this.Database.CommitTransactionAsync();
                    DbTransaction = null;
                }
            }
        }
        public async Task RollbackAsync()
        {
            if (!this.Database.IsInMemory())
            {
                if (DbTransaction != null)
                {
                    await this.Database.RollbackTransactionAsync();
                    DbTransaction = null;
                }
            }
        }
        public override async void Dispose()
        {
            try
            {
                await RollbackAsync();
            }
            catch
            {
                // pass
            }
            base.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
