using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedResultDto<T> where T : class
    {
        public CommonPageDto Page { get; set; } = null!;
        public List<T> Data { get; set; } = null!;
    }
}
