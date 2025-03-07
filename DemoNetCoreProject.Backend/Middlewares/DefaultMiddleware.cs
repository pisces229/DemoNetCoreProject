namespace DemoNetCoreProject.Backend.Middlewares
{
    public class DefaultMiddleware
    {
        private readonly RequestDelegate _dequestDelegate;
        public DefaultMiddleware(RequestDelegate requestDelegate)
        {
            _dequestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            //var correlationGuid = Guid.NewGuid().ToString();
            //context.Request.Headers.Add("CorrelationGuid", correlationGuid);
            await _dequestDelegate(context);
            //context.Response.Headers.Add("CorrelationGuid", correlationGuid);
        }
    }
}
