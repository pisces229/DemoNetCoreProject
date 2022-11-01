using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPageDto
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
