using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class Repository<Database, Entity> : IRepository<Database, Entity>
        where Database : DbContext, IDbContext
        where Entity : class
    {
        protected readonly Database _context;
        public Repository(Database context)
        {
            _context = context;
        }
        public IQueryable<Entity> Queryable() => _context.Set<Entity>().AsQueryable();
        public async Task<Entity?> Get(int row) 
            => await _context.Set<Entity>().FindAsync(row);
        public async Task<bool> Exist(IQueryable<Entity> query)
            => await query.AnyAsync();
        public async Task<IEnumerable<Entity>> Query(IQueryable<Entity> query)
            => await query.ToListAsync();
        public async Task<CommonPagedResultDto<Entity>> PagedQuery(IQueryable<Entity> query, CommonPageDto commanPage)
        {
            var result = new CommonPagedResultDto<Entity>()
            { 
                Page = commanPage
            };
            result.Page.TotalCount = await query.CountAsync();
            result.Data = await query
                .Skip((commanPage.PageNo - 1) * commanPage.PageSize)
                .Take(commanPage.PageSize)
                .ToListAsync();
            return result;
        }
        public void Create(Entity entity)
            => _context.Set<Entity>().Add(entity);
        public void CreateRange(IEnumerable<Entity> entities)
            => _context.Set<Entity>().AddRange(entities);
        public void Modify(Entity entity)
            => _context.Set<Entity>().Update(entity);
        public void ModifyRange(IEnumerable<Entity> entities)
            => _context.Set<Entity>().UpdateRange(entities);
        public void Remove(Entity entity)
            => _context.Set<Entity>().Remove(entity);
        public void RemoveRange(IEnumerable<Entity> entities)
            => _context.Set<Entity>().RemoveRange(entities);
    }
}
