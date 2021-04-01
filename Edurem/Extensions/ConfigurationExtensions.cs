using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetFilePath(this IConfiguration confguration, string fileName)
        {
            return confguration.GetValue<string>($"Paths:{fileName}");
        }
    }
}
