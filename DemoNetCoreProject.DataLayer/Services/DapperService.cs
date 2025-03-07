using Dapper;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal sealed class DapperService<DB> : IDapperService<DB> where DB : IDbContext
    {
        private const string PAGED_SKIP = "__paged_skip";
        private const string PAGED_TAKE = "__paged_take";
        private readonly ILogger<DapperService<DB>> _logger;
        private readonly DB _dbContext;
        public DapperService(ILogger<DapperService<DB>> logger,
            DB dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<T> QueryFirstOrDefault<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //return await dbConnection.QueryFirstOrDefaultAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            return await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.QueryFirstOrDefaultAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task<IEnumerable<T>> Query<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //return await dbConnection.QueryAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            return await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.QueryAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task<SqlMapper.GridReader> QueryMultiple(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //return await dbConnection.QueryMultipleAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            return await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.QueryMultipleAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task Execute(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //await dbConnection.ExecuteAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.ExecuteAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task<T> ExecuteScalar<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //return await dbConnection.ExecuteScalarAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            return await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.ExecuteScalarAsync<T>(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task<DbDataReader> ExecuteReader(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
        {
            var dbConnection = await _dbContext.GetDbConnection();
            var dbTransaction = _dbContext.GetDbTransaction();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            //return await dbConnection.ExecuteReaderAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            return await Run(async (message) =>
            {
                message.AppendLine(GetSqlString(sql));
                message.AppendLine(GetDynamicParameters(parameters));
                return await dbConnection.ExecuteReaderAsync(sql, parameters, dbTransaction, dbCommandTimeout, commandType);
            });
        }
        public async Task<CommonPageOutputDto<T>> PagedQuery<T>(string sql, string order, DynamicParameters parameters, int pageSize, int pageNo,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class
        {
            var result = new CommonPageOutputDto<T>();
            var dbCommandTimeout = _dbContext.GetDatabase().GetCommandTimeout();
            dbCommandTimeout ??= commandTimeout;
            result.TotalCount = await ExecuteScalar<int>($"SELECT COUNT(1) FROM ({sql}) M",
                parameters, dbCommandTimeout, commandType);
            parameters.Add(PAGED_SKIP, pageSize * (pageNo - 1));
            parameters.Add(PAGED_TAKE, pageSize);
            result.Data = await Query<T>($"{sql} ORDER BY {order} OFFSET @{PAGED_SKIP} ROWS FETCH NEXT @{PAGED_TAKE} ROWS ONLY",
                parameters, dbCommandTimeout, commandType);
            return result;
        }
        private static string GetSqlString(string parameter)
        {
            return new Regex("[ ]{2,}", RegexOptions.None).Replace(parameter.Replace(Environment.NewLine, " "), " ");
        }
        private static string GetDynamicParameters(DynamicParameters? dynamicParameters)
        {
            var result = new StringBuilder();
            if (dynamicParameters != null)
            {
                var parameterNames = dynamicParameters.ParameterNames.ToList();
                if (parameterNames.Any())
                {
                    result.Append("-- parameters:");
                    parameterNames.ForEach(name =>
                    {
                        var dynamicParameter = dynamicParameters.Get<object>(name);
                        if (dynamicParameter != null)
                        {
                            var dynamicParameterType = dynamicParameter.GetType();
                            if (dynamicParameter is IList list)
                            {
                                var stringBuilder = new StringBuilder();
                                foreach (var value in list)
                                {
                                    if (stringBuilder.Length > 0)
                                    {
                                        stringBuilder.Append(',');
                                    }
                                    stringBuilder.Append(GetParameterValue(value));
                                }
                                result.Append($"[{name}]:[{stringBuilder.ToString()}],");
                            }
                            else
                            {
                                result.Append($"[{name}]:[{GetParameterValue(dynamicParameter)}],");
                            }
                        }
                        else
                        {
                            result.Append($"[{name}]:[NULL],");
                        }
                    });
                }
            }
            return result.ToString();
        }
        private static string GetParameterValue(object value)
        {
            if (value == null)
                return "NULL";
            else if (value is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            else
                return value.ToString()!;

        }
        private async Task<T> Run<T>(Func<StringBuilder, Task<T>> next)
        {
            var success = true;
            var message = new StringBuilder();
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                return await next(message);
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                watch.Stop();
                message.Append($"-- Run Time:[{watch.ElapsedMilliseconds}]ms");
                if (success)
                {
                    _logger.LogInformation(message.ToString());
                }
                else
                {
                    _logger.LogError(message.ToString());
                }
            }
        }
    }
}
