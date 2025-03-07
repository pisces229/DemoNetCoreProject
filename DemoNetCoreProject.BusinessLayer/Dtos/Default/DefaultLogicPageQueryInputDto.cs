using DemoNetCoreProject.Common.Dtos;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultLogicPageQueryInputDto : CommonPageInputDto
    {
        public string Value { get; set; } = null!;
        public IEnumerable<string> Values { get; set; } = null!;
    }
}
