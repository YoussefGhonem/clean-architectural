using Elearninig.Shared.Core.Identity.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Elearninig.Shared.Core.Identity.CurrentUser;
public static class CurrentUser
{
    public static IHttpContextAccessor? _httpContextAccessor;
    internal static void Initializer(IHttpContextAccessor? httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public static Guid? Id => string.IsNullOrEmpty(GetClaimValue(ClaimKeys.Id))
        ? null
        : Guid.Parse(GetClaimValue(ClaimKeys.Id)!);
    public static string FirstName => GetClaimValue(ClaimKeys.FirstName);
    public static string LastName => GetClaimValue(ClaimKeys.LastName);
    public static string Email => GetClaimValue(ClaimKeys.Email);
    public static string ImageUrl => GetClaimValue(ClaimKeys.ImageUrl);
    public static List<RolesEnum> Roles => GetRoles();

    #region Helper Methods
    private static List<RolesEnum> GetRoles()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated) return Enumerable.Empty<RolesEnum>().ToList();

        var roles = user.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => Enum.Parse<RolesEnum>(x.Value))
            .ToList();
        return roles;
    }
    private static string GetClaimValue(string key)
    {
        var user = _httpContextAccessor.HttpContext.User;
        var value = user.Claims.FirstOrDefault(c => c.Value == key).Value;
        return value ?? string.Empty;
    }
    #endregion

}
