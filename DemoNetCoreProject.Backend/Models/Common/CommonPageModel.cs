using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPageModel
    {
        [JsonPropertyName(name: "PageNo")]
        [Required]
        public int PageNo { get; set; }
        [JsonPropertyName(name: "PageSize")]
        [Required]
        public int PageSize { get; set; }
        [JsonIgnore]
        public int TotalCount { get; set; }
    }
}
