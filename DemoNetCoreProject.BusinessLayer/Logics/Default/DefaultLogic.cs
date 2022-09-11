using Dapper;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Enums;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.BusinessLayer.Logics.Default
{
    internal sealed class DefaultLogic : IDefaultLogic
    {
        private readonly ILogger<DefaultLogic> _logger;
        private readonly DefaultDbContext _defaultDbContext;
        private readonly IDefaultCustomerRepository _defaultCustomerRepository;
        private readonly IDapperService<DefaultDbContext> _defaultDapperService;
        public DefaultLogic(ILogger<DefaultLogic> logger,
            DefaultDbContext defaultDbContext,
            IDefaultCustomerRepository defaultCustomerRepository,
            IDapperService<DefaultDbContext> defaultDapperService) 
        {
            _logger = logger;
            _defaultDbContext = defaultDbContext;
            _defaultCustomerRepository = defaultCustomerRepository;
            _defaultDapperService = defaultDapperService;
        }
        public async Task RunRepository()
        {
            await Task.Run(() => _logger.LogInformation("DefaultLogic.RunRepository"));
            {
                _logger.LogInformation("----------Queryable----------");
                var query = _defaultCustomerRepository.Queryable()
                    .Where(p => p.Id.StartsWith("A"))
                    //.Where(p => p.Name == "A")
                    .Where(p => p.Age > 0)
                    .OrderBy(k => k.Row)
                    .ThenBy(p => p.Id);
                var data = await _defaultCustomerRepository.Query(query);
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
            {
                _logger.LogInformation("----------Create----------");
                _defaultCustomerRepository.Create(new Customer()
                {
                    Id = "RunId",
                    Name = "RunName",
                    Birthday = DateTime.Now,
                    Age = 10,
                    Remark = Guid.NewGuid().ToString(),
                });
                await _defaultDbContext.SaveChangesAsync();
            }
            {
                _logger.LogInformation("----------Modify----------");
                var data = await _defaultCustomerRepository.Queryable()
                    .OrderByDescending(o => o.Row)
                    .FirstAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
                data.Remark = Guid.NewGuid().ToString();
                _defaultDbContext.Update(data);
                await _defaultDbContext.SaveChangesAsync();
            }
            {
                _logger.LogInformation("----------Remove----------");
                var data = await _defaultCustomerRepository.Queryable()
                    .OrderByDescending(o => o.Row)
                    .FirstAsync();
                _logger.LogInformation(JsonSerializer.Serialize(data));
                _defaultDbContext.Remove(data);
                await _defaultDbContext.SaveChangesAsync();
            }
            {
                _logger.LogInformation("----------Queryable----------");
                var query = _defaultCustomerRepository.Queryable()
                    //.Where(p => p.Age > 0)
                    .OrderBy(k => k.Row)
                    .ThenBy(p => p.Id);
                var data = await _defaultCustomerRepository.PagedQuery(query,
                    new CommonPageDto() { PageSize = 3, PageNo = 3 });
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunSqlStatement()
        {
            await Task.Run(() => _logger.LogInformation("DefaultLogic.RunSqlStatement"));

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ValueA", "A");
            dynamicParameters.Add("ValueB", new[] { "1", "2", "3" });
            {
                _logger.LogInformation("----------QueryFirstOrDefault----------");
                var data = await _defaultDapperService.QueryFirstOrDefault<Customer>("SELECT * FROM [Customer]", dynamicParameters);
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
            {
                _logger.LogInformation("----------Query----------");
                var data = await _defaultDapperService.Query<Customer>("SELECT * FROM [Customer]", dynamicParameters);
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
            {
                _logger.LogInformation("----------ExecuteScalar----------");
                var data = await _defaultDapperService.ExecuteScalar<int>("SELECT Row FROM [Customer]", dynamicParameters);
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
            {
                _logger.LogInformation("----------Execute----------");
                await _defaultDapperService.Execute("SELECT Row FROM [Customer]", dynamicParameters);
            }
            {
                _logger.LogInformation("----------QueryMultiple----------");
                using var gridReader = await _defaultDapperService.QueryMultiple("SELECT top 1 Row FROM [Customer] SELECT top 2 Row FROM [Customer]", dynamicParameters);
                var first = await gridReader.ReadAsync<Customer>();
                var second = await gridReader.ReadAsync<Customer>();
                _logger.LogInformation(JsonSerializer.Serialize(first));
                _logger.LogInformation(JsonSerializer.Serialize(second));
            }
            {
                _logger.LogInformation("----------ExecuteReader----------");
                using var dbDataReader = await _defaultDapperService.ExecuteReader("SELECT Row FROM [Customer]", dynamicParameters);
                if (dbDataReader.HasRows)
                {
                    while (await dbDataReader.ReadAsync())
                    {
                        _logger.LogInformation(JsonSerializer.Serialize(dbDataReader.GetRowParser<Customer>()(dbDataReader)));
                    }
                }
            }
            {
                _logger.LogInformation("----------PagedQuery----------");
                var data = await _defaultDapperService.PagedQuery<Customer>("SELECT * FROM [Customer]", "Row",
                    dynamicParameters, new CommonPageDto() { PageSize = 3, PageNo = 3 });
                _logger.LogInformation(JsonSerializer.Serialize(data));
            }
        }
        public async Task RunSqlCondition()
        {
            await Task.Run(() => _logger.LogInformation("DefaultLogic.RunSqlStatementCondition"));

            var sql = new StringBuilder("SELECT M.* FROM [Customer] M WHERE 1=1");
            var dynamicParameters = new DynamicParameters();
            sql.AppendLine();
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.Equal, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.NotEqual, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.GreaterThan, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.GreaterThanOrEqual, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.LessThan, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.LessThanOrEqual, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.LikeStart, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.LikeEnd, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.LikeContain, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.NotLikeStart, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.NotLikeEnd, "0");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.NotLikeContain, "0");
            sql.AppendLine();
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.Equal, "1");
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Birthday", SqlOperatorType.Equal, DateTime.Now.AddHours(1));
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Age", SqlOperatorType.Equal, Convert.ToInt32("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Age", SqlOperatorType.Equal, Convert.ToDouble("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Remark", SqlOperatorType.Equal, '1', DbType.AnsiString);
            sql.AppendLine();
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.NotContain, new List<string>() { "1", "2" });
            sql.AppendLine();
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Name", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Birthday", SqlOperatorType.Contain, new List<DateTime>() { DateTime.Now.AddHours(1), DateTime.Now.AddHours(2) });
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Age", SqlOperatorType.Contain, new List<int>() { Convert.ToInt32("1"), Convert.ToInt32("2") });
            SqlConditionUtility.Add(sql, dynamicParameters, "M.Age", SqlOperatorType.Contain, new List<double>() { Convert.ToDouble("1"), Convert.ToInt64("2") });
            var data = await _defaultDapperService.Query<Customer>(sql.ToString(), dynamicParameters);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
    }
}
