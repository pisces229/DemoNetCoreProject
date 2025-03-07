namespace DemoNetCoreProject.DataLayer.Dtos.Default
{
    public class DefaultRepositoryUploadInputDto
    {
        public Stream File { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
