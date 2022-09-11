using DemoNetCoreProject.Common.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IRepository<Database, Entity> 
        where Database : DbContext, IDbContext
        where Entity : class
    {
        IQueryable<Entity> Queryable();
        Task<Entity?> Get(int row);
        Task<bool> Exist(IQueryable<Entity> query);
        Task<IEnumerable<Entity>> Query(IQueryable<Entity> query);
        Task<CommonPagedResultDto<Entity>> PagedQuery(IQueryable<Entity> query, CommonPageDto commanPage);
        void Create(Entity entity);
        void CreateRange(IEnumerable<Entity> entities);
        void Modify(Entity entity);
        void ModifyRange(IEnumerable<Entity> entities);
        void Remove(Entity entity);
        void RemoveRange(IEnumerable<Entity> entities);
    }
}
