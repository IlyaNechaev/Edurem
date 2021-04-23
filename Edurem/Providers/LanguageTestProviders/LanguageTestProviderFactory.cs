using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Providers
{
    public class LanguageTestProviderFactory
    {
        private IReadOnlyDictionary<Language, ILanguageTestProvider> LanguageTestProviders { get; init; }

        public LanguageTestProviderFactory()
        {
            var languageTestProviderType = typeof(ILanguageTestProvider);

            LanguageTestProviders = languageTestProviderType.Assembly.ExportedTypes
                .Where(x => languageTestProviderType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x))
                .Cast<ILanguageTestProvider>()
                .ToImmutableDictionary(x => x.Language, x => x);
        }

        public ILanguageTestProvider GetLanguageTestProvider(Language language)
        {
            return LanguageTestProviders[language];
        }
    }
}
