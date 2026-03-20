using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow_Warehouse.Migrations
{
    /// <inheritdoc />
    public partial class MiscChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Transactions_TransactionId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_TransactionId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "LineItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "LineItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLine_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionLine_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLine_ProductId",
                table: "TransactionLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLine_TransactionId",
                table: "TransactionLine",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems",
                column: "WarehouseId",
                principalTable: "Recipient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems");

            migrationBuilder.DropTable(
                name: "TransactionLine");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "LineItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "LineItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_TransactionId",
                table: "LineItems",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems",
                column: "WarehouseId",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Transactions_TransactionId",
                table: "LineItems",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}
