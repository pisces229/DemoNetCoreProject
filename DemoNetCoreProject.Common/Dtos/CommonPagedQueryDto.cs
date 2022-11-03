using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedQueryDto<T> where T : class
    {
        public T Data { get; set; } = null!;
        public CommonPageDto Page { get; set; } = null!;
    }
}
