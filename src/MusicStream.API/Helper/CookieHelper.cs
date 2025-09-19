namespace Music.API.Helper;

public static class CookieHelper
{

    public static void SetCookie(HttpContext context, string token, DateTime exipirationTime, LinkGenerator linkGenerator)
    {
        var refreshEndpoint = linkGenerator.GetUriByName(context, "refresh-token-endpoint");
        var uri = new Uri(refreshEndpoint!);

        var option = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = exipirationTime,
            Path = uri.AbsolutePath
        };
        context.Response.Cookies.Append("refresh-token", token, option);
    }

    public static string GetRefreshToken(HttpContext context)
    {
        var token = context.Request.Cookies.First(c => c.Key == "refresh-token");
        return token.Value;
    }

}
