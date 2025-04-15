using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    public class DefaultPersonDbRepository : DbRepository<DefaultDbContext, Person>, IDefaultPersonDbRepository
    {
        public DefaultPersonDbRepository(DefaultDbContext context) : base(context)
        {
        }
        public async Task<Person?> GetByName(string name)
            => await DbSet().FirstOrDefaultAsync(x => x.Name == name);
    }
}
