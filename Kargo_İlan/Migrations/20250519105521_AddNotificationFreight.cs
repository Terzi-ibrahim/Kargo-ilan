using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationFreight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreightId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_FreightId",
                table: "Notification",
                column: "FreightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Freight_FreightId",
                table: "Notification",
                column: "FreightId",
                principalTable: "Freight",
                principalColumn: "Freight_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Freight_FreightId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_FreightId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "FreightId",
                table: "Notification");
        }
    }
}
