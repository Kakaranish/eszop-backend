using Microsoft.EntityFrameworkCore.Migrations;

namespace NotificationService.Migrations
{
    public partial class Notification_improvement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Notifications",
                newName: "Metadata");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Metadata",
                table: "Notifications",
                newName: "Details");
        }
    }
}
