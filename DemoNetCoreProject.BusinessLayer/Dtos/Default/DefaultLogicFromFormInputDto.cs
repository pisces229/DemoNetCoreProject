namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    public class DefaultLogicFromFormInputDto
    {
        public string Value { get; set; } = null!;
        public IEnumerable<string> Values { get; set; } = null!;
        public Stream File { get; set; } = null!;
    }
}
