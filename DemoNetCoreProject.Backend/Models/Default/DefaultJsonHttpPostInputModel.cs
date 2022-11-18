using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonHttpPostInputModel
    {
        [JsonPropertyName(name: "text")]
        public string? Text { get; set; }
        [JsonPropertyName(name: "value")]
        public int? Value { get; set; }
        [JsonPropertyName(name: "date")]
        public DateTime? Date { get; set; }
    }
}
