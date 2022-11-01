using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPagedQueryDto<T> where T : class
    {
        public T? Data { get; set; }
        public CommonPageDto? Page { get; set; }
    }
}
