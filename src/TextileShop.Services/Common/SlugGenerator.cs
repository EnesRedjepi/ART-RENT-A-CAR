using System.Text.RegularExpressions;

namespace TextileShop.Services.Common;

public static partial class SlugGenerator
{
    public static string Generate(string value)
    {
        var slug = NonAlphanumeric().Replace(value.Trim().ToLowerInvariant(), "-");
        return slug.Trim('-');
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlphanumeric();
}
