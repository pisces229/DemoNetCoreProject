using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IRepositories.Default;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultLogic : IDefaultLogic
    {
        private readonly ILogger<DefaultLogic> _logger;
        private readonly IDbManager<DefaultDbContext> _defaultDbManager;
        private readonly IDefaultCustomerDbRepository _defaultCustomerDbRepository;
        private readonly IDefaultRepository _defaultRepository;
        public DefaultLogic(ILogger<DefaultLogic> logger,
            IDbManager<DefaultDbContext> defaultDbManager,
            IDefaultCustomerDbRepository defaultCustomerDbRepository,
            IDefaultRepository defaultRepository) 
        {
            _logger = logger;
            _defaultDbManager = defaultDbManager;
            _defaultCustomerDbRepository = defaultCustomerDbRepository;
            _defaultRepository = defaultRepository;
        }
        public async Task RunDbRepositoryQuery()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryQuery----------"));
            Func<IQueryable<Customer>, IQueryable<Customer>> where = (query) => query.Where(p => p.Id.StartsWith("A")).Where(p => p.Age > 0);
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> order = (query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);  
            var data = await _defaultCustomerDbRepository.Query(where: where, order: order);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDbRepositoryCreate()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryCreate----------"));
            var data = new Customer()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Birthday = DateTime.Now,
                Age = 10,
                Remark = Guid.NewGuid().ToString(),
            };
            _defaultCustomerDbRepository.Create(data);
            await _defaultDbManager.SaveChangesAsync();
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDbRepositoryModify()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryModify----------"));
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> order = (query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultCustomerDbRepository.Query(order: order)).FirstOrDefault();
            if (data != null)
            {
                data.Remark = Guid.NewGuid().ToString();
                _defaultCustomerDbRepository.Modify(data);
                await _defaultDbManager.SaveChangesAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunDbRepositoryRemove()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryRemove----------"));
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> order = (query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultCustomerDbRepository.Query(order: order)).FirstOrDefault();
            if (data != null)
            {
                _defaultCustomerDbRepository.Remove(data);
                await _defaultDbManager.SaveChangesAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunDbRepositoryPagedQuery()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryPagedQuery----------"));
            var commonPage = new CommonPageDto() { PageSize = 3, PageNo = 3 };
            Func<IQueryable<Customer>, IQueryable<Customer>> where = (query) => query.Where(p => p.Age > 0);
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> order = (query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);
            var data = await _defaultCustomerDbRepository
                .PagedQuery(commonPage, where: where, order: order);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDapperQuery() => await _defaultRepository.RunDapperQuery();
        public async Task RunDapperExecuteScalar() => await _defaultRepository.RunDapperQuery();
        public async Task RunDapperQueryMultiple() => await _defaultRepository.RunDapperQuery();
        public async Task RunDapperExecuteReader() => await _defaultRepository.RunDapperQuery();
        public async Task RunDapperPagedQuery() => await _defaultRepository.RunDapperQuery();
        public async Task RunSqlCondition() => await _defaultRepository.RunDapperQuery();
    }
}
