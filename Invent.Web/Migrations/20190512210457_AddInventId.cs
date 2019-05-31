using Microsoft.EntityFrameworkCore.Migrations;

namespace Invent.Web.Migrations
{
    public partial class AddInventId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModel_Manufacturer_ManufacturerId",
                table: "ProductModel");

            migrationBuilder.AlterColumn<int>(
                name: "ManufacturerId",
                table: "ProductModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModel_Manufacturer_ManufacturerId",
                table: "ProductModel",
                column: "ManufacturerId",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModel_Manufacturer_ManufacturerId",
                table: "ProductModel");

            migrationBuilder.AlterColumn<int>(
                name: "ManufacturerId",
                table: "ProductModel",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModel_Manufacturer_ManufacturerId",
                table: "ProductModel",
                column: "ManufacturerId",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
