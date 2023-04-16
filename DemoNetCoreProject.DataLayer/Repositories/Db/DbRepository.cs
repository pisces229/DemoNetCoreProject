using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    internal class DbRepository<Database, Entity> : IDbRepository<Database, Entity>
        where Database : DbContext, IDbContext
        where Entity : class
    {
        protected readonly Database _context;
        public DbRepository(Database context)
        {
            _context = context;
        }
        protected DbSet<Entity> DbSet() => _context.Set<Entity>();
        public async Task<Entity?> Find(int row)
            => await DbSet().FindAsync(row);
        public async Task<bool> Any(Func<IQueryable<Entity>, IQueryable<Entity>>? where = null)
        {
            var query = DbSet().AsQueryable();
            if (where != null)
            {
                query = where(query);
            }
            return await query.AnyAsync();
        }
        public async Task<int> Count(Func<IQueryable<Entity>, IQueryable<Entity>>? where = null)
        {
            var query = DbSet().AsQueryable();
            if (where != null)
            {
                query = where(query);
            }
            return await query.CountAsync();
        }
        public async Task<IEnumerable<Entity>> Query(
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null)
        {
            var query = DbSet().AsQueryable();
            if (where != null)
            {
                query = where(query);
            }
            if (order != null)
            {
                query = order(query);
            }
            return await query.ToListAsync();
        }
        public async Task<CommonPageOutputDto<Entity>> PagedQuery(int pageSize, int pageNo,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null)
        {
            var result = new CommonPageOutputDto<Entity>();
            var query = DbSet().AsQueryable();
            if (where != null)
            {
                query = where(query);
            }
            result.TotalCount = await query.CountAsync();
            if (order != null)
            {
                query = order(query);
            }
            result.Data = await query
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize)
                .ToListAsync();
            return result;
        }
        public async Task<int> Create(Entity entity)
        {
            DbSet().Add(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> CreateRange(IEnumerable<Entity> entities)
        {
            DbSet().AddRange(entities);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Modify(Entity entity)
        {
            DbSet().Update(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> ModifyRange(IEnumerable<Entity> entities)
        {
            DbSet().UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Remove(Entity entity)
        {
            DbSet().Remove(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveRange(IEnumerable<Entity> entities)
        {
            DbSet().RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }
    }
}
