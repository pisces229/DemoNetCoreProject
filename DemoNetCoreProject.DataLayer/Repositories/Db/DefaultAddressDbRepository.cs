using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Db
{
    public class DefaultAddressDbRepository(DefaultDbContext context) : DbRepository<DefaultDbContext, Address>(context), IDefaultAddressDbRepository
    {
        public async Task<Address?> GetByText(string text)
            => await DbSet().FirstOrDefaultAsync(x => x.Text == text);
    }
}
