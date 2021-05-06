using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Extensions
{
    public static class PostEditingExtensions
    {
        public class DirectoryPath
        {
            public IConfiguration Configuration { get; init; }
            public string ForPostFiles(string postId) => Configuration.GetValue<string>($"Directories:PostFiles").Replace("{postId}", postId);
            public string ForPostCommonTests(string postId) => Configuration.GetValue<string>($"Directories:PostCommonTests").Replace("{postId}", postId);
            public string ForPostOptionTests(string postId, string userId) => Configuration.GetValue<string>($"Directories:PostOptionTests").Replace("{postId}", postId).Replace("{userId}", userId);
            public string ForPostGeneratedOptionTests(string postId, string userId) => Configuration.GetValue<string>($"Directories:PostGeneratedOptionTests").Replace("{postId}", postId).Replace("{userId}", userId);
            public string ForPostGeneratedCommonTests(string postId) => Configuration.GetValue<string>($"Directories:PostGeneratedCommonTests").Replace("{postId}", postId);
            public string ForUserSolution(string postId, string userId) => Configuration.GetValue<string>($"Directories:UserSolution").Replace("{postId}", postId).Replace("{userId}", userId);
            public string ForDockerfile(string postId, string userId) => Configuration.GetValue<string>($"Directories:Dockerfile").Replace("{postId}", postId).Replace("{userId}", userId);
            public string ForContext(string postId) => Configuration.GetValue<string>($"Directories:Context").Replace("{postId}", postId);
        }

        public static DirectoryPath GetDirectoryPath(this IConfiguration confguration)
        {
            return new DirectoryPath { Configuration = confguration };
        }
    }
}
