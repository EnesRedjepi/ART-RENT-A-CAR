namespace TextileShop.Infrastructure.Data;

/// <summary>
/// Albanian, Macedonian and Turkish words for the brands, materials and colors of the sample catalog,
/// in the order of <see cref="Cultures"/>. A missing value means the English text is used as it is.
/// </summary>
internal static class CatalogVocabulary
{
    private static readonly string[] Cultures = { "sq", "mk", "tr" };

    private static readonly Dictionary<string, string?[]> Brands = new()
    {
        ["Nordic Home"] = new string?[] { null, "Нордик Хоум", null },
        ["ComfortFit"] = new string?[] { null, "КомфортФит", null },
        ["Casa Dorma"] = new string?[] { null, "Каза Дорма", null },
        ["LuxeWindow"] = new string?[] { null, "ЛуксВиндоу", null },
        ["HomeBasics"] = new string?[] { null, "ХоумБејсикс", null },
    };

    private static readonly Dictionary<string, string?[]> Materials = new()
    {
        ["Fleece"] = new string?[] { "Fleece", "Флис", "Polar" },
        ["Wool blend"] = new string?[] { "Përzierje leshi", "Мешавина на волна", "Yün karışımı" },
        ["Polyester spandex"] = new string?[] { "Poliestër me spandex", "Полиестер со спандекс", "Polyester elastan" },
        ["Velvet"] = new string?[] { "Kadife", "Кадифе", "Kadife" },
        ["Cotton"] = new string?[] { "Pambuk", "Памук", "Pamuk" },
        ["Polyester"] = new string?[] { "Poliestër", "Полиестер", "Polyester" },
        ["Linen blend"] = new string?[] { "Përzierje liri", "Мешавина на лен", "Keten karışımı" },
        ["Boucle"] = new string?[] { "Bukle", "Букле", "Bukle" },
        ["PVC coated"] = new string?[] { "I veshur me PVC", "Обложен со PVC", "PVC kaplı" },
        ["Jacquard"] = new string?[] { "Xhakard", "Жакард", "Jakar" },
        ["Bamboo"] = new string?[] { "Bambu", "Бамбус", "Bambu" },
        ["Polypropylene"] = new string?[] { "Polipropilen", "Полипропилен", "Polipropilen" },
    };

    private static readonly Dictionary<string, string?[]> Colors = new()
    {
        ["Grey"] = new string?[] { "Gri", "Сива", "Gri" },
        ["Beige"] = new string?[] { "Bezhë", "Беж", "Bej" },
        ["Charcoal"] = new string?[] { "Antracit", "Темносива", "Antrasit" },
        ["Emerald"] = new string?[] { "Smerald", "Смарагдна", "Zümrüt yeşili" },
        ["Sand"] = new string?[] { "Ranë", "Песочна", "Kum rengi" },
        ["White"] = new string?[] { "E bardhë", "Бела", "Beyaz" },
        ["Navy"] = new string?[] { "Blu marine", "Морнарскосина", "Lacivert" },
        ["Natural"] = new string?[] { "Natyrale", "Природна", "Doğal" },
        ["Cream"] = new string?[] { "Krem", "Крем", "Krem" },
        ["Terracotta"] = new string?[] { "Terrakotë", "Теракота", "Kiremit" },
        ["Gold"] = new string?[] { "Ar", "Златна", "Altın" },
        ["Sage"] = new string?[] { "Jeshile e çelët", "Светлозелена", "Adaçayı yeşili" },
    };

    private static readonly Dictionary<string, string?[]> Sizes = new()
    {
        ["120 x 170 cm"] = new string?[] { null, "120 x 170 см", null },
        ["130 x 170 cm"] = new string?[] { null, "130 x 170 см", null },
        ["140 x 180 cm"] = new string?[] { null, "140 x 180 см", null },
        ["140 x 240 cm"] = new string?[] { null, "140 x 240 см", null },
        ["140 x 250 cm"] = new string?[] { null, "140 x 250 см", null },
        ["145 - 185 cm"] = new string?[] { null, "145 - 185 см", null },
        ["150 x 200 cm"] = new string?[] { null, "150 x 200 см", null },
        ["190 - 230 cm"] = new string?[] { null, "190 - 230 см", null },
        ["200 x 220 cm"] = new string?[] { null, "200 x 220 см", null },
        ["240 x 260 cm"] = new string?[] { null, "240 x 260 см", null },
        ["40 x 140 cm"] = new string?[] { null, "40 x 140 см", null },
        ["45 x 45 cm"] = new string?[] { null, "45 x 45 см", null },
        ["50 x 50 cm"] = new string?[] { null, "50 x 50 см", null },
        ["70 x 140 cm"] = new string?[] { null, "70 x 140 см", null },
    };

    public static string? Brand(string? english, string culture) => Translate(Brands, english, culture);

    public static string? Material(string? english, string culture) => Translate(Materials, english, culture);

    public static string? Color(string? english, string culture) => Translate(Colors, english, culture);

    public static string? Size(string? english, string culture) => Translate(Sizes, english, culture);

    private static string? Translate(Dictionary<string, string?[]> vocabulary, string? english, string culture)
    {
        var index = Array.IndexOf(Cultures, culture);
        return english is not null && index >= 0 && vocabulary.TryGetValue(english, out var values) ? values[index] : null;
    }
}
