using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;

namespace DemoNetCoreProject.DataLayer.IRepositories.Db
{
    public interface IDefaultPersonDbRepository : IDbRepository<DefaultDbContext, Person>
    {
        Task<Person?> GetByName(string name);
    }
}
