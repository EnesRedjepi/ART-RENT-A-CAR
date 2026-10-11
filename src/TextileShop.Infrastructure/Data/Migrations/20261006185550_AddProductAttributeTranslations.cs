using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextileShop.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAttributeTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ProductTranslations",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductTranslations",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "ProductTranslations",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            // Fill in the words for the products that already have translated names (one-time).
            migrationBuilder.Sql("UPDATE ProductTranslations SET Brand = 'Нордик Хоум' WHERE Culture = 'mk' AND Brand IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Brand = 'Nordic Home');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Brand = 'КомфортФит' WHERE Culture = 'mk' AND Brand IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Brand = 'ComfortFit');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Brand = 'Каза Дорма' WHERE Culture = 'mk' AND Brand IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Brand = 'Casa Dorma');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Brand = 'ЛуксВиндоу' WHERE Culture = 'mk' AND Brand IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Brand = 'LuxeWindow');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Brand = 'ХоумБејсикс' WHERE Culture = 'mk' AND Brand IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Brand = 'HomeBasics');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Fleece' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Fleece');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Флис' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Fleece');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Polar' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Fleece');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Përzierje leshi' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Wool blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Мешавина на волна' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Wool blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Yün karışımı' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Wool blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Poliestër me spandex' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester spandex');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Полиестер со спандекс' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester spandex');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Polyester elastan' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester spandex');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Kadife' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Velvet');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Кадифе' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Velvet');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Kadife' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Velvet');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Pambuk' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Cotton');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Памук' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Cotton');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Pamuk' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Cotton');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Poliestër' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Полиестер' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Polyester' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polyester');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Përzierje liri' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Linen blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Мешавина на лен' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Linen blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Keten karışımı' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Linen blend');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Bukle' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Boucle');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Букле' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Boucle');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Bukle' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Boucle');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'I veshur me PVC' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'PVC coated');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Обложен со PVC' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'PVC coated');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'PVC kaplı' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'PVC coated');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Xhakard' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Jacquard');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Жакард' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Jacquard');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Jakar' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Jacquard');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Bambu' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Bamboo');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Бамбус' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Bamboo');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Bambu' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Bamboo');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Polipropilen' WHERE Culture = 'sq' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polypropylene');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Полипропилен' WHERE Culture = 'mk' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polypropylene');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Material = 'Polipropilen' WHERE Culture = 'tr' AND Material IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Material = 'Polypropylene');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Gri' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Grey');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Сива' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Grey');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Gri' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Grey');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Bezhë' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Beige');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Беж' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Beige');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Bej' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Beige');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Antracit' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Charcoal');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Темносива' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Charcoal');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Antrasit' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Charcoal');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Smerald' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Emerald');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Смарагдна' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Emerald');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Zümrüt yeşili' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Emerald');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Ranë' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sand');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Песочна' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sand');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Kum rengi' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sand');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'E bardhë' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'White');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Бела' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'White');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Beyaz' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'White');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Blu marine' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Navy');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Морнарскосина' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Navy');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Lacivert' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Navy');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Natyrale' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Natural');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Природна' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Natural');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Doğal' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Natural');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Krem' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Cream');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Крем' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Cream');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Krem' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Cream');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Terrakotë' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Terracotta');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Теракота' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Terracotta');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Kiremit' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Terracotta');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Ar' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Gold');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Златна' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Gold');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Altın' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Gold');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Jeshile e çelët' WHERE Culture = 'sq' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sage');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Светлозелена' WHERE Culture = 'mk' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sage');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Color = 'Adaçayı yeşili' WHERE Culture = 'tr' AND Color IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Color = 'Sage');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ProductTranslations");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductTranslations");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "ProductTranslations");
        }
    }
}
