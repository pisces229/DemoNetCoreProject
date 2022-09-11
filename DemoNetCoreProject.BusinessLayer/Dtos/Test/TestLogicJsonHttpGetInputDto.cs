using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Test
{
    // [BindProperties]: Bind public Property
    // [BindProperty(Name = "Name")]
    // [BindRequired]
    // [BindNever]

    //[BindProperties]
    public class TestLogicJsonHttpGetInputDto
    {
        [BindProperty(Name = "Text")]
        public string? Text { get; set; }
        [BindRequired]
        public int? Value { get; set; }
        [BindProperty]
        public DateTime? Date { get; set; }
    }
}
