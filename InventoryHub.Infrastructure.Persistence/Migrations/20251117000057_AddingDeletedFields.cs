using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingDeletedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_MovementTypes_MovementTypeId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "MovementTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MovementTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InventoryMovements",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InventoryMovements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsDeleted",
                table: "Products",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MovementTypes_IsDeleted",
                table: "MovementTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_IsDeleted",
                table: "InventoryMovements",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IsDeleted",
                table: "Inventories",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_MovementTypes_MovementTypeId",
                table: "InventoryMovements",
                column: "MovementTypeId",
                principalTable: "MovementTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_MovementTypes_MovementTypeId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements");

            migrationBuilder.DropIndex(
                name: "IX_Products_IsDeleted",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_MovementTypes_IsDeleted",
                table: "MovementTypes");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovements_IsDeleted",
                table: "InventoryMovements");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_IsDeleted",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "MovementTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MovementTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InventoryMovements");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InventoryMovements");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inventories");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_MovementTypes_MovementTypeId",
                table: "InventoryMovements",
                column: "MovementTypeId",
                principalTable: "MovementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Products_ProductId",
                table: "InventoryMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
