﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Edurem.Services
{
    public class CookieService : ICookieService
    {
        ISecurityService SecurityService { get; init; }
        public CookieService(
            ISecurityService securityService)
        {
            SecurityService = securityService;
        }
        public string GenerateCookie(List<(string Key, string Value)> cookieClaims, string encryptKey = "edurem")
        {
            var cookie = string.Join("&", cookieClaims.Select(claim => $"{claim.Key}={claim.Value}"));

            return SecurityService.Encrypt(cookie, encryptKey);
        }

        public string GetCookie(string key, string cookies, string encryptKey = "edurem")
        {
            var cookie = SecurityService.Decrypt(cookies, encryptKey);

            return cookie.Split("&").FirstOrDefault(claim => claim.Contains($"{key}="))?.Split("=")[1];
        }
    }
}
