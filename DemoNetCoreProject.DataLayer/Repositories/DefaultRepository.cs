using DemoNetCoreProject.DataLayer.IRepositories;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories
{
    internal class DefaultRepository : IDefaultRepository
    {
        private readonly DefaultDbContext _defaultDbContext;
        public DefaultRepository(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }
        public async Task<int?> MaxRow()
            => await _defaultDbContext.Customers.Select(s => s.Row).MaxAsync();
    }
}
