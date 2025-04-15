using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    public class DefaultSqlLogic : IDefaultSqlLogic
    {
        private readonly ILogger<DefaultSqlLogic> _logger;
        private readonly IDbManager<DefaultDbContext> _defaultDbManager;
        private readonly IDefaultPersonDbRepository _defaultPersonDbRepository;
        private readonly IDefaultSqlRepository _defaultRepository;
        public DefaultSqlLogic(ILogger<DefaultSqlLogic> logger,
            IDbManager<DefaultDbContext> defaultDbManager,
            IDefaultPersonDbRepository defaultPersonDbRepository,
            IDefaultSqlRepository defaultRepository)
        {
            _logger = logger;
            _defaultDbManager = defaultDbManager;
            _defaultPersonDbRepository = defaultPersonDbRepository;
            _defaultRepository = defaultRepository;
        }
        public async Task RunDbRepositoryQuery()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryQuery----------"));
            IQueryable<Person> where(IQueryable<Person> query) => query
                .Where(p => p.Id.StartsWith("A"))
                .Where(p => p.Age > 0)
                .OrderBy(o => o.Row)
                .ThenBy(o => o.Id);
            var data = await _defaultPersonDbRepository.Query(where);
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
            // test " Cannot insert duplicate key row in object 'dbo.person' with unique index 'uni__person__id'. The duplicate key value is (A                                   )."
            //try
            //{
            //    await _defaultPersonDbRepository.Create(new Person()
            //    {
            //        Id = "A",
            //        Name = Guid.NewGuid().ToString(),
            //        Birthday = DateTime.Now,
            //        Age = 10,
            //        Remark = Guid.NewGuid().ToString(),
            //    });
            //    //await _defaultRepository.RunDapperExcute();
            //}
            //catch (DbUpdateException e)
            //{
            //    Console.WriteLine("DbUpdateException");
            //    Console.WriteLine(e.ToString());
            //}
            //catch (SqlException e)
            //{
            //    Console.WriteLine("SqlException");
            //    Console.WriteLine(e.ToString());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception");
            //    Console.WriteLine(e.ToString());
            //}
        }
        public async Task RunDbRepositoryModify()
        {
            await Task.Run(() => _logger.LogInformation("----------RunDbRepositoryModify----------"));
            IQueryable<Person> query(IQueryable<Person> query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultPersonDbRepository.Query(query)).FirstOrDefault();
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
            IQueryable<Person> query(IQueryable<Person> query) => query.OrderByDescending(o => o.Row);
            var data = (await _defaultPersonDbRepository.Query(query)).FirstOrDefault();
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
            IQueryable<Person> where(IQueryable<Person> query) => query.Where(p => p.Age > 0);
            IOrderedQueryable<Person> order(IQueryable<Person> query) => query.OrderBy(o => o.Row).ThenBy(o => o.Id);
            var data = await _defaultPersonDbRepository
                .PagedQuery(pageSize: 3, pageNo: 3, where, order);
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
