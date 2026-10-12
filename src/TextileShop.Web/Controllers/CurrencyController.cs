using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Controllers;

public class CurrencyController : Controller
{
    private static readonly string[] PriceFilterKeys = { "minPrice", "maxPrice" };

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Set(string currency, string? returnUrl = null)
    {
        if (Money.IsSupported(currency))
        {
            Response.Cookies.Append(
                Money.CookieName,
                currency.ToUpperInvariant(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = Request.IsHttps
                });
        }

        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? WithoutPriceFilters(returnUrl!) : "~/");
    }

    /// <summary>Min/max price filters are typed in the displayed currency, so they don't survive a currency switch.</summary>
    private static string WithoutPriceFilters(string url)
    {
        var queryStart = url.IndexOf('?');
        if (queryStart < 0)
        {
            return url;
        }

        var path = url[..queryStart];
        var query = QueryHelpers.ParseQuery(url[queryStart..]);
        var kept = query
            .Where(pair => !PriceFilterKeys.Contains(pair.Key, StringComparer.OrdinalIgnoreCase))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        return kept.Count == 0 ? path : QueryHelpers.AddQueryString(path, kept);
    }
}
