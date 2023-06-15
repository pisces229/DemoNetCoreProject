using DemoNetCoreProject.Backend.Models.Common;
using System.Text;
using System.Text.Json;

namespace DemoNetCoreProject.Backend.Utilities
{
    public class DownloadUtility
    {
        public const string ContentTypeJson = "application/json; charset=utf-8";
        public const string ContentTypeOctetStream = "application/octet-stream";
        public const string ContentTypePdf = "application/pdf";
        public static byte[] ToBytes(string message)
        {
            var json = JsonSerializer.Serialize(new CommonOutputModel<string>
            {
                Success = false,
                Message = message
            }, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
