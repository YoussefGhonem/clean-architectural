using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Shared.Core.Identity.CurrentUser;
public static class ConfigureServices
{
    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        CurrentUser.Initializer(
            app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        return app;
    }
}
