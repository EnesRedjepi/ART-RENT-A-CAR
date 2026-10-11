using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextileShop.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImproveMacedonianCatalogWording : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Use "Покривки" for table covers too, as for bed covers (one-time, only touches the seeded Macedonian wording).
            migrationBuilder.Sql("UPDATE CategoryTranslations SET Name = REPLACE(Name, 'Прекривки за маса', 'Покривки за маса'), Description = REPLACE(Description, 'Прекривки и ранери', 'Покривки и ранери') WHERE Culture = 'mk';");
            migrationBuilder.Sql("UPDATE ProductTranslations SET Name = REPLACE(Name, 'Водоотпорна прекривка', 'Водоотпорна покривка'), Description = REPLACE(Description, 'Водоотпорна прекривка', 'Водоотпорна покривка') WHERE Culture = 'mk';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
