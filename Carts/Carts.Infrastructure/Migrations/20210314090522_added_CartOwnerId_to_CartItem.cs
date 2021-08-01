using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Carts.API.Migrations
{
    public partial class added_CartOwnerId_to_CartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CartOwnerId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartOwnerId",
                table: "CartItems");
        }
    }
}
