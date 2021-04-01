using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public class CodeLanguage
    {
        public Language Language { get; set; }

        public string LanguageTitle { get; set; }

        public void SetLanguage(Language language) => (Language, LanguageTitle) = (language, GetLanguageTitle(language));

        private string GetLanguageTitle(Language language) => language switch
        {
            Language.CSHARP => "C#",
            Language.PYTHON => "Python",
            Language.JAVA => "Java",
            _ => ""
        };
    }

    [Flags]
    public enum Language
    {
        CSHARP = 1 << 0,
        PYTHON = 1 << 1,
        JAVA = 1 << 2,

        DEFAULT = 0
    }
}
