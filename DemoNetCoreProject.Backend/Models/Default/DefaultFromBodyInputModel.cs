using DemoNetCoreProject.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultFromBodyInputModel : IValidatableObject
    {
        [JsonPropertyName(name: "value")]
        [Required(ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Required}")]
        public string Value { get; set; } = null!;
        [JsonPropertyName(name: "values")]
        [Required(ErrorMessage = $"【Values】{ValidationErrorMessageConstant.Required}")]
        public IEnumerable<string>? Values { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success!;
        }
    }
}
