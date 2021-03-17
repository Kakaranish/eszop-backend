using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class changed_User_lock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d73f0de6-fc0c-4eab-a543-69b233968afc"));

            migrationBuilder.DropColumn(
                name: "LockedUntil",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeliveryAddresses", "Email", "HashedPassword", "IsLocked", "LastLogin", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("cf1de650-a91f-492d-b6fb-abc36a9f9027"), new DateTime(2021, 3, 17, 21, 7, 38, 241, DateTimeKind.Utc).AddTicks(2474), "[]", "sa@mail.com", "bvgVqhvn0be2OKNEVkFQWWUDlg0jc18CbFdrgvW3nDM=|r+vbHPz67apcmp07qlJAaA==|10000", false, null, null, "SUPER_ADMIN", new DateTime(2021, 3, 17, 21, 7, 38, 241, DateTimeKind.Utc).AddTicks(2919) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cf1de650-a91f-492d-b6fb-abc36a9f9027"));

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedUntil",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeliveryAddresses", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("d73f0de6-fc0c-4eab-a543-69b233968afc"), new DateTime(2021, 3, 17, 20, 11, 59, 616, DateTimeKind.Utc).AddTicks(4884), "[]", "sa@mail.com", "8Kn8a4+qd9TKfbrwxPL10h0EXdtiOcwtSopkA2Vhjrc=|NRvKwbF0SEQqFX6kgr7QqA==|10000", null, null, null, "SUPER_ADMIN", new DateTime(2021, 3, 17, 20, 11, 59, 616, DateTimeKind.Utc).AddTicks(5264) });
        }
    }
}
