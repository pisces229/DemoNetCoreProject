using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using DemoNetCoreProject.DataLayer.IRepositories.Db;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    internal class DefaultCustomerDbRepository : DbRepository<DefaultDbContext, Customer>, IDefaultCustomerDbRepository
    {
        public DefaultCustomerDbRepository(DefaultDbContext context) : base(context)
        {
        }
        public async Task<Customer?> GetByName(string name)
            => await Queryable().FirstOrDefaultAsync(x => x.Name == name);
    }
}
