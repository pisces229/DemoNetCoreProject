using System;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonOutputModel
    {
        [JsonPropertyName(name: "String")]
        //[JsonIgnore]
        public string? ValueString { get; set; }
        [JsonPropertyName(name: "Date")]
        //[JsonIgnore]
        public DateTime? ValueDate { get; set; }
    }
}
