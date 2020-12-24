using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBanHang.Migrations
{
    public partial class fixWarehouseTransation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "WarehouseTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransactions_ManufacturerId",
                table: "WarehouseTransactions",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactions_Manufacturers_ManufacturerId",
                table: "WarehouseTransactions",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactions_Manufacturers_ManufacturerId",
                table: "WarehouseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTransactions_ManufacturerId",
                table: "WarehouseTransactions");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "WarehouseTransactions");
        }
    }
}
