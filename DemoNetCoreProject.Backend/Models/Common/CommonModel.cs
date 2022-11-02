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

        //[JsonPropertyName(name: "Name")]
        //[JsonIgnore]

        //[Required]
        //[ValidateNever]
        //[StringLength(10)]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]

        //[Default("")]
    }
}
