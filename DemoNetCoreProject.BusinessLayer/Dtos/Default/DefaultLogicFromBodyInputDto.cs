namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultLogicFromBodyInputDto
    {
        public string Value { get; set; } = null!;
        public IEnumerable<string> Values { get; set; } = null!;
    }
}
