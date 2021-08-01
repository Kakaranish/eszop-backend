using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class removed_delivery_address_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_DeliveryAddresses_PrimaryDeliveryAddressId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Users_PrimaryDeliveryAddressId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("953e922d-b4bd-4e09-9e67-43beebd59f36"));

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddresses",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeliveryAddresses", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("45166220-76f5-4532-ba10-f929aa12241e"), new DateTime(2021, 2, 23, 18, 47, 35, 838, DateTimeKind.Utc).AddTicks(7555), "[]", "admin@mail.com", "2D927JPbgRULEiYNm5eotrqjd3kWLF8WajuZXlrC25Y=|JMe1Spt20euEmyJsweRGEQ==|10000", null, null, null, "admin", new DateTime(2021, 2, 23, 18, 47, 35, 838, DateTimeKind.Utc).AddTicks(7912) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("45166220-76f5-4532-ba10-f929aa12241e"));

            migrationBuilder.DropColumn(
                name: "DeliveryAddresses",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("953e922d-b4bd-4e09-9e67-43beebd59f36"), new DateTime(2021, 1, 9, 18, 38, 55, 331, DateTimeKind.Utc).AddTicks(6511), "admin@mail.com", "xZisokErkTKH5Fli0ZkGCU5ZhHJAxawKL0xgKtpRkzQ=|GzyUZ+AsKvOAI80ZLz6GRg==|10000", null, null, null, "admin", new DateTime(2021, 1, 9, 18, 38, 55, 331, DateTimeKind.Utc).AddTicks(6854) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimaryDeliveryAddressId",
                table: "Users",
                column: "PrimaryDeliveryAddressId",
                unique: true,
                filter: "[PrimaryDeliveryAddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_UserId",
                table: "DeliveryAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DeliveryAddresses_PrimaryDeliveryAddressId",
                table: "Users",
                column: "PrimaryDeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
