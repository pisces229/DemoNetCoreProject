using DemoNetCoreProject.DataLayer.IServices;

namespace DemoNetCoreProject.Batch.Services
{
    internal class UserBachService : IUserService
    {
        public string ProgId => Environment.GetEnvironmentVariable("PROG_ID")!;
        public string UserId => Environment.GetEnvironmentVariable("PROG_ID")!;
    }
}
