using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TextileShop.Domain.Entities;

namespace TextileShop.Infrastructure.Data;

/// <summary>
/// Albanian, Macedonian and Turkish names and descriptions for the sample catalog. They are only added to categories
/// and products that have no translations at all, so anything an administrator edits or removes is never overwritten.
/// </summary>
internal static class CatalogTranslationSeed
{
    private sealed record Text(string Culture, string Name, string Description);

    private static readonly Dictionary<string, Text[]> Categories = new()
    {
        ["blankets"] = new Text[]
        {
            new("sq", "Batanije", "Batanije të buta dhe të ngrohta për çdo stinë."),
            new("mk", "Ќебиња", "Меки прекривки и топли ќебиња за секоја сезона."),
            new("tr", "Battaniyeler", "Her mevsim için yumuşak örtüler ve sıcak battaniyeler.")
        },
        ["sofa-covers"] = new Text[]
        {
            new("sq", "Mbulesa divani", "Mbulesa elastike dhe të përshtatura që e mbrojnë dhe e freskojnë divanin tuaj."),
            new("mk", "Навлаки за кауч", "Растегливи и прилагодени навлаки што го штитат и освежуваат вашиот кауч."),
            new("tr", "Koltuk Örtüleri", "Koltuğunuzu koruyan ve yenileyen esnek ve kalıp koltuk örtüleri.")
        },
        ["bed-covers"] = new Text[]
        {
            new("sq", "Mbulesa shtrati", "Mbulesa shtrati, jorganë të qepur dhe këllëfë jorgani."),
            new("mk", "Покривки за кревет", "Покривки, прошиени ќебиња и навлаки за јорган."),
            new("tr", "Yatak Örtüleri", "Yatak örtüleri, kapitone yorganlar ve nevresimlar.")
        },
        ["curtains"] = new Text[]
        {
            new("sq", "Perde", "Perde që filtrojnë dritën dhe perde errësuese."),
            new("mk", "Завеси", "Завеси што ја филтрираат светлината и завеси за затемнување."),
            new("tr", "Perdeler", "Işık filtreleyen ve karartma perdeleri.")
        },
        ["pillow-covers"] = new Text[]
        {
            new("sq", "Këllëfë jastëkësh", "Këllëfë dekorativë për jastëkë dhe jastëkë dekorativë."),
            new("mk", "Навлаки за перници", "Декоративни навлаки за перници и перничиња."),
            new("tr", "Yastık Kılıfları", "Dekoratif kırlent ve yastık kılıfları.")
        },
        ["table-covers"] = new Text[]
        {
            new("sq", "Mbulesa tavoline", "Mbulesa dhe vrapues tavoline për përdorim të përditshëm dhe raste festive."),
            new("mk", "Покривки за маса", "Покривки и ранери за маса за секојдневна употреба и за празнични прилики."),
            new("tr", "Masa Örtüleri", "Günlük kullanım ve özel günler için masa örtüleri ve runnerlar.")
        },
        ["other-home-textiles"] = new Text[]
        {
            new("sq", "Tekstile të tjera për shtëpi", "Qilima, peshqirë dhe më shumë për gjithë shtëpinë."),
            new("mk", "Друг домашен текстил", "Теписи, крпи и повеќе за целиот дом."),
            new("tr", "Diğer Ev Tekstilleri", "Tüm ev için halılar, havlular ve daha fazlası.")
        }
    };

