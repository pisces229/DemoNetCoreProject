namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPageOutputDto<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
