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

        public static string GetLanguageTitle(Language language) => language switch
        {
            Language.CSHARP => "C#",
            Language.PYTHON => "Python",
            Language.JSON => "Json",
            _ => ""
        };

        public static string GetLanguageExtension(Language language) => language switch
        {
            Language.CSHARP => ".cs",
            Language.PYTHON => ".py",
            Language.JSON => ".json",
            _ => ""
        };

        public static Language GetLanguageByExtension(string extension) => extension switch
        {
            ".py" or ".pyd" or ".pyw" => Language.PYTHON,
            ".cs" => Language.CSHARP,
            _ => Language.DEFAULT
        };
    }

    [Flags]
    public enum Language
    {
        CSHARP = 1 << 0,
        PYTHON = 1 << 1,
        JSON = 1 << 3,

        DEFAULT = 0
    }
}
