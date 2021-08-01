using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class SellerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("14fadc53-fccb-407b-8583-fef444b802ca"));

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AboutSellers",
                newName: "ContactEmail");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AboutSellers",
                newName: "BankAccountNumber");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "AboutSellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("2de9034d-8b49-4ce9-8175-61e2f12e8f84"), new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8187), "admin@mail.com", "rgD/9ec0VkDn2p62uyZ0GtBx2PNSzljck+QarzDYklw=|5AotysMDU9NsxEFX+H/EnQ==|10000", null, null, null, "admin", new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8540) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2de9034d-8b49-4ce9-8175-61e2f12e8f84"));

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "AboutSellers");

            migrationBuilder.RenameColumn(
                name: "ContactEmail",
                table: "AboutSellers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "AboutSellers",
                newName: "Description");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("14fadc53-fccb-407b-8583-fef444b802ca"), new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2162), "admin@mail.com", "ts3ERCFgDJeTsO9V/Nb1//83THfQBhxYb1YYi6THGLA=|JyCgNG0Qw4dru5Ii5ENRyQ==|10000", null, null, null, "admin", new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2502) });
        }
    }
}
