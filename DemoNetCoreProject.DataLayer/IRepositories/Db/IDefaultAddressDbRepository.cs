using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;

namespace DemoNetCoreProject.DataLayer.IRepositories.Db
{
    public interface IDefaultAddressDbRepository : IDbRepository<DefaultDbContext, Address>
    {
        Task<Address?> GetByText(string text);
    }
}
