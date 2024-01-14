using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    /// <summary>
    /// 用於處理未處理的例外狀況
    /// </summary>
    public class DefaultExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<DefaultExceptionFilter> _logger;
        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(0, context.Exception, "DefaultExceptionFilter");
            // Suggested by dropoutcoder
            //context.HttpContext.Response.Clear();
            //context.HttpContext.Response.WriteAsync("Exception").Wait();
            //context.HttpContext.Response.StatusCode = 400;
            // Suggested by stuartd
            //var result = new CommonResponseDto<string>()
            //{
            //    Success = false,
            //    Message = "Exception",
            //};
            //context.Result = new OkObjectResult(result);
            context.Result = new BadRequestObjectResult("Exception");
            return Task.CompletedTask;
        }
    }
}
