using Dapper;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.Enums;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.DataLayer.Utilities;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using System.Text.Json;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    public class DefaultSqlRepository(ILogger<DefaultSqlRepository> _logger,
        IDapperService<DefaultDbContext> _defaultDapperService) 
        : IDefaultSqlRepository
    {
        public async Task RunDapperQuery()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperQuery"));
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("param1", "1");
            dynamicParameters.Add("param2", "2");
            var data = await _defaultDapperService.Query<Person>(
                "SELECT * FROM [Person]",
                dynamicParameters);
            _logger.LogInformation("{v}", JsonSerializer.Serialize(data));
        }
        public async Task RunDapperExecuteScalar()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperExecuteScalar"));
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("param1", "1");
            dynamicParameters.Add("param2", "2");
            var data = await _defaultDapperService.ExecuteScalar<int>(
                "SELECT [Row] FROM [Person]",
                dynamicParameters);
            _logger.LogInformation("{v}", JsonSerializer.Serialize(data));
        }
        public async Task RunDapperQueryMultiple()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperQueryMultiple"));
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("param1", "1");
            dynamicParameters.Add("param2", "2");
            using var gridReader = await _defaultDapperService.QueryMultiple(
                "SELECT top 1 Row FROM [Person] SELECT top 2 Row FROM [Person]",
                dynamicParameters);
            var first = await gridReader.ReadAsync<Person>();
            var second = await gridReader.ReadAsync<Person>();
            _logger.LogInformation("{v}", JsonSerializer.Serialize(first));
            _logger.LogInformation("{v}", JsonSerializer.Serialize(second));
        }
        public async Task RunDapperExecuteReader()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperExecuteReader"));
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("param1", "1");
            dynamicParameters.Add("param2", "2");
            using var dbDataReader = await _defaultDapperService.ExecuteReader(
                "SELECT [Row] FROM [Person]",
                dynamicParameters);
            if (dbDataReader.HasRows)
            {
                while (await dbDataReader.ReadAsync())
                {
                    _logger.LogInformation("{v}", JsonSerializer.Serialize(dbDataReader.GetRowParser<Person>()(dbDataReader)));
                }
            }
        }
        public async Task RunDapperPagedQuery()
        {
            await Task.Run(() => _logger.LogInformation("RunDapperPagedQuery"));
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("param1", "1");
            dynamicParameters.Add("param2", "2");
            var data = await _defaultDapperService.PagedQuery<Person>(
                "SELECT * FROM [Person]", "[Row]",
                dynamicParameters,
                pageSize: 3, pageNo: 3);
            _logger.LogInformation("{v}", JsonSerializer.Serialize(data));
        }
        public async Task RunDapperExcute()
        {
            await _defaultDapperService.Execute(@"
                INSERT INTO [dbo].[person]
                (
                    [id]
                    ,[name]
                    ,[age]
                    ,[birthday]
                    ,[remark]
                )
                VALUES
                ('A','',0,GETDATE(),'')
            ");
        }
        public async Task RunSqlCondition()
        {
            await Task.Run(() => _logger.LogInformation("RunSqlCondition"));
            var sql = new StringBuilder("SELECT M.* FROM [Person] M WHERE 1=1");
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
            _logger.LogInformation("{v}", sql.ToString());
            _logger.LogInformation("{v}", JsonSerializer.Serialize(dynamicParameters));
        }
    }
}
