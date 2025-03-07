namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonTokenDto
    {
        public string Account { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
