using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class DeliveryAddressEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("87744ea0-e89b-40cf-bc65-fb0da81531de"));

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "DeliveryAddresses");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("561cf1dc-4f36-4f0b-b0ee-1d5ab30c0236"), new DateTime(2021, 1, 8, 18, 56, 45, 425, DateTimeKind.Utc).AddTicks(6530), "admin@mail.com", "Qnr2wlFkDfe9gZgpSqp6wzaTIfGbeetb+mW3bH9fdlg=|FgYavu7NfO3mE89pvHvyPA==|10000", null, null, null, "admin", new DateTime(2021, 1, 8, 18, 56, 45, 425, DateTimeKind.Utc).AddTicks(6883) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("561cf1dc-4f36-4f0b-b0ee-1d5ab30c0236"));

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "DeliveryAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("87744ea0-e89b-40cf-bc65-fb0da81531de"), new DateTime(2021, 1, 7, 17, 42, 55, 794, DateTimeKind.Utc).AddTicks(9554), "admin@mail.com", "oP31oHB3jdMNuQm0QtwNIE8yDePgHAWHekDWELQLs74=|j/daXjaP7fPXBWWIqghl5g==|10000", null, null, null, "admin", new DateTime(2021, 1, 7, 17, 42, 55, 794, DateTimeKind.Utc).AddTicks(9927) });
        }
    }
}
