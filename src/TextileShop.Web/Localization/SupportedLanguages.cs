using System.Globalization;

namespace TextileShop.Web.Localization;

public sealed record Language(string Code, string NativeName);

public static class SupportedLanguages
{
    /// <summary>Numbers, currency and sorting stay on this culture; only the UI language changes.</summary>
    public const string FormattingCulture = "en-IE";

    public const string DefaultUiCulture = "en";

    public static readonly IReadOnlyList<Language> All = new[]
    {
        new Language("en", "English"),
        new Language("sq", "Shqip"),
        new Language("mk", "Македонски"),
        new Language("tr", "Türkçe")
    };

    public static bool IsSupported(string? code) =>
        code is not null && All.Any(language => language.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

    public static Language Current =>
        All.FirstOrDefault(language => language.Code == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) ?? All[0];
}
