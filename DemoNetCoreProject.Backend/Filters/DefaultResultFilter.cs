using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    /// <summary>
    /// 用於在控制器方法傳回結果之前和之後執行任意程式碼
    /// </summary>
    public class DefaultResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<DefaultResultFilter> _logger;
        public DefaultResultFilter(ILogger<DefaultResultFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("DefaultResultFilter Before");
            //await context.HttpContext.Request.ReadFormAsync();
            await next();
            if (!context.ModelState.IsValid)
            {
                _logger.LogError("model validation errors occurred.");
            }
            //await context.HttpContext.Response.WriteAsync("...");
            _logger.LogInformation("DefaultResultFilter After");
        }
    }
}
