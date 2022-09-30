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
        protected IQueryable<Entity> Queryable(bool tracking = true)
            => tracking ? _context.Set<Entity>().AsTracking() : _context.Set<Entity>().AsNoTracking();
        public async Task<Entity?> Find(int row)
            => await _context.Set<Entity>().FindAsync(row);
        public async Task<bool> Any(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null)
        {
            var query = Queryable(tracking);
            if (where != null)
            {
                query = where(query);
            }
            return await query.AnyAsync();
        }
        public async Task<int> Count(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null)
        {
            var query = Queryable(tracking);
            if (where != null)
            {
                query = where(query);
            }
            return await query.CountAsync();
        }
        public async Task<IEnumerable<Entity>> Query(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null)
        {
            var query = Queryable(tracking);
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
        public async Task<CommonPagedResultDto<Entity>> PagedQuery(CommonPageDto commonPage,
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null)
        {
            var result = new CommonPagedResultDto<Entity>()
            {
                Page = commonPage
            };
            var query = Queryable(tracking);
            if (where != null)
            {
                query = where(query);
            }
            result.Page.TotalCount = await query.CountAsync();
            if (order != null)
            {
                query = order(query);
            }
            result.Data = await query
                .Skip((commonPage.PageNo - 1) * commonPage.PageSize)
                .Take(commonPage.PageSize)
                .ToListAsync();
            return result;
        }
        public void Create(Entity entity) => _context.Set<Entity>().Add(entity);
        public void CreateRange(IEnumerable<Entity> entities) => _context.Set<Entity>().AddRange(entities);
        public void Modify(Entity entity) => _context.Set<Entity>().Update(entity);
        public void ModifyRange(IEnumerable<Entity> entities) => _context.Set<Entity>().UpdateRange(entities);
        public void Remove(Entity entity) => _context.Set<Entity>().Remove(entity);
        public void RemoveRange(IEnumerable<Entity> entities) => _context.Set<Entity>().RemoveRange(entities);
    }
}
