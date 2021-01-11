using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Offers.API.Migrations
{
    public partial class DeliveryMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethods",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Offers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryMethods",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Offers");
        }
    }
}
