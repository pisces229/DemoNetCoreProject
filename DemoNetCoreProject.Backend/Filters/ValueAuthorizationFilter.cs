using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoNetCoreProject.Backend.Filters
{
    public class ValueAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _values;
        private readonly ILogger<ValueAuthorizationFilter> _logger;
        public ValueAuthorizationFilter(string[] values,
            ILogger<ValueAuthorizationFilter> logger)
        {
            _values = values;
            _logger = logger;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Run(() => _logger.LogInformation("ValueAuthorizationFilter"));
            _values.ToList().ForEach(f => _logger.LogInformation(f));
        }
    }
}
