using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace TextileShop.Web.Localization;

/// <summary>
/// Translates the English messages produced by the service layer. Fixed messages are looked up by their text;
/// messages with values in them are matched against the templates below and re-formatted.
/// </summary>
public sealed class MessageTranslator
{
    private static readonly string[] Templates =
    {
        "Quantity must be between 1 and {0}.",
        "Only {0} units of {1} are in stock.",
        "{0} is no longer available. Please remove it from your cart.",
        "An order that is {0} cannot be changed to {1}.",
        "A product can have at most {0} images.",
        "{0}: only JPG, PNG and WEBP images are allowed.",
        "{0}: images must be 5 MB or smaller."
    };

    private static readonly (Regex Pattern, string Template)[] Patterns = Templates
        .Select(template => (new Regex(
            "^" + Regex.Escape(template).Replace(@"\{0}", "(?<a0>.+?)").Replace(@"\{1}", "(?<a1>.+?)") + "$",
            RegexOptions.Singleline | RegexOptions.CultureInvariant), template))
        .ToArray();

    private readonly IStringLocalizer<SharedResource> localizer;

    public MessageTranslator(IStringLocalizer<SharedResource> localizer)
    {
        this.localizer = localizer;
    }

    public string Format(string key, params object[] arguments) => localizer[key, arguments].Value;

    public string Translate(IEnumerable<string> messages) => string.Join(" ", messages.Select(Translate));

    public string Translate(string message)
    {
        var direct = localizer[message];
        if (!direct.ResourceNotFound)
        {
            return direct.Value;
        }

        foreach (var (pattern, template) in Patterns)
        {
            var match = pattern.Match(message);
            if (!match.Success)
            {
                continue;
            }

            var arguments = new List<object>();
            for (var index = 0; match.Groups[$"a{index}"].Success; index++)
            {
                // Values such as an order status ("Pending") have their own translation; names and numbers pass through.
                var value = match.Groups[$"a{index}"].Value;
                var translated = localizer[value];
                arguments.Add(translated.ResourceNotFound ? value : translated.Value);
            }

            return localizer[template, arguments.ToArray()].Value;
        }

        return message;
    }
}
