using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultValidatableModel : IValidatableObject
    {
        [JsonPropertyName(name: "First")]
        [Required]
        public string First { get; set; } = null!;
        [JsonPropertyName(name: "Second")]
        [Required]
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
