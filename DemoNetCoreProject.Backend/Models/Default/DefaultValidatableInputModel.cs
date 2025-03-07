using DemoNetCoreProject.Backend.Validations;
using DemoNetCoreProject.Common.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultValidatableInputModel : IValidatableObject
    {
        [JsonPropertyName(name: "value")]
        [DisplayName("【Value】")]
        //[Required(ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Required}")]
        //[MinLength(length: 2, ErrorMessage = $"【Value】{ValidationErrorMessageConstant.MinLength}2")]
        //[MaxLength(length: 2, ErrorMessage = $"【Value】{ValidationErrorMessageConstant.MinLength}2")]
        //[Range(0, int.MaxValue, ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Number}")]
        //[RegularExpression(ValidationRegexConstant.Ascii, ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Ascii}")]
        //[RegularExpression(ValidationRegexConstant.Email, ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Email}")]
        //[RegularExpression(@"^[0-9]{1}\.[0-9]{1,6}$", ErrorMessage = $"【Value】{ValidationErrorMessageConstant.Format}0.000000")]
        //[Default]
        [Email]
        public string Value { get; set; } = null!;
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
            else
            {
                yield return ValidationResult.Success!;
            }
        }
    }
}
