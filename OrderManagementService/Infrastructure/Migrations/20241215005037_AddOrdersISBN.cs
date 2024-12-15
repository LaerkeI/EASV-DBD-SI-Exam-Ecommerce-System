using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookISBN",
                table: "Orders");
        }
    }
}
