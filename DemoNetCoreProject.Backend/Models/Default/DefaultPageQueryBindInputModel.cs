using DemoNetCoreProject.Backend.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultPageQueryBindInputModel : CommonPageBindInputModel, IValidatableObject
    {
        [BindProperty(Name = "value")]
        public string Value { get; set; } = null!;
        [BindProperty(Name = "values[]")]
        public IEnumerable<string>? Values { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success!;
        }
    }
}
