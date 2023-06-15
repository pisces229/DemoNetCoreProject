namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonOutputModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
