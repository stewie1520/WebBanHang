using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBanHang.Migrations
{
    public partial class FixModelProductsAddWarehouseItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseItems_ProductId",
                table: "WarehouseItems");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ProductId",
                table: "WarehouseItems",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseItems_ProductId",
                table: "WarehouseItems");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ProductId",
                table: "WarehouseItems",
                column: "ProductId");
        }
    }
}
