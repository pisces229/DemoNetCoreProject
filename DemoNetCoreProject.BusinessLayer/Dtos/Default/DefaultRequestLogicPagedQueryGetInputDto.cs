using DemoNetCoreProject.Common.Dtos;
using System;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultRequestLogicPagedQueryGetInputDto : CommonPageInputDto
    {
        public string? Text { get; set; }
        public int? Value { get; set; }
        public DateTime? Date { get; set; }
        public IEnumerable<string>? Values { get; set; }
    }
}
