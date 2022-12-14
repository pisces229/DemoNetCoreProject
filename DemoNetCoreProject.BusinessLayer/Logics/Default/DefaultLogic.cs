using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultLogic : IDefaultLogic
    {
        private readonly ILogger<DefaultLogic> _logger;
        private readonly IDbManager<DefaultDbContext> _defaultDbManager;
        private readonly IDefaultPersonDbRepository _defaultPersonDbRepository;
        private readonly IDefaultRepository _defaultRepository;
        public DefaultLogic(ILogger<DefaultLogic> logger,
            IDbManager<DefaultDbContext> defaultDbManager,
            IDefaultPersonDbRepository defaultPersonDbRepository,
            IDefaultRepository defaultRepository) 
        {
            _logger = logger;
            _defaultDbManager = defaultDbManager;
            _defaultPersonDbRepository = defaultPersonDbRepository;
            _defaultRepository = defaultRepository;
        }
        public async Task RunDbRepositoryQuery()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryQuery----------"));
            IQueryable<Person> where(IQueryable<Person> query) => query.Where(p => p.Id.StartsWith("A")).Where(p => p.Age > 0);
            IOrderedQueryable<Person> order(IQueryable<Person> query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);
            var data = await _defaultPersonDbRepository.Query(where, order);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDbRepositoryCreate()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryCreate----------"));
            var data = new Person()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Birthday = DateTime.Now,
                Age = 10,
                Remark = Guid.NewGuid().ToString(),
            };
            await _defaultDbManager.BeginTransactionAsync();
            await _defaultPersonDbRepository.Create(data);
            await _defaultDbManager.CommitAsync();
            //await _defaultDbManager.RollbackAsync();
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDbRepositoryModify()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryModify----------"));
            IOrderedQueryable<Person> order(IQueryable<Person> query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultPersonDbRepository.Query((Func<IQueryable<Person>, IOrderedQueryable<Person>>)order)).FirstOrDefault();
            if (data != null)
            {
                data.Remark = Guid.NewGuid().ToString();
                await _defaultDbManager.BeginTransactionAsync();
                await _defaultPersonDbRepository.Modify(data);
                await _defaultDbManager.CommitAsync();
                //await _defaultDbManager.RollbackAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunDbRepositoryRemove()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryRemove----------"));
            IOrderedQueryable<Person> order(IQueryable<Person> query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultPersonDbRepository.Query((Func<IQueryable<Person>, IOrderedQueryable<Person>>)order)).FirstOrDefault();
            if (data != null)
            {
                await _defaultDbManager.BeginTransactionAsync();
                await _defaultPersonDbRepository.Remove(data);
                await _defaultDbManager.CommitAsync();
                //await _defaultDbManager.RollbackAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunDbRepositoryPagedQuery()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryPagedQuery----------"));
            var commonPage = new CommonPageInputDto() { PageSize = 3, PageNo = 3 };
            IQueryable<Person> where(IQueryable<Person> query) => query.Where(p => p.Age > 0);
            IOrderedQueryable<Person> order(IQueryable<Person> query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);
            var data = await _defaultPersonDbRepository
                .PagedQuery(commonPage, where, order);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDapperQuery() => await _defaultRepository.RunDapperQuery();
        public async Task RunDapperExecuteScalar() => await _defaultRepository.RunDapperExecuteScalar();
        public async Task RunDapperQueryMultiple() => await _defaultRepository.RunDapperQueryMultiple();
        public async Task RunDapperExecuteReader() => await _defaultRepository.RunDapperExecuteReader();
        public async Task RunDapperPagedQuery() => await _defaultRepository.RunDapperPagedQuery();
        public async Task RunSqlCondition() => await _defaultRepository.RunSqlCondition();
    }
}
