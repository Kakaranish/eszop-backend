using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class profile_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("585eaf42-7dba-4252-91cf-c6d3dbf4a354"));

            migrationBuilder.CreateTable(
                name: "ProfileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "Role", "UpdatedAt" },
                values: new object[] { new Guid("0b7e7262-e06a-436d-9049-6b7655d8e91d"), new DateTime(2021, 1, 6, 16, 57, 55, 124, DateTimeKind.Utc).AddTicks(8285), "admin@mail.com", "90aFcZq9I2zPmRXnQzWrItNtyzwmd7QPCRMQLmsfX1o=|LVdpW3tdc4GMZETuVTr4ug==|10000", "admin", new DateTime(2021, 1, 6, 16, 57, 55, 124, DateTimeKind.Utc).AddTicks(8638) });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfos_UserId",
                table: "ProfileInfos",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileInfos");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0b7e7262-e06a-436d-9049-6b7655d8e91d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "Role", "UpdatedAt" },
                values: new object[] { new Guid("585eaf42-7dba-4252-91cf-c6d3dbf4a354"), new DateTime(2021, 1, 2, 18, 32, 23, 590, DateTimeKind.Utc).AddTicks(5798), "admin@mail.com", "asoFDxusJZGY35vLYhxxUoD46SHvflMNR/DkrnETXk0=|b4aFh7qC2/x51e96wQhqZA==|10000", "admin", new DateTime(2021, 1, 2, 18, 32, 23, 590, DateTimeKind.Utc).AddTicks(6137) });
        }
    }
}
