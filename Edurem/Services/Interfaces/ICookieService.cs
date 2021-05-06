using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Services
{
    public interface ICookieService
    {
        public string GenerateCookie(List<(string Key, string Value)> cookieClaims, string encryptKey = "edurem_ecnrypt_key");
        public string GetCookie(string key, string cookies, string encryptKey = "edurem_ecnrypt_key");
    }
}
