using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextileShop.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSizeTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductTranslations",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            // Fill in the translated sizes for the products that already have translations (one-time).
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '120 x 170 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '120 x 170 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '130 x 170 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '130 x 170 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '140 x 180 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '140 x 180 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '140 x 240 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '140 x 240 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '140 x 250 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '140 x 250 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '145 - 185 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '145 - 185 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '150 x 200 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '150 x 200 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '190 - 230 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '190 - 230 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '200 x 220 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '200 x 220 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '240 x 260 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '240 x 260 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '40 x 140 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '40 x 140 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '45 x 45 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '45 x 45 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '50 x 50 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '50 x 50 cm');");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Size = '70 x 140 см' WHERE Culture = 'mk' AND Size IS NULL AND ProductId IN (SELECT Id FROM Products WHERE Size = '70 x 140 cm');");
            // The Macedonian descriptions write the unit in Cyrillic too.
            migrationBuilder.Sql("UPDATE ProductTranslations SET Description = REPLACE(Description, ' cm', ' см') WHERE Culture = 'mk' AND Description LIKE '% cm%';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductTranslations");
        }
    }
}
