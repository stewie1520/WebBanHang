using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBanHang.Migrations
{
    public partial class FixWarehouseItemIndexOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "WarehouseItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "WarehouseItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
