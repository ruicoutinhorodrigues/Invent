using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class TicketChange5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenIssue",
                table: "Tickets",
                newName: "ClosedIssue");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CloseDate",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClosedIssue",
                table: "Tickets",
                newName: "OpenIssue");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CloseDate",
                table: "Tickets",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
