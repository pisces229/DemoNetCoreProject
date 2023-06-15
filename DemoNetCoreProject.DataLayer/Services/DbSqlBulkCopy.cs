using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class DbSqlBulkCopy<DB> : IDbSqlBulkCopy<DB> where DB : IDbContext
    {
        private readonly ILogger<DbSqlBulkCopy<DB>> _logger;
        private readonly DB _dbContext;
        private readonly IUserService _userService;
        public DbSqlBulkCopy(ILogger<DbSqlBulkCopy<DB>> logger,
            DB dbContext,
            IUserService userService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userService = userService;
        }
        public async Task Write<T>(List<T> datas) where T : class
        {
            if (datas.Any())
            {
                var propertyInfoArray = typeof(T).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var dataTable = new DataTable
                {
                    TableName = typeof(T).Name
                };
                foreach (var propertyInfo in propertyInfoArray)
                {
                    var databaseGenerated = propertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGenerated == null)
                    {
                        dataTable.Columns.Add(propertyInfo.Name);
                    }
                }
                foreach (var data in datas)
                {
                    var dataRow = dataTable.NewRow();
                    foreach (var propertyInfo in propertyInfoArray)
                    {
                        var databaseGeneratedAttribute = propertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
                        if (databaseGeneratedAttribute == null)
                        {
                            var value = propertyInfo.GetValue(data);
                            if (value != null)
                            {
                                if (typeof(DateTime).Equals(propertyInfo.PropertyType)
                                    ||
                                    typeof(DateTime?).Equals(propertyInfo.PropertyType))
                                {
                                    dataRow[propertyInfo.Name] = ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss.fff");
                                }
                                else
                                {
                                    dataRow[propertyInfo.Name] = value;
                                }
                            }
                            else
                            {
                                if ("UPDATE_USER_ID".Equals(propertyInfo.Name))
                                {
                                    dataRow[propertyInfo.Name] = _userService.UserId;
                                }
                                else if ("UPDATE_PROG_CD".Equals(propertyInfo.Name))
                                {
                                    dataRow[propertyInfo.Name] = _userService.ProgId;
                                }
                                else if ("UPDATE_DATE_TIME".Equals(propertyInfo.Name))
                                {
                                    dataRow[propertyInfo.Name] = DateTime.Now;
                                }
                            }
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
                DataTableDetail(dataTable);
                var dbConnection = (SqlConnection)_dbContext.GetDbConnection().Result;
                var dbTransaction = (SqlTransaction)_dbContext.GetDbTransaction();
                //SqlBulkCopyOptions
                //SqlBulkCopyOptions.Default
                //SqlBulkCopyOptions.FireTriggers
                using var sqlBulkCopy = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.FireTriggers, dbTransaction)
                {
                    BatchSize = 5000
                };
                if (_dbContext.GetDatabase().GetCommandTimeout().HasValue)
                {
                    sqlBulkCopy.BulkCopyTimeout = _dbContext.GetDatabase().GetCommandTimeout()!.Value;
                }
                sqlBulkCopy.DestinationTableName = "dbo." + dataTable.TableName;
                foreach (DataColumn dc in dataTable.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                await sqlBulkCopy.WriteToServerAsync(dataTable);
            }
        }
        private void DataTableDetail(DataTable data)
        {
#pragma warning disable CA2254 // 範本應為靜態運算式
            _logger.LogInformation($"Batch Insert Table [{data.TableName}] Count [{data.Rows.Count}]");
#pragma warning restore CA2254 // 範本應為靜態運算式
        }
    }
}
