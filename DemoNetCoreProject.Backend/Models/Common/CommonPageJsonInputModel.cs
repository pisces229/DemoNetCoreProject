using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPageJsonInputModel
    {
        [JsonPropertyName(name: "pageSize")]
        [Required]
        public int PageSize { get; set; }
        [JsonPropertyName(name: "pageNo")]
        [Required]
        public int PageNo { get; set; }
    }
}
