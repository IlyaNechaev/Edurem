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
    }

    public static class AuthenticationExtensions
    {
        public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimKey)
        {
            return claimsPrincipal?.FindFirst(claimKey)?.Value ?? string.Empty;
        }
    }
}
