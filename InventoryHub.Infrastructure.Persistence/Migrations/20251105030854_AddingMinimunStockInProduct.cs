using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingMinimunStockInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Products");
        }
    }
}
