namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultLogicFromQueryInputDto
    {
        public string Value { get; set; } = null!;
        public IEnumerable<string> Values { get; set; } = null!;
    }
}
