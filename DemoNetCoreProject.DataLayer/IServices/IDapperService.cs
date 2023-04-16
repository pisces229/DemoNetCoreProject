using Dapper;
using DemoNetCoreProject.Common.Dtos;
using System.Data;
using System.Data.Common;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IDapperService<DB> where DB : IDbContext
    {
        Task<T> QueryFirstOrDefault<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class;
        Task<IEnumerable<T>> Query<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class;
        Task<SqlMapper.GridReader> QueryMultiple(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task Execute(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<T> ExecuteScalar<T>(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<DbDataReader> ExecuteReader(string sql, DynamicParameters? parameters = null,
            int? commandTimeout = null, CommandType commandType = CommandType.Text);
        Task<CommonPageOutputDto<T>> PagedQuery<T>(string countSql, string querySql, DynamicParameters parameters, int pageSize, int pageNo,
            int? commandTimeout = null, CommandType commandType = CommandType.Text)
            where T : class;
    }
}