    private static readonly Dictionary<string, Text[]> Products = new()
    {
        ["BLK-FLC-001"] = new Text[]
        {
            new("sq", "Batanije Fleece e ngrohtë", "Batanije Fleece e ngrohtë prej fleece cilësor. E disponueshme në gri, përmasa 150 x 200 cm."),
            new("mk", "Топло флис ќебе", "Топло флис ќебе од квалитетен флис. Достапно во сива боја, големина 150 x 200 см."),
            new("tr", "Rahat Polar Battaniye", "Kaliteli polardan üretilmiş rahat polar battaniye. Gri renkte, 150 x 200 cm ölçüsünde mevcut.")
        },
        ["BLK-WOL-002"] = new Text[]
        {
            new("sq", "Batanije me përzierje leshi", "Batanije dekorative prej përzierjeje leshi cilësore. E disponueshme në bezhë, përmasa 130 x 170 cm."),
            new("mk", "Ќебе од мешавина на волна", "Ќебе од квалитетна мешавина на волна. Достапно во беж боја, големина 130 x 170 см."),
            new("tr", "Yün Karışımlı Battaniye", "Kaliteli yün karışımından üretilmiş battaniye. Bej renkte, 130 x 170 cm ölçüsünde mevcut.")
        },
        ["SOF-STR-001"] = new Text[]
        {
            new("sq", "Mbulesë elastike për divan 3-vendësh", "Mbulesë elastike për divan 3-vendësh prej poliestri me spandex cilësor. E disponueshme në antracit, përmasa 190 - 230 cm."),
            new("mk", "Растеглива навлака за троседот", "Растеглива навлака за троседот од квалитетен полиестер со спандекс. Достапна во темносива боја, големина 190 - 230 см."),
            new("tr", "Esnek 3 Kişilik Koltuk Örtüsü", "Kaliteli polyester elastandan üretilmiş esnek 3 kişilik koltuk örtüsü. Antrasit renkte, 190 - 230 cm ölçüsünde mevcut.")
        },
        ["SOF-VLV-002"] = new Text[]
        {
            new("sq", "Mbulesë kadifeje për divan 2-vendësh", "Mbulesë për divan 2-vendësh prej kadifeje cilësore. E disponueshme në smerald, përmasa 145 - 185 cm."),
            new("mk", "Кадифена навлака за двосед", "Навлака за двосед од квалитетна кадифа. Достапна во смарагдна боја, големина 145 - 185 см."),
            new("tr", "Kadife 2 Kişilik Koltuk Örtüsü", "Kaliteli kadifeden üretilmiş 2 kişilik koltuk örtüsü. Zümrüt yeşili renkte, 145 - 185 cm ölçüsünde mevcut.")
        },
        ["BED-QLT-001"] = new Text[]
        {
            new("sq", "Mbulesë shtrati e qepur King", "Mbulesë shtrati e qepur King prej pambuku cilësor. E disponueshme në ngjyrë rëre, përmasa 240 x 260 cm."),
            new("mk", "Прошиена покривка за кревет King", "Прошиена покривка за кревет King од квалитетен памук. Достапна во песочна боја, големина 240 x 260 см."),
            new("tr", "Kapitone Yatak Örtüsü King", "Kaliteli pamuktan üretilmiş kapitone King yatak örtüsü. Kum renginde, 240 x 260 cm ölçüsünde mevcut.")
        },
        ["BED-DUV-002"] = new Text[]
        {
            new("sq", "Set këllëfësh jorgani prej pambuku", "Set këllëfësh jorgani prej pambuku cilësor. I disponueshëm në të bardhë, përmasa 200 x 220 cm."),
            new("mk", "Сет навлаки за јорган од памук", "Сет навлаки за јорган од квалитетен памук. Достапен во бела боја, големина 200 x 220 см."),
            new("tr", "Pamuklu Nevresim Takımı", "Kaliteli pamuktan üretilmiş nevresim takımı. Beyaz renkte, 200 x 220 cm ölçüsünde mevcut.")
        },
        ["CUR-BLK-001"] = new Text[]
        {
            new("sq", "Çift perdesh errësuese", "Çift perdesh errësuese prej poliestri cilësor. Të disponueshme në blu marine, përmasa 140 x 250 cm."),
            new("mk", "Пар завеси за затемнување", "Пар завеси за затемнување од квалитетен полиестер. Достапни во морнарскосина боја, големина 140 x 250 см."),
            new("tr", "Karartma Perde Çifti", "Kaliteli polyesterden üretilmiş karartma perde çifti. Lacivert renkte, 140 x 250 cm ölçüsünde mevcut.")
        },
        ["CUR-LIN-002"] = new Text[]
        {
            new("sq", "Perde me pamje liri", "Perde me pamje liri prej përzierjeje liri cilësore. E disponueshme në ngjyrë natyrale, përmasa 140 x 240 cm."),
            new("mk", "Завеса со изглед на лен", "Завеса со изглед на лен од квалитетна мешавина на лен. Достапна во природна боја, големина 140 x 240 см."),
            new("tr", "Keten Görünümlü Perde", "Kaliteli keten karışımından üretilmiş keten görünümlü perde. Doğal renkte, 140 x 240 cm ölçüsünde mevcut.")
        },
        ["PIL-BOU-001"] = new Text[]
        {
            new("sq", "Këllëf jastëku bukle 45x45", "Këllëf jastëku prej bukle cilësore. I disponueshëm në krem, përmasa 45 x 45 cm."),
            new("mk", "Навлака за перница букле 45x45", "Навлака за перница од квалитетно букле. Достапна во крем боја, големина 45 x 45 см."),
            new("tr", "Bukle Yastık Kılıfı 45x45", "Kaliteli bukle kumaştan üretilmiş yastık kılıfı. Krem renkte, 45 x 45 cm ölçüsünde mevcut.")
        },
        ["PIL-EMB-002"] = new Text[]
        {
            new("sq", "Këllëf jastëku me qëndisje", "Këllëf jastëku me qëndisje prej pambuku cilësor. I disponueshëm në terrakotë, përmasa 50 x 50 cm."),
            new("mk", "Везена навлака за перниче", "Везена навлака за перниче од квалитетен памук. Достапна во теракота боја, големина 50 x 50 см."),
            new("tr", "İşlemeli Kırlent Kılıfı", "Kaliteli pamuktan üretilmiş işlemeli kırlent kılıfı. Kiremit renginde, 50 x 50 cm ölçüsünde mevcut.")
        },
        ["TAB-WTR-001"] = new Text[]
        {
            new("sq", "Mbulesë tavoline kundër ujit", "Mbulesë tavoline kundër ujit, me veshje PVC cilësore. E disponueshme në të bardhë, përmasa 140 x 180 cm."),
            new("mk", "Водоотпорна покривка за маса", "Водоотпорна покривка за маса со квалитетна PVC обвивка. Достапна во бела боја, големина 140 x 180 см."),
            new("tr", "Su Geçirmez Masa Örtüsü", "Kaliteli PVC kaplamalı su geçirmez masa örtüsü. Beyaz renkte, 140 x 180 cm ölçüsünde mevcut.")
        },
        ["TAB-JAC-002"] = new Text[]
        {
            new("sq", "Vrapues tavoline xhakard", "Vrapues tavoline prej xhakardi cilësor. I disponueshëm në ar, përmasa 40 x 140 cm."),
            new("mk", "Жакард ранер за маса", "Ранер за маса од квалитетен жакард. Достапен во златна боја, големина 40 x 140 см."),
            new("tr", "Jakarlı Masa Runner’ı", "Kaliteli jakardan üretilmiş masa runner’ı. Altın renkte, 40 x 140 cm ölçüsünde mevcut.")
        },
        ["OTH-TWL-001"] = new Text[]
        {
            new("sq", "Set peshqirësh banjo prej bambuje", "Set peshqirësh banjo prej bambuje cilësor. I disponueshëm në jeshile të çelët, përmasa 70 x 140 cm."),
            new("mk", "Сет крпи за бања од бамбус", "Сет крпи за бања од квалитетен бамбус. Достапен во светлозелена боја, големина 70 x 140 см."),
            new("tr", "Bambu Banyo Havlusu Seti", "Kaliteli bambudan üretilmiş banyo havlusu seti. Adaçayı yeşili renkte, 70 x 140 cm ölçüsünde mevcut.")
        },
        ["OTH-RUG-002"] = new Text[]
        {
            new("sq", "Qilim kundër rrëshqitjes", "Qilim kundër rrëshqitjes prej polipropileni cilësor. I disponueshëm në gri, përmasa 120 x 170 cm."),
            new("mk", "Нелизгачки тепих", "Нелизгачки тепих од квалитетен полипропилен. Достапен во сива боја, големина 120 x 170 см."),
            new("tr", "Kaymaz Taban Halı", "Kaliteli polipropilenden üretilmiş kaymaz taban halı. Gri renkte, 120 x 170 cm ölçüsünde mevcut.")
        }
    };

    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        var added = 0;

        foreach (var category in await context.Categories.Include(category => category.Translations).ToListAsync())
        {
            if (category.Translations.Count > 0 || !Categories.TryGetValue(category.Slug, out var texts))
            {
                continue;
            }

            foreach (var text in texts)
            {
                category.Translations.Add(new CategoryTranslation { Culture = text.Culture, Name = text.Name, Description = text.Description });
                added++;
            }
        }

        foreach (var product in await context.Products.Include(product => product.Translations).ToListAsync())
        {
            if (product.Translations.Count > 0 || !Products.TryGetValue(product.Sku, out var texts))
            {
                continue;
            }

            foreach (var text in texts)
            {
                product.Translations.Add(new ProductTranslation
                {
                    Culture = text.Culture,
                    Name = text.Name,
                    Description = text.Description,
                    Brand = CatalogVocabulary.Brand(product.Brand, text.Culture),
                    Material = CatalogVocabulary.Material(product.Material, text.Culture),
                    Color = CatalogVocabulary.Color(product.Color, text.Culture),
                    Size = CatalogVocabulary.Size(product.Size, text.Culture)
                });
                added++;
            }
        }

        if (added > 0)
        {
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} catalog translations", added);
        }
    }
}
