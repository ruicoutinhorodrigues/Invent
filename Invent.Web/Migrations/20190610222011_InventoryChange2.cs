using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class InventoryChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotAvailable",
                table: "Inventory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotAvailable",
                table: "Inventory");
        }
    }
}
