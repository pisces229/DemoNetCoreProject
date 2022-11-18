using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultSignInInputModel
    {
        [JsonPropertyName(name: "account")]
        [Required]
        public string Account { get; set; } = null!;
        [JsonPropertyName(name: "password")]
        [Required]
        public string Password { get; set; } = null!;
    }
}
