using System;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Default
{
    public class DefaultJsonInputModel
    {
        [JsonPropertyName(name: "ValueString")]
        //[JsonIgnore]
        public string? ValueString { get; set; }
        [JsonPropertyName(name: "valueDate")]
        //[JsonIgnore]
        public DateTime? ValueDate { get; set; }
    }
}
