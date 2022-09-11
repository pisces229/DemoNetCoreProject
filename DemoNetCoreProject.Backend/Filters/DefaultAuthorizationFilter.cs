using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    public class DefaultAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<DefaultAuthorizationFilter> _logger;
        public DefaultAuthorizationFilter(ILogger<DefaultAuthorizationFilter> logger)
        {
            _logger = logger;
        }
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            _logger.LogInformation("DefaultAuthorizationFilter");
            return Task.CompletedTask;
        }
    }
}
