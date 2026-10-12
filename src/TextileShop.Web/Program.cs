using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using TextileShop.Infrastructure;
using TextileShop.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using TextileShop.Services;
using TextileShop.Web.Localization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    // Behind a TLS-terminating proxy (Render): trust X-Forwarded-* so the app sees https and the real client IP.
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    // Keep login cookies and antiforgery tokens valid across restarts/deploys when a persistent path is configured.
    var dataProtectionKeysPath = builder.Configuration["DataProtection:KeysPath"];
    if (!string.IsNullOrWhiteSpace(dataProtectionKeysPath))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
            .SetApplicationName("TextileShop");
    }

    Money.MkdPerEur = builder.Configuration.GetValue("Currency:MkdPerEur", Money.MkdPerEur);
    var defaultCurrency = builder.Configuration["Currency:Default"];
    if (Money.IsSupported(defaultCurrency))
    {
        Money.Default = defaultCurrency!.ToUpperInvariant();
    }

    builder.Services.AddAppLocalization();
    builder.Services.AddControllersWithViews().AddAppLocalization();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddScoped<IdentityErrorDescriber, LocalizedIdentityErrorDescriber>();

    var app = builder.Build();

    Log.Information("Application starting in {Environment} environment", app.Environment.EnvironmentName);

    using (var scope = app.Services.CreateScope())
    {
        await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
    }

    app.UseForwardedHeaders();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");

    // In production the proxy (Render) already redirects http to https and the app only listens on plain http.
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseStaticFiles();
    app.UseAppLocalization();

    // Storefront shows prices in the visitor's chosen currency; the admin area always works in euros.
    app.Use(async (context, next) =>
    {
        var chosen = context.Request.Cookies[Money.CookieName];
        var isAdmin = context.Request.Path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase);
        Money.Current = isAdmin ? Money.Euro : Money.IsSupported(chosen) ? chosen!.ToUpperInvariant() : Money.Default;
        await next();
    });

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception) when (exception.GetType().Name != "HostAbortedException")
{
    Log.Fatal(exception, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
