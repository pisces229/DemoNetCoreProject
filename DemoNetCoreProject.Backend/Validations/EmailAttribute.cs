using DemoNetCoreProject.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DemoNetCoreProject.Backend.Validations
{
    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(value?.ToString()) && !Regex.IsMatch(value?.ToString()!, ValidationRegexConstant.Email))
            {
                return new ValidationResult(string.Format("{0}Email", validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
