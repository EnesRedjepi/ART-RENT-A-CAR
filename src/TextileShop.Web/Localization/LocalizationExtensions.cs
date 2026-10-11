using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TextileShop.Services.Common;

namespace TextileShop.Web.Localization;

public static class LocalizationExtensions
{
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddSingleton<MessageTranslator>();
        return services;
    }

    public static IMvcBuilder AddAppLocalization(this IMvcBuilder builder)
    {
        builder.AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)));

        // Registered after MVC's own setup so our metadata providers run after the data-annotations provider,
        // which is the one that adds the validation attributes we give default messages to.
        builder.Services.ConfigureOptions<LocalizedMvcOptionsSetup>();
        return builder;
    }

    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
    {
        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(SupportedLanguages.FormattingCulture, SupportedLanguages.DefaultUiCulture),
            SupportedCultures = new[] { new CultureInfo(SupportedLanguages.FormattingCulture) },
            SupportedUICultures = SupportedLanguages.All.Select(language => new CultureInfo(language.Code)).ToList()
        };
        options.RequestCultureProviders = new IRequestCultureProvider[]
        {
            new CookieRequestCultureProvider(),
            new AcceptLanguageHeaderRequestCultureProvider()
        };
        return app.UseRequestLocalization(options);
    }

    /// <summary>The localized text as a quoted JavaScript string literal, safe inside an HTML attribute.</summary>
    public static string JsString(this LocalizedString text) => JsonSerializer.Serialize(text.Value);

    public static string Tr(this Controller controller, string key, params object[] arguments) =>
        controller.HttpContext.RequestServices.GetRequiredService<MessageTranslator>().Format(key, arguments);

    public static string ErrorText(this Controller controller, ServiceResult result) =>
        controller.HttpContext.RequestServices.GetRequiredService<MessageTranslator>().Translate(result.Errors);

    public static void AddErrors(this Controller controller, ServiceResult result)
    {
        var translator = controller.HttpContext.RequestServices.GetRequiredService<MessageTranslator>();
        foreach (var error in result.Errors)
        {
            controller.ModelState.AddModelError(string.Empty, translator.Translate(error));
        }
    }
}
