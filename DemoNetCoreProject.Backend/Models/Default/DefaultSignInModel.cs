using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultSignInModel
    {
        [JsonPropertyName(name: "Account")]
        [Required(ErrorMessage = "[Account] can't be null.")]
        public string? Account { get; set; }
        [JsonPropertyName(name: "Password")]
        [Required(ErrorMessage = "[Password] can't be null.")]
        public string? Password { get; set; }
    }
}
