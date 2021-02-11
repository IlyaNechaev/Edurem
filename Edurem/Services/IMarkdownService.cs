using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public interface IMarkdownService
    {
        public string ToHtml(string markdownText);

        public string ToMarkdown(string htmlText);
    }
}
