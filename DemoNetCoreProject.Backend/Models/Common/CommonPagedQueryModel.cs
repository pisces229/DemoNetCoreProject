using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPagedQueryModel<T> where T : class
    {
        [JsonPropertyName(name: "Data")]
        [Required]
        public T? Data { get; set; }
        [JsonPropertyName(name: "Page")]
        [Required]
        public CommonPageModel? Page { get; set; }
    }
}
