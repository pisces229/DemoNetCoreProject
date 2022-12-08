using DemoNetCoreProject.DataLayer.IServices;

namespace DemoNetCoreProject.IntegrationTest
{
    internal class TestUserService : IUserService
    {
        public string ProgId => "Test";
        public string UserId => "Test";
    }
}
