using DemoNetCoreProject.Backend.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonHttpGetModel
    {
        [BindProperty(Name = "Text")]
        public string? Text { get; set; }
        [BindProperty(Name = "Value")]
        public int? Value { get; set; }
        [BindProperty]
        public DateTime? Date { get; set; }
    }
}
