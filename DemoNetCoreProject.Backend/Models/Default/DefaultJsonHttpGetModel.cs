using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonHttpGetModel
    {
        [BindProperty(Name = "Text")]
        [BindRequired]
        public string? Text { get; set; }
        [BindRequired]
        public int? Value { get; set; }
        [BindProperty]
        public DateTime? Date { get; set; }
    }
}
