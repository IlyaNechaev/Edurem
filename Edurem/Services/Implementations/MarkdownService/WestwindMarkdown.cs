using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.Markdown;
using Html2Markdown;

namespace Edurem.Services
{
    public class WestwindMarkdown : IMarkdownService
    {
        public string ToHtml(string markdownText)
        {
            return Markdown.Parse(markdownText);
        }

        public string ToMarkdown(string htmlText)
        {
            return new Markdig().ToMarkdown(htmlText);
        }
    }
}
