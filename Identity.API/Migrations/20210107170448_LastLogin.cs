using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class LastLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fd8d44ed-4722-4296-abdd-d232f1c8ce5c"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("a1d30c61-7587-4eac-8d7e-e1cda4f1ef76"), new DateTime(2021, 1, 7, 17, 4, 47, 978, DateTimeKind.Utc).AddTicks(4574), "admin@mail.com", "hjlu4aGlyTi7hc20P+H8Do+fw8wnCsWHK2393TF0n2A=|TR/x1DDivtEQd6Lj73bFUQ==|10000", null, null, "admin", new DateTime(2021, 1, 7, 17, 4, 47, 978, DateTimeKind.Utc).AddTicks(4937) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1d30c61-7587-4eac-8d7e-e1cda4f1ef76"));

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("fd8d44ed-4722-4296-abdd-d232f1c8ce5c"), new DateTime(2021, 1, 6, 18, 14, 48, 591, DateTimeKind.Utc).AddTicks(9741), "admin@mail.com", "SIKPcCy3JY99Uv4Wl+re/+4i3Iz65TzdMNZFqVW6rNg=|iTrV4OtGqGUo3VG0Flm1CA==|10000", null, "admin", new DateTime(2021, 1, 6, 18, 14, 48, 592, DateTimeKind.Utc).AddTicks(85) });
        }
    }
}
