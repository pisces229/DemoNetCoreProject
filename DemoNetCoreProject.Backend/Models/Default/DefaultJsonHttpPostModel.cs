using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonHttpPostModel
    {
        [JsonPropertyName(name: "Text")]
        public string? Text { get; set; }
        [JsonPropertyName(name: "Value")]
        public int? Value { get; set; }
        [JsonPropertyName(name: "Date")]
        public DateTime? Date { get; set; }
    }
}
