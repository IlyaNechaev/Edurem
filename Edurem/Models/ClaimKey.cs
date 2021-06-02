using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public static class ClaimKey
    {
        public static string Login { get; private set; } = "Claim.Key.Login";
        public static string Name { get; private set; } = "Claim.Key.Name";
        public static string Surname { get; private set; } = "Claim.Key.Surname";
        public static string Id { get; private set; } = "Claim.Key.Id";
        public static string Status { get; private set; } = "Claim.Key.Status";
        public static string CookiesId { get; private set; } = "Edurem.Cookies.Id";
    }

    public static class AuthenticationExtensions
    {
        public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimKey)
        {
            var cl = claimsPrincipal?.FindFirst(claimKey)?.Value ?? string.Empty;

            return cl;
        }

        public static string GetCookieValue(this HttpRequest request, string cookieKey)
        {
            if (request.Cookies.TryGetValue(cookieKey, out var cookieValue))
            {
                return cookieValue;
            }
            return string.Empty;
        }
    }
}
