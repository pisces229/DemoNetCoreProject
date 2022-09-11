using System;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface ISqlBulkCopyService<DB> where DB : IDbContext
    {
        Task Write<T>(List<T> datas) where T : class;
    }
}
