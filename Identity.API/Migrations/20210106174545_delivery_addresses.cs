using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class delivery_addresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0b7e7262-e06a-436d-9049-6b7655d8e91d"));

            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryDeliveryAddressId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("22b975b1-aecd-4150-8a01-f05a40aced85"), new DateTime(2021, 1, 6, 17, 45, 45, 313, DateTimeKind.Utc).AddTicks(1257), "admin@mail.com", "oxM890aSm0FACmqDX7f5e08IDSdOhHp8l71c8D5LEts=|3XparpdfIFVW9Z29sCqqHg==|10000", null, "admin", new DateTime(2021, 1, 6, 17, 45, 45, 313, DateTimeKind.Utc).AddTicks(1591) });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_UserId",
                table: "DeliveryAddresses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22b975b1-aecd-4150-8a01-f05a40aced85"));

            migrationBuilder.DropColumn(
                name: "PrimaryDeliveryAddressId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "Role", "UpdatedAt" },
                values: new object[] { new Guid("0b7e7262-e06a-436d-9049-6b7655d8e91d"), new DateTime(2021, 1, 6, 16, 57, 55, 124, DateTimeKind.Utc).AddTicks(8285), "admin@mail.com", "90aFcZq9I2zPmRXnQzWrItNtyzwmd7QPCRMQLmsfX1o=|LVdpW3tdc4GMZETuVTr4ug==|10000", "admin", new DateTime(2021, 1, 6, 16, 57, 55, 124, DateTimeKind.Utc).AddTicks(8638) });
        }
    }
}
