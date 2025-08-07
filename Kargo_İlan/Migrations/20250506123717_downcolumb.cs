using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class downcolumb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freight_Districts_District_id",
                table: "Freight");

            migrationBuilder.DropForeignKey(
                name: "FK_Freight_Districts_District_id1",
                table: "Freight");

            migrationBuilder.DropForeignKey(
                name: "FK_Freight_Province_Province_id",
                table: "Freight");

            migrationBuilder.DropForeignKey(
                name: "FK_Freight_Province_Province_id1",
                table: "Freight");

            migrationBuilder.DropIndex(
                name: "IX_Freight_District_id",
                table: "Freight");

            migrationBuilder.DropIndex(
                name: "IX_Freight_District_id1",
                table: "Freight");

            migrationBuilder.DropIndex(
                name: "IX_Freight_Province_id",
                table: "Freight");

            migrationBuilder.DropIndex(
                name: "IX_Freight_Province_id1",
                table: "Freight");

            migrationBuilder.DropColumn(
                name: "District_id",
                table: "Freight");

            migrationBuilder.DropColumn(
                name: "District_id1",
                table: "Freight");

            migrationBuilder.DropColumn(
                name: "Province_id",
                table: "Freight");

            migrationBuilder.DropColumn(
                name: "Province_id1",
                table: "Freight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "District_id",
                table: "Freight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "District_id1",
                table: "Freight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Province_id",
                table: "Freight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Province_id1",
                table: "Freight",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Freight_District_id",
                table: "Freight",
                column: "District_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_District_id1",
                table: "Freight",
                column: "District_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Province_id",
                table: "Freight",
                column: "Province_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Province_id1",
                table: "Freight",
                column: "Province_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Freight_Districts_District_id",
                table: "Freight",
                column: "District_id",
                principalTable: "Districts",
                principalColumn: "District_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Freight_Districts_District_id1",
                table: "Freight",
                column: "District_id1",
                principalTable: "Districts",
                principalColumn: "District_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Freight_Province_Province_id",
                table: "Freight",
                column: "Province_id",
                principalTable: "Province",
                principalColumn: "Province_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Freight_Province_Province_id1",
                table: "Freight",
                column: "Province_id1",
                principalTable: "Province",
                principalColumn: "Province_id");
        }
    }
}
