using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class TicketChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GuyName",
                table: "Ticket",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuyContact",
                table: "Ticket",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuyContact",
                table: "Ticket");

            migrationBuilder.AlterColumn<string>(
                name: "GuyName",
                table: "Ticket",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
