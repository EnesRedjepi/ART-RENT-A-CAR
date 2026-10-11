using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TextileShop.Domain.Constants;
using TextileShop.Domain.Entities;
using TextileShop.Services.Common;

namespace TextileShop.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DatabaseSeeder));

        await context.Database.MigrateAsync();
        await SeedRolesAsync(services.GetRequiredService<RoleManager<IdentityRole>>());
        await SeedAdministratorAsync(services, logger);
        await SeedCatalogAsync(context, services.GetRequiredService<TimeProvider>(), logger);
        await CatalogTranslationSeed.SeedAsync(context, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in new[] { Roles.Administrator, Roles.RetailCustomer, Roles.WholesaleCustomer })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdministratorAsync(IServiceProvider services, ILogger logger)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var email = configuration["SeedAdministrator:Email"];
        var password = configuration["SeedAdministrator:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return;
        }

        var administrator = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = "Store Administrator",
            CreatedAt = services.GetRequiredService<TimeProvider>().GetUtcNow().UtcDateTime
        };

        var creation = await userManager.CreateAsync(administrator, password);
        if (!creation.Succeeded)
        {
            logger.LogError("Seeding the administrator account failed: {Errors}", string.Join(", ", creation.Errors.Select(error => error.Code)));
            return;
        }

        await userManager.AddToRoleAsync(administrator, Roles.Administrator);
        logger.LogInformation("Administrator account {UserId} seeded", administrator.Id);
    }

    private static async Task SeedCatalogAsync(AppDbContext context, TimeProvider timeProvider, ILogger logger)
    {
        if (await context.Categories.AnyAsync())
        {
            return;
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var categories = new[]
        {
            NewCategory("Blankets", "Soft throws and warm blankets for every season."),
            NewCategory("Sofa Covers", "Stretch and tailored covers that protect and refresh your sofa."),
            NewCategory("Bed Covers", "Bedspreads, quilts and duvet covers."),
            NewCategory("Curtains", "Light-filtering and blackout curtains."),
            NewCategory("Pillow Covers", "Decorative cushion and pillow covers."),
            NewCategory("Table Covers", "Tablecloths and runners for everyday and festive occasions."),
            NewCategory("Other Home Textiles", "Rugs, towels and more for the whole home.")
        };

        context.Categories.AddRange(categories);

        context.Products.AddRange(
            NewProduct("Cozy Fleece Blanket", "BLK-FLC-001", categories[0], 25m, 18m, 5, 120, "Nordic Home", "Fleece", "150 x 200 cm", "Grey", now),
            NewProduct("Wool Blend Throw", "BLK-WOL-002", categories[0], 49m, 38m, 4, 60, "Nordic Home", "Wool blend", "130 x 170 cm", "Beige", now),
            NewProduct("Stretch Sofa Cover 3-Seater", "SOF-STR-001", categories[1], 39m, 29m, 5, 45, "ComfortFit", "Polyester spandex", "190 - 230 cm", "Charcoal", now),
            NewProduct("Velvet Sofa Cover 2-Seater", "SOF-VLV-002", categories[1], 45m, 34m, 5, 3, "ComfortFit", "Velvet", "145 - 185 cm", "Emerald", now),
            NewProduct("Quilted Bedspread King", "BED-QLT-001", categories[2], 79m, 62m, 3, 30, "Casa Dorma", "Cotton", "240 x 260 cm", "Sand", now),
            NewProduct("Cotton Duvet Cover Set", "BED-DUV-002", categories[2], 55m, 42m, 4, 70, "Casa Dorma", "Cotton", "200 x 220 cm", "White", now),
            NewProduct("Blackout Curtain Pair", "CUR-BLK-001", categories[3], 34m, 25m, 6, 80, "LuxeWindow", "Polyester", "140 x 250 cm", "Navy", now),
            NewProduct("Linen Look Curtain", "CUR-LIN-002", categories[3], 29m, 21m, 6, 55, "LuxeWindow", "Linen blend", "140 x 240 cm", "Natural", now),
            NewProduct("Boucle Pillow Cover 45x45", "PIL-BOU-001", categories[4], 9m, 6m, 10, 200, "Nordic Home", "Boucle", "45 x 45 cm", "Cream", now),
            NewProduct("Embroidered Cushion Cover", "PIL-EMB-002", categories[4], 12m, 8.5m, 10, 4, "Casa Dorma", "Cotton", "50 x 50 cm", "Terracotta", now),
            NewProduct("Waterproof Tablecloth", "TAB-WTR-001", categories[5], 19m, 14m, 6, 90, "HomeBasics", "PVC coated", "140 x 180 cm", "White", now),
            NewProduct("Jacquard Table Runner", "TAB-JAC-002", categories[5], 15m, 11m, 8, 65, "HomeBasics", "Jacquard", "40 x 140 cm", "Gold", now),
            NewProduct("Bamboo Bath Towel Set", "OTH-TWL-001", categories[6], 32m, 24m, 5, 75, "Casa Dorma", "Bamboo", "70 x 140 cm", "Sage", now),
            NewProduct("Anti-Slip Area Rug", "OTH-RUG-002", categories[6], 59m, 45m, 3, 25, "HomeBasics", "Polypropylene", "120 x 170 cm", "Grey", now));

        await context.SaveChangesAsync();
        logger.LogInformation("Sample catalog seeded");
    }

    private static Category NewCategory(string name, string description) => new()
    {
        Name = name,
        Slug = SlugGenerator.Generate(name),
        Description = description
    };

    private static Product NewProduct(
        string name,
        string sku,
        Category category,
        decimal price,
        decimal wholesalePrice,
        int minimumWholesaleQuantity,
        int stockQuantity,
        string brand,
        string material,
        string size,
        string color,
        DateTime now) => new()
    {
        Name = name,
        Sku = sku,
        Category = category,
        Price = price,
        WholesalePrice = wholesalePrice,
        MinimumWholesaleQuantity = minimumWholesaleQuantity,
        StockQuantity = stockQuantity,
        Brand = brand,
        Material = material,
        Size = size,
        Color = color,
        Description = $"{name} made from quality {material.ToLowerInvariant()}. Available in {color.ToLowerInvariant()}, size {size}.",
        IsActive = true,
        CreatedAt = now,
        UpdatedAt = now
    };
}
