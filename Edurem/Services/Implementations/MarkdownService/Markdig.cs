using Markdig;
using ColorCode;
using Html2Markdown;
using Markdig.Prism;
using Jering.Web.SyntaxHighlighters.Prism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class Markdig : IMarkdownService
    {
        public string ToHtml(string markdownText)
        {
            var codeFragments = GetCodeFragments(markdownText);
            var html = Markdown.ToHtml(markdownText);

            if (codeFragments.Count > 0)
                html = InsertCodeFragments(html, codeFragments.Select(cf => ColorCode(cf.Code, cf.Language)).ToList());

            return html;
        }

        public string ToMarkdown(string htmlText)
        {
            var converter = new Converter();

            htmlText = SimplifyCodeFragments(htmlText);

            return converter.Convert(htmlText).Replace("\r\n\r\n", "\r\n"); 
        }

        private List<(string Code, string Language)> GetCodeFragments(string markdownText)
        {
            var fragments = markdownText.Split("\n");
            var codeFragments = new List<(string, string)>();

            var language = string.Empty;
            var codeFragment = string.Empty;

            bool IsCodeFragment = false;

            foreach (var @string in fragments)
            {
                if (@string.Contains("```"))
                {
                    IsCodeFragment = !IsCodeFragment;

                    // Если блок кода закончился
                    if (!IsCodeFragment)
                    {
                        codeFragments.Add((codeFragment, language));
                    }
                    else
                    {
                        codeFragment = string.Empty;
                        language = @string.Remove(0, 3);
                    }
                    continue;
                }

                if (IsCodeFragment)
                {
                    codeFragment += @string + "\n";
                }
            }

            return codeFragments;
        }
        private string InsertCodeFragments(string html, List<string> htmlCodes)
        {
            var fragments = html.Split("<code");

            for (int i = 1; i < fragments.Length; i++)
            {
                var fragmentAfterCode = (fragments[i].Split("</code>").Length == 2 ? fragments[i].Split("</code>")[1] : "");
                var language = fragments[i].Split(">")[0];
                fragments[i] = "<code " + language.Trim() + ">" + (htmlCodes.Count >= i ? htmlCodes[i - 1] : "") + "</code>" + fragmentAfterCode;
            }

            return string.Join(string.Empty, fragments);
        }

        private string SimplifyCodeFragments(string html)
        {            
            var fragments = html.Split("<code");
            List<(string Code, string Language)> codeFragments = new();
            List<string> tempFragments = new();

            for (int i = 1; i < fragments.Length; i+=2)
            {
                fragments[i] = fragments[i].Split("</code>")[0];
            }

            for (int i = 1; i < fragments.Length; i++)
            {
                tempFragments = fragments[i].Split(">").ToList();
                for (int y = 1; y < tempFragments.Count; y++)
                {
                    tempFragments[y] = tempFragments[y].Split("<")[0];
                }
                codeFragments.Add((string.Join("", tempFragments.Skip(1)), tempFragments[0].Replace("class=\"language-", "").Replace("\"", "").Trim()));
            }

            html = InsertCodeFragments(html, codeFragments.Select(fragment => fragment.Code).ToList());

            foreach (var codeFragment in codeFragments)
            {
                var current = $"<code class=\"language-{codeFragment.Language}\">";
                var replaceTo = $"```{codeFragment.Language}\n";
                html = html.Replace(current, replaceTo);

                current = "</code>";
                replaceTo = "```";
                html = html.Replace(current, replaceTo);
            }

            return html;
        }

        private string ColorCode(string code, string language)
        {
            //var formatter = new HtmlFormatter();
            //return formatter.GetHtmlString(code, Languages.CSharp);

            return StaticPrismService.IsValidLanguageAliasAsync(language).Result ? StaticPrismService.HighlightAsync(code, language).Result : code;
        }
    }
}
