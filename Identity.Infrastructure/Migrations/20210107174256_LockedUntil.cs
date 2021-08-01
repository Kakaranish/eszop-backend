using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class LockedUntil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1d30c61-7587-4eac-8d7e-e1cda4f1ef76"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedUntil",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("87744ea0-e89b-40cf-bc65-fb0da81531de"), new DateTime(2021, 1, 7, 17, 42, 55, 794, DateTimeKind.Utc).AddTicks(9554), "admin@mail.com", "oP31oHB3jdMNuQm0QtwNIE8yDePgHAWHekDWELQLs74=|j/daXjaP7fPXBWWIqghl5g==|10000", null, null, null, "admin", new DateTime(2021, 1, 7, 17, 42, 55, 794, DateTimeKind.Utc).AddTicks(9927) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("87744ea0-e89b-40cf-bc65-fb0da81531de"));

            migrationBuilder.DropColumn(
                name: "LockedUntil",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("a1d30c61-7587-4eac-8d7e-e1cda4f1ef76"), new DateTime(2021, 1, 7, 17, 4, 47, 978, DateTimeKind.Utc).AddTicks(4574), "admin@mail.com", "hjlu4aGlyTi7hc20P+H8Do+fw8wnCsWHK2393TF0n2A=|TR/x1DDivtEQd6Lj73bFUQ==|10000", null, null, "admin", new DateTime(2021, 1, 7, 17, 4, 47, 978, DateTimeKind.Utc).AddTicks(4937) });
        }
    }
}
