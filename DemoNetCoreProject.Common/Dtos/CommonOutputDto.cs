namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonOutputDto<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
