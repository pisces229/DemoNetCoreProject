using DemoNetCoreProject.Backend.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultPageQueryJsonInputModel : CommonPageJsonInputModel, IValidatableObject
    {
        [JsonPropertyName(name: "value")]
        public string Value { get; set; } = null!;
        [JsonPropertyName(name: "values")]
        public IEnumerable<string>? Values { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success!;
        }
    }
}
