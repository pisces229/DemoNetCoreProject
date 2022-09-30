using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Default
{
    // [BindProperties]: Bind public Property
    // [BindProperty(Name = "Name")]
    // [BindRequired]
    // [BindNever]

    //[BindProperties]
    public class DefaultRequestLogicJsonHttpGetInputDto
    {
        [BindProperty(Name = "Text")]
        public string? Text { get; set; }
        [BindRequired]
        public int? Value { get; set; }
        [BindProperty]
        public DateTime? Date { get; set; }
    }
}
