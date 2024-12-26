using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderWithOrderIdAndAddOrderLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "Id");
        }
    }
}
