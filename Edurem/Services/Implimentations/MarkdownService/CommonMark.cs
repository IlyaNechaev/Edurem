using CommonMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class CommonMark : IMarkdownService
    {
        public string ToHtml(string MarkdownText)
        {
            return CommonMarkConverter.Convert(MarkdownText);
        }

        public string ToMarkdown(string htmlText)
        {
            throw new NotImplementedException();
        }
    }
}
