using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;

namespace DemoNetCoreProject.DataLayer.IRepositories
{
    public interface IDefaultCustomerDbRepository : IDbRepository<DefaultDbContext, Customer>
    {
        Task<Customer?> GetByName(string name);
    }
}
