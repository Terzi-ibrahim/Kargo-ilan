using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class Configretions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListingId1",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListingId1",
                table: "Offers",
                column: "ListingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Listings_ListingId1",
                table: "Offers",
                column: "ListingId1",
                principalTable: "Listings",
                principalColumn: "ListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Listings_ListingId1",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ListingId1",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ListingId1",
                table: "Offers");
        }
    }
}
