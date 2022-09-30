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
        Task<bool> Any(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null);
        Task<int> Count(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null);
        Task<IEnumerable<Entity>> Query(
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null);
        Task<CommonPagedResultDto<Entity>> PagedQuery(CommonPageDto commonPage,
            bool tracking = true,
            Func<IQueryable<Entity>, IQueryable<Entity>>? where = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? order = null);
        void Create(Entity entity);
        void CreateRange(IEnumerable<Entity> entities);
        void Modify(Entity entity);
        void ModifyRange(IEnumerable<Entity> entities);
        void Remove(Entity entity);
        void RemoveRange(IEnumerable<Entity> entities);
    }
}
