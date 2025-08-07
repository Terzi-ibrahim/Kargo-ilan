using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class AddTableNotificationsPrefer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserNotificationPrefs",
                columns: table => new
                {
                    UserNotificationPrefId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Yuklemeil_id = table.Column<int>(type: "int", nullable: false),
                    Yuklemeilce_id = table.Column<int>(type: "int", nullable: false),
                    Varisil_id = table.Column<int>(type: "int", nullable: false),
                    Varisilce_id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationPrefs", x => x.UserNotificationPrefId);
                    table.ForeignKey(
                        name: "FK_UserNotificationPrefs_Districts_Varisilce_id",
                        column: x => x.Varisilce_id,
                        principalTable: "Districts",
                        principalColumn: "District_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotificationPrefs_Districts_Yuklemeilce_id",
                        column: x => x.Yuklemeilce_id,
                        principalTable: "Districts",
                        principalColumn: "District_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotificationPrefs_Province_Varisil_id",
                        column: x => x.Varisil_id,
                        principalTable: "Province",
                        principalColumn: "Province_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotificationPrefs_Province_Yuklemeil_id",
                        column: x => x.Yuklemeil_id,
                        principalTable: "Province",
                        principalColumn: "Province_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotificationPrefs_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationPrefs_User_id",
                table: "UserNotificationPrefs",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationPrefs_Varisil_id",
                table: "UserNotificationPrefs",
                column: "Varisil_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationPrefs_Varisilce_id",
                table: "UserNotificationPrefs",
                column: "Varisilce_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationPrefs_Yuklemeil_id",
                table: "UserNotificationPrefs",
                column: "Yuklemeil_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationPrefs_Yuklemeilce_id",
                table: "UserNotificationPrefs",
                column: "Yuklemeilce_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotificationPrefs");
        }
    }
}
