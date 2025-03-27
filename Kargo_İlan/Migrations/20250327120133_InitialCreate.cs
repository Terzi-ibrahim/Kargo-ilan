using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoTypes",
                columns: table => new
                {
                    CargoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KargoAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Kargo tipinin adını belirtir")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoTipleri", x => x.CargoId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Kategori adını belirtir")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enlem = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Boylam = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    UlkeAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ulkeler", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enlem = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Boylam = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    IlceAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.DistrictId);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enlem = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Boylam = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    IlAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Rolün adını belirtir")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Araç tipinin adını belirtir")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracTipleri", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Roller",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    ListingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Agirlik = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ResimYolu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CargoId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    YuklemeIlId = table.Column<int>(type: "int", nullable: false),
                    YuklemeIlceId = table.Column<int>(type: "int", nullable: false),
                    YuklemeUlkeId = table.Column<int>(type: "int", nullable: false),
                    VarisIlId = table.Column<int>(type: "int", nullable: false),
                    VarisIlceId = table.Column<int>(type: "int", nullable: false),
                    VarisUlkeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.ListingId);
                    table.ForeignKey(
                        name: "FK_Ilanlar_AracTipi",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_KargoTipi",
                        column: x => x.CargoId,
                        principalTable: "CargoTypes",
                        principalColumn: "CargoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_Kategori",
                        column: x => x.CategoryId,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_Kullanicilar",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ilanlar_VarisIlcesi",
                        column: x => x.VarisIlceId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_VarisIli",
                        column: x => x.VarisIlId,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_VarisUlkesi",
                        column: x => x.VarisUlkeId,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_YuklemeIlcesi",
                        column: x => x.YuklemeIlceId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_YuklemeIli",
                        column: x => x.YuklemeIlId,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_YuklemeUlkesi",
                        column: x => x.YuklemeUlkeId,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listing_AracTipi",
                table: "Listings",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_Kullanici",
                table: "Listings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_OlusturulmaTarihi",
                table: "Listings",
                column: "OlusturulmaTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_Rota",
                table: "Listings",
                columns: new[] { "YuklemeIlId", "VarisIlId" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CargoId",
                table: "Listings",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CategoryId",
                table: "Listings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_VarisIlceId",
                table: "Listings",
                column: "VarisIlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_VarisIlId",
                table: "Listings",
                column: "VarisIlId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_VarisUlkeId",
                table: "Listings",
                column: "VarisUlkeId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_YuklemeIlceId",
                table: "Listings",
                column: "YuklemeIlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_YuklemeUlkeId",
                table: "Listings",
                column: "YuklemeUlkeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolID",
                table: "Users",
                column: "RolID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "CargoTypes");

            migrationBuilder.DropTable(
                name: "CategoryTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
