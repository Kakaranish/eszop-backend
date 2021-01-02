using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.API.Migrations
{
    public partial class added_init_admin_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "HashedPassword", "Role", "UpdatedAt" },
                values: new object[] { new Guid("585eaf42-7dba-4252-91cf-c6d3dbf4a354"), new DateTime(2021, 1, 2, 18, 32, 23, 590, DateTimeKind.Utc).AddTicks(5798), "admin@mail.com", "asoFDxusJZGY35vLYhxxUoD46SHvflMNR/DkrnETXk0=|b4aFh7qC2/x51e96wQhqZA==|10000", "admin", new DateTime(2021, 1, 2, 18, 32, 23, 590, DateTimeKind.Utc).AddTicks(6137) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("585eaf42-7dba-4252-91cf-c6d3dbf4a354"));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
