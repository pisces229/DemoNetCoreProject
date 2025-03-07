using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.IRepositories.Db
{
    public interface IDbRepository<Database, Entity>
        where Database : DbContext, IDbContext
        where Entity : class
    {
        Task<Entity?> Find(int row);
        Task<bool> Any(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null);
        Task<int> Count(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null);
        Task<IEnumerable<Entity>> Query(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null);
        Task<CommonPageOutputDto<Entity>> PagedQuery(int pageSize, int pageNo,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null);
        Task<int> Create(Entity entity);
        Task<int> CreateRange(IEnumerable<Entity> entities);
        Task<int> Modify(Entity entity);
        Task<int> ModifyRange(IEnumerable<Entity> entities);
        Task<int> Remove(Entity entity);
        Task<int> RemoveRange(IEnumerable<Entity> entities);
    }
}
