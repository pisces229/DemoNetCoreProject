using DemoNetCoreProject.DataLayer.IServices;

namespace DemoNetCoreProject.IntegrationTest
{
    internal class UserServiceTest : IUserService
    {
        public string ProgId => "Test";
        public string UserId => "Test";
    }
}
