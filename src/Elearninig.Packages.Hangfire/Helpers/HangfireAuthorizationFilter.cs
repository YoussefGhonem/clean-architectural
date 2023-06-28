using Hangfire.Dashboard;

namespace Elearninig.Packages.Hangfire.Helpers;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string[] _roles;

    public HangfireAuthorizationFilter(params string[] roles)
    {
        _roles = roles;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;

        // TODO: Your authorization logic goes here.

        return true;
    }
}