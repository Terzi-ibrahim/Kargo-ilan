using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class columbremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_Role_id1",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_User_id1",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_Role_id1",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_User_id1",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "Role_id1",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "User_id1",
                table: "UserRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role_id1",
                table: "UserRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_id1",
                table: "UserRole",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Role_id1",
                table: "UserRole",
                column: "Role_id1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_User_id1",
                table: "UserRole",
                column: "User_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_Role_id1",
                table: "UserRole",
                column: "Role_id1",
                principalTable: "Role",
                principalColumn: "Role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_User_id1",
                table: "UserRole",
                column: "User_id1",
                principalTable: "User",
                principalColumn: "User_id");
        }
    }
}
