namespace DemoNetCoreProject.DataLayer.Entities
{
    public partial class Person
    {
        public Person()
        {
            Addresses = new HashSet<Address>();
        }

        public int Row { get; set; }
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string? Remark { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
