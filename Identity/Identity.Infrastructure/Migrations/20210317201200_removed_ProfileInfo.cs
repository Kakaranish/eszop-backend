using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class removed_ProfileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileInfos");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("45166220-76f5-4532-ba10-f929aa12241e"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeliveryAddresses", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("d73f0de6-fc0c-4eab-a543-69b233968afc"), new DateTime(2021, 3, 17, 20, 11, 59, 616, DateTimeKind.Utc).AddTicks(4884), "[]", "sa@mail.com", "8Kn8a4+qd9TKfbrwxPL10h0EXdtiOcwtSopkA2Vhjrc=|NRvKwbF0SEQqFX6kgr7QqA==|10000", null, null, null, "SUPER_ADMIN", new DateTime(2021, 3, 17, 20, 11, 59, 616, DateTimeKind.Utc).AddTicks(5264) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d73f0de6-fc0c-4eab-a543-69b233968afc"));

            migrationBuilder.CreateTable(
                name: "ProfileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeliveryAddresses", "Email", "HashedPassword", "LastLogin", "LockedUntil", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("45166220-76f5-4532-ba10-f929aa12241e"), new DateTime(2021, 2, 23, 18, 47, 35, 838, DateTimeKind.Utc).AddTicks(7555), "[]", "admin@mail.com", "2D927JPbgRULEiYNm5eotrqjd3kWLF8WajuZXlrC25Y=|JMe1Spt20euEmyJsweRGEQ==|10000", null, null, null, "admin", new DateTime(2021, 2, 23, 18, 47, 35, 838, DateTimeKind.Utc).AddTicks(7912) });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfos_UserId",
                table: "ProfileInfos",
                column: "UserId",
                unique: true);
        }
    }
}
