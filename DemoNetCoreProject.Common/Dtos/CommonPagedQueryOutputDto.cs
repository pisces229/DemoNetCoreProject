using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedQueryOutputDto<T> where T : class
    {
        public List<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
