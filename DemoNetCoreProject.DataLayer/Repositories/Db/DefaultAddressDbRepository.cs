using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using DemoNetCoreProject.DataLayer.IRepositories.Db;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    internal class DefaultAddressDbRepository : DbRepository<DefaultDbContext, Address>, IDefaultAddressDbRepository
    {
        public DefaultAddressDbRepository(DefaultDbContext context) : base(context)
        {
        }
        public async Task<Address?> GetByText(string text)
            => await Queryable().FirstOrDefaultAsync(x => x.Text == text);
    }
}
