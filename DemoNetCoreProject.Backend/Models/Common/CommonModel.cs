using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonModel
    {
        //[BindProperties]: Bind public Property
        //[BindProperty(Name = "Name")]
        //[BindRequired]
        //[BindNever]

        //[JsonPropertyName(name: "hhh")]
        //[JsonIgnore]
        //[JsonInclude]

        //[Required]
    }
}
