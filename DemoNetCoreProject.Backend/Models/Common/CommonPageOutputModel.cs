namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPageOutputModel<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
