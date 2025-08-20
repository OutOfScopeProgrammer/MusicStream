using Microsoft.Extensions.Options;
using MusicStream.Infrastructure.Auth;

namespace Music.API.Helper;

public static class CookieHelper
{

    public static void SetCookie(HttpContext context, string token, int expireTimeInMinute)
    {
        var option = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(expireTimeInMinute)
        };
        context.Response.Cookies.Append("access-token", token, option);
    }

}
