using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedQueryOutputDto<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
