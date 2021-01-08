using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("561cf1dc-4f36-4f0b-b0ee-1d5ab30c0236"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("14fadc53-fccb-407b-8583-fef444b802ca"), new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2162), "admin@mail.com", "ts3ERCFgDJeTsO9V/Nb1//83THfQBhxYb1YYi6THGLA=|JyCgNG0Qw4dru5Ii5ENRyQ==|10000", null, null, null, "admin", new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2502) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimaryDeliveryAddressId",
                table: "Users",
                column: "PrimaryDeliveryAddressId",
                unique: true,
                filter: "[PrimaryDeliveryAddressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DeliveryAddresses_PrimaryDeliveryAddressId",
                table: "Users",
                column: "PrimaryDeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_DeliveryAddresses_PrimaryDeliveryAddressId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PrimaryDeliveryAddressId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("14fadc53-fccb-407b-8583-fef444b802ca"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("561cf1dc-4f36-4f0b-b0ee-1d5ab30c0236"), new DateTime(2021, 1, 8, 18, 56, 45, 425, DateTimeKind.Utc).AddTicks(6530), "admin@mail.com", "Qnr2wlFkDfe9gZgpSqp6wzaTIfGbeetb+mW3bH9fdlg=|FgYavu7NfO3mE89pvHvyPA==|10000", null, null, null, "admin", new DateTime(2021, 1, 8, 18, 56, 45, 425, DateTimeKind.Utc).AddTicks(6883) });
        }
    }
}
