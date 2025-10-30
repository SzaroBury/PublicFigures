using System.Security.Claims;

namespace RespectCounter.API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string? TryGetCurrentUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
    }

    public static string GetCurrentUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "??";
    }
}