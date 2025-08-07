using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferStatusStatusId",
                table: "Offer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OfferStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferStatus", x => x.StatusId);
                });

            migrationBuilder.InsertData(
                table: "OfferStatus",
                columns: new[] { "StatusId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Teklif beklemede", "Pending" },
                    { 2, "Teklif onaylandı", "Confirmed" },
                    { 3, "Teklif reddedildi", "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_OfferStatusStatusId",
                table: "Offer",
                column: "OfferStatusStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_OfferStatus_OfferStatusStatusId",
                table: "Offer",
                column: "OfferStatusStatusId",
                principalTable: "OfferStatus",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_OfferStatus_OfferStatusStatusId",
                table: "Offer");

            migrationBuilder.DropTable(
                name: "OfferStatus");

            migrationBuilder.DropIndex(
                name: "IX_Offer_OfferStatusStatusId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "OfferStatusStatusId",
                table: "Offer");
        }
    }
}
