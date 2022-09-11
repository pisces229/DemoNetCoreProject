using System.Text.Json.Serialization;

namespace DemoNetCoreProject.BusinessLayer.Dtos.Test
{
    // [JsonPropertyName(name: "hhh")]
    // [JsonIgnore]
    // [Required]
    public class TestLogicJsonHttpPostInputDto
    {
        [JsonPropertyName(name: "Text")]
        public string? Text { get; set; }
        [JsonPropertyName(name: "Value")]
        public int? Value { get; set; }
        [JsonPropertyName(name: "Date")]
        public DateTime? Date { get; set; }
    }
}
