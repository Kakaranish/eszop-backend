using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.API.Migrations
{
    public partial class Buyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryAddresses_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddressId",
                table: "Orders",
                newName: "Buyer_DeliveryAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                newName: "IX_Orders_Buyer_DeliveryAddressId");

            migrationBuilder.AddColumn<Guid>(
                name: "Buyer_Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryAddresses_Buyer_DeliveryAddressId",
                table: "Orders",
                column: "Buyer_DeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryAddresses_Buyer_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Buyer_Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Buyer_DeliveryAddressId",
                table: "Orders",
                newName: "DeliveryAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Buyer_DeliveryAddressId",
                table: "Orders",
                newName: "IX_Orders_DeliveryAddressId");

            migrationBuilder.AddColumn<Guid>(
                name: "BuyerId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryAddresses_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
