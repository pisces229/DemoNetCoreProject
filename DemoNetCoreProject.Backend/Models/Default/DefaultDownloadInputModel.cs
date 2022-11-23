using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultDownloadInputModel
    {
        [JsonPropertyName(name: "filename")]
        [Required]
        public string Filename { get; set; } = null!;
    }
}
