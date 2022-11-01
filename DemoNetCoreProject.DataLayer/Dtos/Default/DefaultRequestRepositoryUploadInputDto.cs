using System;

namespace DemoNetCoreProject.DataLayer.Dtos.Default
{
    public class DefaultRequestRepositoryUploadInputDto
    {
        public Stream File { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
