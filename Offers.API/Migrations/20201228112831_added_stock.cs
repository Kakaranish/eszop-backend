using Microsoft.EntityFrameworkCore.Migrations;

namespace Offers.API.Migrations
{
    public partial class added_stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableStock",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalStock",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableStock",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "TotalStock",
                table: "Offers");
        }
    }
}
