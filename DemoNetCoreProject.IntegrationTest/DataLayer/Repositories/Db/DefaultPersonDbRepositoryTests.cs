using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.DataLayer.Repositories.Db
{
    [TestClass]
    public class DefaultPersonDbRepositoryTests : InitializeTest
    {
        private readonly ILogger<DefaultPersonDbRepositoryTests> _logger;
        private readonly IDefaultPersonDbRepository _repository;
        private readonly DefaultDbContext _dbContext;
        public DefaultPersonDbRepositoryTests() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<DefaultPersonDbRepositoryTests>>();
            _repository = _serviceProvider.GetRequiredService<IDefaultPersonDbRepository>();
            _dbContext = _serviceProvider.GetRequiredService<DefaultDbContext>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("Test_DefaultPersonDbRepository.Initialize");
        }
        [TestMethod]
        public async Task Query()
        {
            //await _repository.Query((IQueryable<Person> query) => query
            //    .Where(p => p.Id.StartsWith("A"))
            //    .Where(p => p.Age > 0)
            //    .OrderBy(o => o.Row)
            //    .ThenBy(o => o.Id));
            Func<IQueryable<Person>, IQueryable<Person>> query = (query) => query
                .Where(p => p.Id.StartsWith("A"))
                .Where(p => p.Age > 0)
                .OrderBy(o => o.Row)
                .ThenBy(o => o.Id);
            await _repository.Query(query);
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
            Func<IQueryable<Person>, IQueryable<Person>> where = (query) => query.Where(p => true);
            var datas = await _repository.Query(where);
            if (datas.Any())
            {
                var data = datas.First();
                data.Remark = Guid.NewGuid().ToString();
                await _repository.Modify(data);
            }
            await _dbContext.SaveChangesAsync();
        }
        [TestMethod]
        public async Task Remove()
        {
            Func<IQueryable<Person>, IQueryable<Person>> where = (query) => query.Where(p => true);
            var datas = await _repository.Query(where);
            if (datas.Any())
            {
                var data = datas.First();
                await _repository.Remove(data);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}