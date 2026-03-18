using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow_Warehouse.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Products_ProductId",
                table: "LineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLine_Products_ProductId",
                table: "TransactionLine");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLine_Transactions_TransactionId",
                table: "TransactionLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipient_FromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipient_ToId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionLine",
                table: "TransactionLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipient",
                table: "Recipient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LineItems",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Recipient");

            migrationBuilder.RenameTable(
                name: "TransactionLine",
                newName: "TransactionLines");

            migrationBuilder.RenameTable(
                name: "Recipient",
                newName: "Recipients");

            migrationBuilder.RenameTable(
                name: "LineItems",
                newName: "InventoryItems");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLine_TransactionId",
                table: "TransactionLines",
                newName: "IX_TransactionLines_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLine_ProductId",
                table: "TransactionLines",
                newName: "IX_TransactionLines_ProductId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "InventoryItems",
                newName: "Quantity");

            migrationBuilder.RenameIndex(
                name: "IX_LineItems_WarehouseId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_LineItems_ProductId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_ProductId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                table: "TransactionLines",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "TransactionLines",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Recipients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "InventoryItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "InventoryItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionLines",
                table: "TransactionLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipients",
                table: "Recipients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Recipients_WarehouseId",
                table: "InventoryItems",
                column: "WarehouseId",
                principalTable: "Recipients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLines_Products_ProductId",
                table: "TransactionLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLines_Transactions_TransactionId",
                table: "TransactionLines",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipients_FromId",
                table: "Transactions",
                column: "FromId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipients_ToId",
                table: "Transactions",
                column: "ToId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Recipients_WarehouseId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLines_Products_ProductId",
                table: "TransactionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLines_Transactions_TransactionId",
                table: "TransactionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_FromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_ToId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionLines",
                table: "TransactionLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipients",
                table: "Recipients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Recipients");

            migrationBuilder.RenameTable(
                name: "TransactionLines",
                newName: "TransactionLine");

            migrationBuilder.RenameTable(
                name: "Recipients",
                newName: "Recipient");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                newName: "LineItems");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLines_TransactionId",
                table: "TransactionLine",
                newName: "IX_TransactionLine_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLines_ProductId",
                table: "TransactionLine",
                newName: "IX_TransactionLine_ProductId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "LineItems",
                newName: "Amount");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_WarehouseId",
                table: "LineItems",
                newName: "IX_LineItems_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_ProductId",
                table: "LineItems",
                newName: "IX_LineItems_ProductId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                table: "TransactionLine",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "TransactionLine",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Recipient",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "LineItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "LineItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionLine",
                table: "TransactionLine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipient",
                table: "Recipient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineItems",
                table: "LineItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Products_ProductId",
                table: "LineItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Recipient_WarehouseId",
                table: "LineItems",
                column: "WarehouseId",
                principalTable: "Recipient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLine_Products_ProductId",
                table: "TransactionLine",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLine_Transactions_TransactionId",
                table: "TransactionLine",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipient_FromId",
                table: "Transactions",
                column: "FromId",
                principalTable: "Recipient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipient_ToId",
                table: "Transactions",
                column: "ToId",
                principalTable: "Recipient",
                principalColumn: "Id");
        }
    }
}
