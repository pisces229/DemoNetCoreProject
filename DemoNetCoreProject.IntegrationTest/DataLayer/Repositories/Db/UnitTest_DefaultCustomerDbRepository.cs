using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.DataLayer.Repositories.Db
{
    [TestClass]
    public class UnitTest_DefaultCustomerDbRepository : UnitTestInitialize
    {
        private readonly ILogger<UnitTest_DefaultCustomerDbRepository> _logger;
        private readonly IDefaultPersonDbRepository _repository;
        private readonly IDbManager<DefaultDbContext> _dbContext;
        public UnitTest_DefaultCustomerDbRepository() : base()
        {
            _logger = _host.Services.GetRequiredService<ILogger<UnitTest_DefaultCustomerDbRepository>>();
            _repository = _host.Services.GetRequiredService<IDefaultPersonDbRepository>();
            _dbContext = _host.Services.GetRequiredService<IDbManager<DefaultDbContext>>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_DefaultRepository.Initialize");
        }
        [TestMethod]
        public async Task Query()
        {
            Func<IQueryable<Person>, IQueryable<Person>> where = (query) => query.Where(p => p.Id.StartsWith("A")).Where(p => p.Age > 0);
            Func<IQueryable<Person>, IOrderedQueryable<Person>> order = (query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);
            await _repository.Query(where, order);
        }
        [TestMethod]
        public async Task Create()
        {
            _repository.Create(new Person());
            await _dbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Modify()
        {
            _repository.Modify(new Person());
            await _dbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Remove()
        {
            _repository.Remove(new Person());
            await _dbContext.SaveChangesAsync();
        }
    }
}