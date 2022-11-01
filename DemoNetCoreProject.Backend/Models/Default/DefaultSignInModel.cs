using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultSignInModel
    {
        [JsonPropertyName(name: "Account")]
        [Required]
        public string? Account { get; set; }
        [JsonPropertyName(name: "Password")]
        [Required]
        public string? Password { get; set; }
    }
}
