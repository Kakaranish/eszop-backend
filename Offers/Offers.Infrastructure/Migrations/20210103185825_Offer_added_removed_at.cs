using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Offers.API.Migrations
{
    public partial class Offer_added_removed_at : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "Offers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "Offers");
        }
    }
}
