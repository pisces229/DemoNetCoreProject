using System;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultRequestLogicSignInInputDto
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
