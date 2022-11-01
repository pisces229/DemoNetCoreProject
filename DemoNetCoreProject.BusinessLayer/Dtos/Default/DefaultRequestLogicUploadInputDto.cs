using Microsoft.AspNetCore.Http;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultRequestLogicUploadInputDto
    {
        public Stream File { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
