using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;

namespace DemoNetCoreProject.DataLayer.IRepositories
{
    public interface IDefaultCustomerRepository : IRepository<DefaultDbContext, Customer>
    {
        Task<Customer?> GetByName(string name);
    }
}
