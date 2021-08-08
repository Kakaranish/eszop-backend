using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.API.Migrations
{
    public partial class OfferDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "PricePerItem",
                table: "OrderItems",
                newName: "OfferDetails_PricePerItem");

            migrationBuilder.RenameColumn(
                name: "ImageUri",
                table: "OrderItems",
                newName: "OfferDetails_ImageUri");

            migrationBuilder.RenameColumn(
                name: "OfferName",
                table: "OrderItems",
                newName: "OfferDetails_Name");

            migrationBuilder.AlterColumn<string>(
                name: "OrderState",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "STARTED",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "started");

            migrationBuilder.AlterColumn<decimal>(
                name: "OfferDetails_PricePerItem",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<Guid>(
                name: "OfferDetails_Id",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferDetails_Id",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "OfferDetails_PricePerItem",
                table: "OrderItems",
                newName: "PricePerItem");

            migrationBuilder.RenameColumn(
                name: "OfferDetails_ImageUri",
                table: "OrderItems",
                newName: "ImageUri");

            migrationBuilder.RenameColumn(
                name: "OfferDetails_Name",
                table: "OrderItems",
                newName: "OfferName");

            migrationBuilder.AlterColumn<string>(
                name: "OrderState",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "started",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "STARTED");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerItem",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
