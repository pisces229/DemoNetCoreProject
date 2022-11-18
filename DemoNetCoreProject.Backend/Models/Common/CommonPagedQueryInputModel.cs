using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPagedQueryInputModel<T> where T : class
    {
        [JsonPropertyName(name: "data")]
        [Required]
        public T Data { get; set; } = null!;
        [JsonPropertyName(name: "page")]
        [Required]
        public CommonPageInputModel Page { get; set; } = null!;
    }
}
