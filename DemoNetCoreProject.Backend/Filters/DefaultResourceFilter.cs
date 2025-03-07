using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    /// <summary>
    /// 用於在模型綁定之前和之後執行任意程式碼
    /// </summary>
    public class DefaultResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<DefaultResourceFilter> _logger;
        public DefaultResourceFilter(ILogger<DefaultResourceFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("DefaultResourceFilter Before");
            //await context.HttpContext.Request.ReadFormAsync();
            await next();
            //await context.HttpContext.Response.WriteAsync("...");
            _logger.LogInformation("DefaultResourceFilter After");
        }
    }
}
