using Microsoft.AspNetCore.Http;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Test
{
    public class TestLogicUploadInputDto
    {
        public IFormFile File { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
