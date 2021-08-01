using Microsoft.AspNetCore.Http;
using System;

namespace Identity.API.Configuration
{
    public static class CookieSettings
    {
        public static readonly CookieOptions PrivateCookie = new()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.MaxValue
        };

        public static readonly CookieOptions PublicCookie = new()
        {
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.MaxValue
        };
    }
}
