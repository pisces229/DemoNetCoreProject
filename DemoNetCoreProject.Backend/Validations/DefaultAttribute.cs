using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Validations
{
    public class DefaultAttribute : ValidationAttribute
    {
        public DefaultAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value?.ToString() == "")
            {
                return new ValidationResult(string.Format("{0}Default", validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
