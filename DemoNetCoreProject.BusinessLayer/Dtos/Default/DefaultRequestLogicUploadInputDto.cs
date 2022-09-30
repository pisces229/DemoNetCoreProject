using Microsoft.AspNetCore.Http;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultRequestLogicUploadInputDto
    {
        public IFormFile File { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
