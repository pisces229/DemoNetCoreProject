using DemoNetCoreProject.Backend.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultPagedQueryGetInputModel : CommonPageBindInputModel
    {
        [BindProperty(Name = "text")]
        public string? Text { get; set; }
        [BindProperty(Name = "value")]
        public int? Value { get; set; }
        [BindProperty(Name = "date")]
        public DateTime? Date { get; set; }
        [BindProperty(Name = "values[]")]
        public IEnumerable<string>? Values { get; set; }
    }
}
