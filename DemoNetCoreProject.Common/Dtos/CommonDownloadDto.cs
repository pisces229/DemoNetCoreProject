using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonDownloadDto
    {
        public string Filename { get; set; } = null!;
        public FileInfo FileInfo { get; set; } = null!;
    }
}
