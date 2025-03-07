using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    internal class DefaultAddressDbRepository : DbRepository<DefaultDbContext, Address>, IDefaultAddressDbRepository
    {
        public DefaultAddressDbRepository(DefaultDbContext context) : base(context)
        {
        }
        public async Task<Address?> GetByText(string text)
            => await DbSet().FirstOrDefaultAsync(x => x.Text == text);
    }
}
