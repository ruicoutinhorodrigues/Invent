using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class LastChangeDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeaveDate",
                table: "Products",
                newName: "LastChangeDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastChangeDate",
                table: "Products",
                newName: "LeaveDate");
        }
    }
}
