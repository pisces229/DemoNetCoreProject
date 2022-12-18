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
    public class Test_DefaultCustomerDbRepository : Test_Initialize
    {
        private readonly ILogger<Test_DefaultCustomerDbRepository> _logger;
        private readonly IDefaultPersonDbRepository _repository;
        private readonly DefaultDbContext _dbContext;
        public Test_DefaultCustomerDbRepository() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<Test_DefaultCustomerDbRepository>>();
            _repository = _serviceProvider.GetRequiredService<IDefaultPersonDbRepository>();
            _dbContext = _serviceProvider.GetRequiredService<DefaultDbContext>();
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
            await _repository.Create(new Person() 
            { 
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Age = 10,
                Birthday = DateTime.Now,
                Remark = Guid.NewGuid().ToString(),
            });
            await _dbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Modify()
        {
            await _repository.Modify(new Person()
            {
                Row = 1,
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Age = 10,
                Birthday = DateTime.Now,
                Remark = Guid.NewGuid().ToString(),
            });
            await _dbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Remove()
        {
            await _repository.Remove(new Person()
            {
                Row = 1,
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Age = 10,
                Birthday = DateTime.Now,
                Remark = Guid.NewGuid().ToString(),
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}