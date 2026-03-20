using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow_Warehouse.Migrations
{
    /// <inheritdoc />
    public partial class RenameTrackingNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackingId",
                table: "Transactions",
                newName: "TrackingNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackingNumber",
                table: "Transactions",
                newName: "TrackingId");
        }
    }
}
