using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class about_seller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22b975b1-aecd-4150-8a01-f05a40aced85"));

            migrationBuilder.CreateTable(
                name: "AboutSellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutSellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutSellers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("fd8d44ed-4722-4296-abdd-d232f1c8ce5c"), new DateTime(2021, 1, 6, 18, 14, 48, 591, DateTimeKind.Utc).AddTicks(9741), "admin@mail.com", "SIKPcCy3JY99Uv4Wl+re/+4i3Iz65TzdMNZFqVW6rNg=|iTrV4OtGqGUo3VG0Flm1CA==|10000", null, "admin", new DateTime(2021, 1, 6, 18, 14, 48, 592, DateTimeKind.Utc).AddTicks(85) });

            migrationBuilder.CreateIndex(
                name: "IX_AboutSellers_UserId",
                table: "AboutSellers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutSellers");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fd8d44ed-4722-4296-abdd-d232f1c8ce5c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "PrimaryDeliveryAddressId", "Role", "UpdatedAt" },
                values: new object[] { new Guid("22b975b1-aecd-4150-8a01-f05a40aced85"), new DateTime(2021, 1, 6, 17, 45, 45, 313, DateTimeKind.Utc).AddTicks(1257), "admin@mail.com", "oxM890aSm0FACmqDX7f5e08IDSdOhHp8l71c8D5LEts=|3XparpdfIFVW9Z29sCqqHg==|10000", null, "admin", new DateTime(2021, 1, 6, 17, 45, 45, 313, DateTimeKind.Utc).AddTicks(1591) });
        }
    }
}
