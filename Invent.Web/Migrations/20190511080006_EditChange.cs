using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class EditChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Products",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
