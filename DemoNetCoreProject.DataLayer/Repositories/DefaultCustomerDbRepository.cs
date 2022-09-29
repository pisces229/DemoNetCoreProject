using DemoNetCoreProject.DataLayer.IRepositories;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories
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
