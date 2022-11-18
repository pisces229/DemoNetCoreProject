using DemoNetCoreProject.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultValidatableInputModel : IValidatableObject
    {
        [JsonPropertyName(name: "first")]
        [Display(Name = "[First]")]
        [Required(ErrorMessage = $"First {ValidationErrorMessageConstant.Required}")]
        public string First { get; set; } = null!;
        [JsonPropertyName(name: "second")]
        [Display(Name = "[Second]")]
        [Required(ErrorMessage = $"Second {ValidationErrorMessageConstant.Required}")]
        public string Second { get; set; } = null!;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (First == Second)
            {
                yield return new ValidationResult("First can't equal to Second");
            }
        }
    }
}
