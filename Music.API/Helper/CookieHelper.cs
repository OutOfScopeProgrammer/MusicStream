namespace Music.API.Helper;

public static class CookieHelper
{

    public static void SetCookie(HttpContext context, string token, int expireTimeInMinute, LinkGenerator linkGenerator)
    {
        var refreshEndpoint = linkGenerator.GetUriByName(context, "refresh-token-endpoint");

        var option = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(expireTimeInMinute),
            Path = refreshEndpoint
        };
        context.Response.Cookies.Append("refresh-token", token, option);
    }

}
