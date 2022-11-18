using DemoNetCoreProject.Backend.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonHttpGetInputModel
    {
        [BindProperty(Name = "text")]
        public string? Text { get; set; }
        [BindProperty(Name = "value")]
        public int? Value { get; set; }
        [BindProperty(Name = "date")]
        public DateTime? Date { get; set; }
    }
}
