namespace DemoNetCoreProject.DataLayer.Entities
{
    public partial class Address
    {
        public int Row { get; set; }
        public string Id { get; set; } = null!;
        public string Text { get; set; } = null!;

        public virtual Person Person { get; set; } = null!;
    }
}
