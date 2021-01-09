using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class SellerInfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutSellers_Users_UserId",
                table: "AboutSellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutSellers",
                table: "AboutSellers");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2de9034d-8b49-4ce9-8175-61e2f12e8f84"));

            migrationBuilder.RenameTable(
                name: "AboutSellers",
                newName: "SellerInfos");

            migrationBuilder.RenameIndex(
                name: "IX_AboutSellers_UserId",
                table: "SellerInfos",
                newName: "IX_SellerInfos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerInfos",
                table: "SellerInfos",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("953e922d-b4bd-4e09-9e67-43beebd59f36"), new DateTime(2021, 1, 9, 18, 38, 55, 331, DateTimeKind.Utc).AddTicks(6511), "admin@mail.com", "xZisokErkTKH5Fli0ZkGCU5ZhHJAxawKL0xgKtpRkzQ=|GzyUZ+AsKvOAI80ZLz6GRg==|10000", null, null, null, "admin", new DateTime(2021, 1, 9, 18, 38, 55, 331, DateTimeKind.Utc).AddTicks(6854) });

            migrationBuilder.AddForeignKey(
                name: "FK_SellerInfos_Users_UserId",
                table: "SellerInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerInfos_Users_UserId",
                table: "SellerInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerInfos",
                table: "SellerInfos");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("953e922d-b4bd-4e09-9e67-43beebd59f36"));

            migrationBuilder.RenameTable(
                name: "SellerInfos",
                newName: "AboutSellers");

            migrationBuilder.RenameIndex(
                name: "IX_SellerInfos_UserId",
                table: "AboutSellers",
                newName: "IX_AboutSellers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutSellers",
                table: "AboutSellers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("2de9034d-8b49-4ce9-8175-61e2f12e8f84"), new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8187), "admin@mail.com", "rgD/9ec0VkDn2p62uyZ0GtBx2PNSzljck+QarzDYklw=|5AotysMDU9NsxEFX+H/EnQ==|10000", null, null, null, "admin", new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8540) });

            migrationBuilder.AddForeignKey(
                name: "FK_AboutSellers_Users_UserId",
                table: "AboutSellers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
