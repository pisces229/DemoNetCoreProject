using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Common.Dtos
{
    // [JsonPropertyName(name: "hhh")]
    // [JsonIgnore]
    // [Required]
    public class CommonPagedQueryDto<T> where T : class
    {
        [JsonPropertyName(name: "Data")]
        [Required]
        public T? Data { get; set; }
        [JsonPropertyName(name: "Page")]
        [Required]
        public CommonPageDto? Page { get; set; }
    }
}
