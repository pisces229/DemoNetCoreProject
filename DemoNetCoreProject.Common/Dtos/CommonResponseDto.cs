using System;

namespace DemoNetCoreProject.Common.Dtos
{
    public class CommonResponseDto<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
