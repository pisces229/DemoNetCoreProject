using System;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Backend.Models.Common
{
    public class CommonPageModel
    {
        [JsonPropertyName(name: "PageNo")]
        public int PageNo { get; set; }
        [JsonPropertyName(name: "PageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName(name: "TotalCount")]
        public int TotalCount { get; set; }
    }
}
