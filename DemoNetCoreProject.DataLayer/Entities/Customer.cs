using System;
using System.Collections.Generic;

namespace DemoNetCoreProject.DataLayer.Entities
{
    public partial class Customer
    {
        public int Row { get; set; }
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string? Remark { get; set; }
    }
}
