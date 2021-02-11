using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Html2Markdown;
using StardustDL.RazorComponents.Markdown;

namespace Edurem.Services
{
    public class RazorMarkdown : IMarkdownService
    {
        MarkdownComponentService service { get; set; }
        public RazorMarkdown()
        {
            service = new();
        }
        public string ToHtml(string markdownText)
        {
            return service.RenderHtml(markdownText).Result;
        }

        public string ToMarkdown(string htmlText)
        {
            var converter = new Converter();

            return converter.Convert(htmlText);
        }
    }
}
