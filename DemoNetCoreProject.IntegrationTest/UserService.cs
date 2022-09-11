using DemoNetCoreProject.DataLayer.IServices;

namespace DemoNetCoreProject.IntegrationTest
{
    internal class UserService : IUserService
    {
        public string ProgId => "Test";
        public string UserId => "Test";
    }
}
