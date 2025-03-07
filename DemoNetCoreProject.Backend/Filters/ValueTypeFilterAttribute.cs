using Microsoft.AspNetCore.Mvc;

namespace DemoNetCoreProject.Backend.Filters
{
    public class ValueTypeFilterAttribute : TypeFilterAttribute
    {
        public ValueTypeFilterAttribute(params string[] values)
            : base(typeof(ValueAuthorizationFilter))
        {
            Arguments = new object[] { values };
        }
    }
}
