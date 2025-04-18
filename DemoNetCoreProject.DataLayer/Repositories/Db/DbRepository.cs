using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    public class DbRepository<Database, Entity>(Database context) : IDbRepository<Database, Entity>
        where Database : DbContext, IDbContext
        where Entity : class
    {
        protected readonly Database _context = context;

        protected DbSet<Entity> DbSet() => _context.Set<Entity>();
        public async Task<Entity?> Find(int row)
            => await DbSet().FindAsync(row);
        public async Task<bool> Any(Func<IQueryable<Entity>, IQueryable<Entity>>? where = null)
        {
            var queryable = DbSet().AsQueryable();
            if (where != null)
            {
                queryable = where(queryable);
            }
            return await queryable.AnyAsync();
        }
        public async Task<int> Count(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null)
        {
            var queryable = DbSet().AsQueryable();
            if (query != null)
            {
                queryable = query(queryable);
            }
            return await queryable.CountAsync();
        }
        public async Task<IEnumerable<Entity>> Query(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null)
        {
            var queryable = DbSet().AsQueryable();
            if (query != null)
            {
                queryable = query(queryable);
            }
            return await queryable.ToListAsync();
        }
        public async Task<CommonPageOutputDto<Entity>> PagedQuery(int pageSize, int pageNo,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null)
        {
            var result = new CommonPageOutputDto<Entity>();
            var queryable = DbSet().AsQueryable();
            if (where != null)
            {
                queryable = where(queryable);
            }
            result.TotalCount = await queryable.CountAsync();
            if (order != null)
            {
                queryable = order(queryable);
            }
            result.Data = await queryable
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
