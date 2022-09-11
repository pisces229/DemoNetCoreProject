using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    public class DefaultExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<DefaultExceptionFilter> _logger;
        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
            // Suggested by dropoutcoder
            //context.HttpContext.Response.Clear();
            //context.HttpContext.Response.WriteAsync("Exception").Wait();
            //context.HttpContext.Response.StatusCode = 400;
            // Suggested by stuartd
            context.Result = new BadRequestObjectResult("Exception");
            return Task.CompletedTask;
        }
    }
}
