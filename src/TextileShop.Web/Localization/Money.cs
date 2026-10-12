using System.Globalization;

namespace TextileShop.Web.Localization;

/// <summary>
/// Display currency for the storefront. Prices are stored and charged in euros; Macedonian denars are a
/// display conversion at a configured rate. The current currency is set per request by the currency middleware.
/// </summary>
public static class Money
{
    public const string Euro = "EUR";
    public const string Denar = "MKD";
    public const string CookieName = "currency";

    private static readonly AsyncLocal<string?> current = new();

    public static readonly IReadOnlyList<string> Currencies = new[] { Denar, Euro };

    /// <summary>Currency shown to visitors who have not chosen one. Overridden by the Currency:Default setting.</summary>
    public static string Default { get; set; } = Denar;

    /// <summary>Denars per euro. Overridden by the Currency:MkdPerEur setting.</summary>
    public static decimal MkdPerEur { get; set; } = 61.5m;

    public static string Current
    {
        get => current.Value ?? Euro;
        set => current.Value = value;
    }

    public static bool IsSupported(string? code) =>
        code is not null && Currencies.Contains(code, StringComparer.OrdinalIgnoreCase);

    public static bool IsDenar => Current == Denar;

    /// <summary>"€" or "MKD", for labels that name the unit.</summary>
    public static string Symbol => IsDenar ? Denar : "€";

    /// <summary>Formats a euro amount in the current display currency.</summary>
    public static string ToMoney(this decimal euroAmount, string euroFormat = "C")
    {
        if (!IsDenar)
        {
            return euroAmount.ToString(euroFormat, CultureInfo.CurrentCulture);
        }

        var denars = Math.Round(euroAmount * MkdPerEur, 0, MidpointRounding.AwayFromZero);
        return $"{denars.ToString("N0", CultureInfo.CurrentCulture)} {Denar}";
    }

    /// <summary>Converts a lower price bound entered in the display currency to euros (rounded down, so edge products stay included).</summary>
    public static decimal ToEuroLowerBound(decimal shownAmount) =>
        IsDenar ? Math.Floor(shownAmount / MkdPerEur * 100m) / 100m : shownAmount;

    /// <summary>Converts an upper price bound entered in the display currency to euros (rounded up, so edge products stay included).</summary>
    public static decimal ToEuroUpperBound(decimal shownAmount) =>
        IsDenar ? Math.Ceiling(shownAmount / MkdPerEur * 100m) / 100m : shownAmount;
}
