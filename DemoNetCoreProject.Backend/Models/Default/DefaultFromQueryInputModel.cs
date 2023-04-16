using DemoNetCoreProject.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultFromQueryInputModel : IValidatableObject
    {
        [BindProperty(Name = "value")]
        [Required(ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Required}")]
        public string Value { get; set; } = null!;
        [BindProperty(Name = "values[]")]
        [Required(ErrorMessage = $"【Values[]】{ValidationErrorMessageConstant.Required}")]
        public IEnumerable<string>? Values { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success!;
        }
    }
}
