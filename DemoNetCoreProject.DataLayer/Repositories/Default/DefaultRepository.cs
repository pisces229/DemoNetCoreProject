using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Dapper;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Enums;
using DemoNetCoreProject.Common.Utilities;
using System.Data;
using System.Text.Json;
using System.Text;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    internal class DefaultRepository : IDefaultRepository
    {
        private readonly ILogger<DefaultRepository> _logger;
        private readonly DefaultDbContext _defaultDbContext;
        private readonly IDapperService<DefaultDbContext> _defaultDapperService;
        public DefaultRepository(ILogger<DefaultRepository> logger, 
            DefaultDbContext defaultDbContext,
            IDapperService<DefaultDbContext> defaultDapperService)
        {
            _logger = logger;
            _defaultDbContext = defaultDbContext;
            _defaultDapperService = defaultDapperService;
        }
        public async Task<int?> MaxRow()
            => await _defaultDbContext.Customers.Select(s => s.Row).MaxAsync();
        public async Task RunDapperQuery()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperQuery"));
            var dynamicParameters = new DynamicParameters();
            var data = await _defaultDapperService.Query<Customer>(
                "SELECT * FROM [Customer]",
                dynamicParameters);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDapperExecuteScalar()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperExecuteScalar"));
            var dynamicParameters = new DynamicParameters();
            var data = await _defaultDapperService.ExecuteScalar<int>(
                "SELECT [Row] FROM [Customer]",
                dynamicParameters);
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunDapperQueryMultiple()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperQueryMultiple"));
            var dynamicParameters = new DynamicParameters();
            using var gridReader = await _defaultDapperService.QueryMultiple(
                "SELECT top 1 Row FROM [Customer] SELECT top 2 Row FROM [Customer]",
                dynamicParameters);
            var first = await gridReader.ReadAsync<Customer>();
            var second = await gridReader.ReadAsync<Customer>();
            _logger.LogInformation(JsonSerializer.Serialize(first));
            _logger.LogInformation(JsonSerializer.Serialize(second));
        }
        public async Task RunDapperExecuteReader()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperExecuteReader"));
            var dynamicParameters = new DynamicParameters();
            using var dbDataReader = await _defaultDapperService.ExecuteReader(
                "SELECT [Row] FROM [Customer]",
                dynamicParameters);
            if (dbDataReader.HasRows)
            {
                while (await dbDataReader.ReadAsync())
                {
                    _logger.LogInformation(JsonSerializer.Serialize(dbDataReader.GetRowParser<Customer>()(dbDataReader)));
                }
            }
        }
        public async Task RunDapperPagedQuery()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperPagedQuery"));
            var dynamicParameters = new DynamicParameters();
            var data = await _defaultDapperService.PagedQuery<Customer>(
                "SELECT * FROM [Customer]", "[Row]",
                dynamicParameters,
                new CommonPageDto() { PageSize = 3, PageNo = 3 });
            _logger.LogInformation(JsonSerializer.Serialize(data));
        }
        public async Task RunSqlCondition()
        {
            await Task.Run(() => _logger.LogInformation("RunSqlCondition"));
            var sql = new StringBuilder("SELECT M.* FROM [Customer] M WHERE 1=1");
            var dynamicParameters = new DynamicParameters();
            //dynamicParameters.Add("ValueA", "A");
            //dynamicParameters.Add("ValueB", new[] { "1", "2", "3" });
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
            _logger.LogInformation(sql.ToString());
            _logger.LogInformation(JsonSerializer.Serialize(dynamicParameters));
        }
    }
}
