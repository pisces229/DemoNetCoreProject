using System;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultRequestLogicJsonHttpPostInputDto
    {
        public string? Text { get; set; }
        public int? Value { get; set; }
        public DateTime? Date { get; set; }
    }
}
