using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationPrefRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserNotificationPrefId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserNotificationPrefId",
                table: "Notification",
                column: "UserNotificationPrefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_UserNotificationPrefs_UserNotificationPrefId",
                table: "Notification",
                column: "UserNotificationPrefId",
                principalTable: "UserNotificationPrefs",
                principalColumn: "UserNotificationPrefId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_UserNotificationPrefs_UserNotificationPrefId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserNotificationPrefId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserNotificationPrefId",
                table: "Notification");
        }
    }
}
