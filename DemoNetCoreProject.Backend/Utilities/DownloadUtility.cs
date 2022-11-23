using DemoNetCoreProject.Backend.Models.Common;
using System.Text.Json;
using System.Text;

namespace DemoNetCoreProject.Backend.Utilities
{
    public class DownloadUtility
    {
        public const string ContentType = "application/json; charset=utf-8";
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
