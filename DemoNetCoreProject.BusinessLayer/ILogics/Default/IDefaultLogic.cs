using System;

namespace DemoNetCoreProject.BusinessLayer.ILogics.Default
{
    public interface IDefaultLogic
    {
        Task RunRepository();
        Task RunSqlStatement();
        Task RunSqlCondition();
    }
}
