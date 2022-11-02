using System.ComponentModel.DataAnnotations;

namespace DemoNetCoreProject.Backend.Validations
{
    public class DefaultAttribute : ValidationAttribute
    {
        public string Value { get; }
        public DefaultAttribute(string value) => Value = value;
        public string GetErrorMessage() => $"DefaultAttribute.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        { 
            if (value!.ToString() == "Default")
            {
                return new ValidationResult(GetErrorMessage());
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
