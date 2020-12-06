using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBanHang.Migrations
{
    public partial class FixWarehouseTransactionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactionItem_Products_ProductId",
                table: "WarehouseTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactionItem_WarehouseTransactions_WarehouseTransactionId",
                table: "WarehouseTransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseTransactionItem",
                table: "WarehouseTransactionItem");

            migrationBuilder.RenameTable(
                name: "WarehouseTransactionItem",
                newName: "WarehouseTransactionItems");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactionItem_WarehouseTransactionId",
                table: "WarehouseTransactionItems",
                newName: "IX_WarehouseTransactionItems_WarehouseTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactionItem_ProductId",
                table: "WarehouseTransactionItems",
                newName: "IX_WarehouseTransactionItems_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseTransactionId",
                table: "WarehouseTransactionItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseTransactionItems",
                table: "WarehouseTransactionItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactionItems_Products_ProductId",
                table: "WarehouseTransactionItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactionItems_WarehouseTransactions_WarehouseTransactionId",
                table: "WarehouseTransactionItems",
                column: "WarehouseTransactionId",
                principalTable: "WarehouseTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactionItems_Products_ProductId",
                table: "WarehouseTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransactionItems_WarehouseTransactions_WarehouseTransactionId",
                table: "WarehouseTransactionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseTransactionItems",
                table: "WarehouseTransactionItems");

            migrationBuilder.RenameTable(
                name: "WarehouseTransactionItems",
                newName: "WarehouseTransactionItem");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactionItems_WarehouseTransactionId",
                table: "WarehouseTransactionItem",
                newName: "IX_WarehouseTransactionItem_WarehouseTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTransactionItems_ProductId",
                table: "WarehouseTransactionItem",
                newName: "IX_WarehouseTransactionItem_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseTransactionId",
                table: "WarehouseTransactionItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseTransactionItem",
                table: "WarehouseTransactionItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactionItem_Products_ProductId",
                table: "WarehouseTransactionItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransactionItem_WarehouseTransactions_WarehouseTransactionId",
                table: "WarehouseTransactionItem",
                column: "WarehouseTransactionId",
                principalTable: "WarehouseTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
