using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedQueryInputDto<T> where T : class
    {
        public T Data { get; set; } = null!;
        public CommonPageInputDto Page { get; set; } = null!;
    }
}
