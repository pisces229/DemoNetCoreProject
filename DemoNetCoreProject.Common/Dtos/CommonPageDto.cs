using System;
using System.Text.Json.Serialization;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonPageDto
    {
        [JsonPropertyName(name: "PageNo")]
        public int PageNo { get; set; }
        [JsonPropertyName(name: "PageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName(name: "TotalCount")]
        public int TotalCount { get; set; }
    }
}
