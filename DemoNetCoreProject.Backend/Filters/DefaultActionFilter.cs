using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    /// <summary>
    /// 用於在控制器方法執行之前和之後執行任意程式碼
    /// </summary>
    public class DefaultActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<DefaultActionFilter> _logger;
        public DefaultActionFilter(ILogger<DefaultActionFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("DefaultActionFilter Before");
            //await context.HttpContext.Request.ReadFormAsync();
            await next();
            //await context.HttpContext.Response.WriteAsync("...");
            _logger.LogInformation("DefaultActionFilter After");
        }
    }
}
