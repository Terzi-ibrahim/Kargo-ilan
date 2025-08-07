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
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Offers_OfferId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Listings_ListingId1",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "RoleUsers");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CommonUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AracTipleri",
                table: "VehicleTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ListingId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ListingId1",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ListingId1",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IlanDurumu",
                table: "ListingStatuses");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "VehicleType");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Provinces",
                newName: "Province");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "ListingStatuses",
                newName: "ListingStatus");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.RenameTable(
                name: "CategoryTypes",
                newName: "CategoryType");

            migrationBuilder.RenameTable(
                name: "CargoTypes",
                newName: "CargoType");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Districts",
                newName: "Province_id");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Districts",
                newName: "District_id");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                newName: "IX_Districts_Province_id");

            migrationBuilder.RenameColumn(
                name: "AracAdi",
                table: "VehicleType",
                newName: "VehicleName");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "VehicleType",
                newName: "VehicleType_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Role",
                newName: "Role_id");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Province",
                newName: "Province_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Offer",
                newName: "User_id");

            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "Offer",
                newName: "Offer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_UserId",
                table: "Offer",
                newName: "IX_Offer_User_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "User_id");

            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "Notification",
                newName: "Offer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Notification",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "Notification",
                newName: "Notification_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_User_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_OfferId",
                table: "Notification",
                newName: "IX_Notification_Offer_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ListingStatus",
                newName: "ListingStatus_id");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "Contact",
                newName: "Contact_id");

            migrationBuilder.RenameColumn(
                name: "KategoriAdi",
                table: "CategoryType",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryType",
                newName: "Category_id");

            migrationBuilder.RenameColumn(
                name: "KargoAdi",
                table: "CargoType",
                newName: "CargoName");

            migrationBuilder.RenameColumn(
                name: "CargoId",
                table: "CargoType",
                newName: "Cargo_id");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleName",
                table: "VehicleType",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Araç tipinin adını belirtir");

            migrationBuilder.AlterColumn<int>(
                name: "ValidityInDays",
                table: "Offer",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 3);

            migrationBuilder.AlterColumn<bool>(
                name: "IsValid",
                table: "Offer",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsConfirmed",
                table: "Offer",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Offer",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Freight_id",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Offer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ListingStatus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "CategoryType",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Kategori adını belirtir");

            migrationBuilder.AlterColumn<string>(
                name: "CargoName",
                table: "CargoType",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Kargo tipinin adını belirtir");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "VehicleType_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Offer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Notification_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "Contact_id");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyAdress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Company_id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Person_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Person_id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSetting",
                columns: table => new
                {
                    SystemSetting_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSetting", x => x.SystemSetting_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Person_id = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.User_id);
                    table.ForeignKey(
                        name: "FK_User_Person_Person_id",
                        column: x => x.Person_id,
                        principalTable: "Person",
                        principalColumn: "Person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Freight",
                columns: table => new
                {
                    Freight_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    ListingStatus_id = table.Column<int>(type: "int", nullable: false),
                    Cargo_id = table.Column<int>(type: "int", nullable: false),
                    Category_id = table.Column<int>(type: "int", nullable: false),
                    VehicleType_id = table.Column<int>(type: "int", nullable: false),
                    Yuklemeil_id = table.Column<int>(type: "int", nullable: false),
                    Yuklemeilce_id = table.Column<int>(type: "int", nullable: false),
                    Varisil_id = table.Column<int>(type: "int", nullable: false),
                    Varisilce_id = table.Column<int>(type: "int", nullable: false),
                    District_id = table.Column<int>(type: "int", nullable: true),
                    District_id1 = table.Column<int>(type: "int", nullable: true),
                    Province_id = table.Column<int>(type: "int", nullable: true),
                    Province_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Freight", x => x.Freight_id);
                    table.ForeignKey(
                        name: "FK_Freight_Districts_District_id",
                        column: x => x.District_id,
                        principalTable: "Districts",
                        principalColumn: "District_id");
                    table.ForeignKey(
                        name: "FK_Freight_Districts_District_id1",
                        column: x => x.District_id1,
                        principalTable: "Districts",
                        principalColumn: "District_id");
                    table.ForeignKey(
                        name: "FK_Freight_Province_Province_id",
                        column: x => x.Province_id,
                        principalTable: "Province",
                        principalColumn: "Province_id");
                    table.ForeignKey(
                        name: "FK_Freight_Province_Province_id1",
                        column: x => x.Province_id1,
                        principalTable: "Province",
                        principalColumn: "Province_id");
                    table.ForeignKey(
                        name: "FK_Freight_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Freight_VehicleType_VehicleType_id",
                        column: x => x.VehicleType_id,
                        principalTable: "VehicleType",
                        principalColumn: "VehicleType_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_IlanDurumu",
                        column: x => x.ListingStatus_id,
                        principalTable: "ListingStatus",
                        principalColumn: "ListingStatus_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_KargoTipi",
                        column: x => x.Cargo_id,
                        principalTable: "CargoType",
                        principalColumn: "Cargo_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_Kategori",
                        column: x => x.Category_id,
                        principalTable: "CategoryType",
                        principalColumn: "Category_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_VarisIlcesi",
                        column: x => x.Varisilce_id,
                        principalTable: "Districts",
                        principalColumn: "District_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_VarisIli",
                        column: x => x.Varisil_id,
                        principalTable: "Province",
                        principalColumn: "Province_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_YuklemeIlcesi",
                        column: x => x.Yuklemeilce_id,
                        principalTable: "Districts",
                        principalColumn: "District_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ilanlar_YuklemeIli",
                        column: x => x.Yuklemeil_id,
                        principalTable: "Province",
                        principalColumn: "Province_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCompany",
                columns: table => new
                {
                    UserCompany_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Company_id = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompany", x => x.UserCompany_id);
                    table.ForeignKey(
                        name: "FK_UserCompany_Companies_Company_id",
                        column: x => x.Company_id,
                        principalTable: "Companies",
                        principalColumn: "Company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCompany_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Role_id = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Role_id1 = table.Column<int>(type: "int", nullable: true),
                    User_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.User_id, x.Role_id });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_Role_id",
                        column: x => x.Role_id,
                        principalTable: "Role",
                        principalColumn: "Role_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_Role_id1",
                        column: x => x.Role_id1,
                        principalTable: "Role",
                        principalColumn: "Role_id");
                    table.ForeignKey(
                        name: "FK_UserRole_User_User_id",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_User_id1",
                        column: x => x.User_id1,
                        principalTable: "User",
                        principalColumn: "User_id");
                });

            migrationBuilder.UpdateData(
                table: "ListingStatus",
                keyColumn: "ListingStatus_id",
                keyValue: 10,
                column: "Status",
                value: "Aktif");

            migrationBuilder.UpdateData(
                table: "ListingStatus",
                keyColumn: "ListingStatus_id",
                keyValue: 20,
                column: "Status",
                value: "Beklemede");

            migrationBuilder.UpdateData(
                table: "ListingStatus",
                keyColumn: "ListingStatus_id",
                keyValue: 30,
                column: "Status",
                value: "Tamamlandı");

            migrationBuilder.UpdateData(
                table: "ListingStatus",
                keyColumn: "ListingStatus_id",
                keyValue: 40,
                column: "Status",
                value: "İptal Edildi");

            migrationBuilder.CreateIndex(
                name: "UK_Role_RoleName",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Freight_id",
                table: "Offer",
                column: "Freight_id");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Email",
                table: "Contact",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "UK_Company_Email",
                table: "Companies",
                column: "CompanyEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Company_TaxNumber",
                table: "Companies",
                column: "TaxNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Cargo_id",
                table: "Freight",
                column: "Cargo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Category_id",
                table: "Freight",
                column: "Category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_District_id",
                table: "Freight",
                column: "District_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_District_id1",
                table: "Freight",
                column: "District_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_ListingStatus_id",
                table: "Freight",
                column: "ListingStatus_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Province_id",
                table: "Freight",
                column: "Province_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Province_id1",
                table: "Freight",
                column: "Province_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_User_id",
                table: "Freight",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Varisil_id",
                table: "Freight",
                column: "Varisil_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Varisilce_id",
                table: "Freight",
                column: "Varisilce_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_VehicleType_id",
                table: "Freight",
                column: "VehicleType_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Yuklemeil_id",
                table: "Freight",
                column: "Yuklemeil_id");

            migrationBuilder.CreateIndex(
                name: "IX_Freight_Yuklemeilce_id",
                table: "Freight",
                column: "Yuklemeilce_id");

            migrationBuilder.CreateIndex(
                name: "UK_Freight_id",
                table: "Freight",
                column: "Freight_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Person_Email",
                table: "Person",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Person_PhoneNumber",
                table: "Person",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Person_id",
                table: "User",
                column: "Person_id");

            migrationBuilder.CreateIndex(
                name: "UK_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompany_Company_id",
                table: "UserCompany",
                column: "Company_id");

            migrationBuilder.CreateIndex(
                name: "UK_UserCompany_User_Company",
                table: "UserCompany",
                columns: new[] { "User_id", "Company_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Role_id",
                table: "UserRole",
                column: "Role_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Role_id1",
                table: "UserRole",
                column: "Role_id1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_User_id1",
                table: "UserRole",
                column: "User_id1");

            migrationBuilder.CreateIndex(
                name: "UK_UserRole_User_Role",
                table: "UserRole",
                columns: new[] { "User_id", "Role_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Offer_Offer_id",
                table: "Notification",
                column: "Offer_id",
                principalTable: "Offer",
                principalColumn: "Offer_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_User_id",
                table: "Notification",
                column: "User_id",
                principalTable: "User",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Freight_Freight_id",
                table: "Offer",
                column: "Freight_id",
                principalTable: "Freight",
                principalColumn: "Freight_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_User_User_id",
                table: "Offer",
                column: "User_id",
                principalTable: "User",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Offer_Offer_id",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_User_id",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Freight_Freight_id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_User_User_id",
                table: "Offer");

            migrationBuilder.DropTable(
                name: "Freight");

            migrationBuilder.DropTable(
                name: "SystemSetting");

            migrationBuilder.DropTable(
                name: "UserCompany");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.DropIndex(
                name: "UK_Role_RoleName",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_Freight_id",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_Email",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Freight_id",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ListingStatus");

            migrationBuilder.RenameTable(
                name: "VehicleType",
                newName: "VehicleTypes");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Province",
                newName: "Provinces");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "ListingStatus",
                newName: "ListingStatuses");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.RenameTable(
                name: "CategoryType",
                newName: "CategoryTypes");

            migrationBuilder.RenameTable(
                name: "CargoType",
                newName: "CargoTypes");

            migrationBuilder.RenameColumn(
                name: "Province_id",
                table: "Districts",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "District_id",
                table: "Districts",
                newName: "DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_Province_id",
                table: "Districts",
                newName: "IX_Districts_ProvinceId");

            migrationBuilder.RenameColumn(
                name: "VehicleName",
                table: "VehicleTypes",
                newName: "AracAdi");

            migrationBuilder.RenameColumn(
                name: "VehicleType_id",
                table: "VehicleTypes",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "Role_id",
                table: "Roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Province_id",
                table: "Provinces",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "Offers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Offer_id",
                table: "Offers",
                newName: "OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_User_id",
                table: "Offers",
                newName: "IX_Offers_UserId");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Offer_id",
                table: "Notifications",
                newName: "OfferId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notifications",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "Notification_id",
                table: "Notifications",
                newName: "NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_User_id",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_Offer_id",
                table: "Notifications",
                newName: "IX_Notifications_OfferId");

            migrationBuilder.RenameColumn(
                name: "ListingStatus_id",
                table: "ListingStatuses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Contact_id",
                table: "Contacts",
                newName: "ContactId");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "CategoryTypes",
                newName: "KategoriAdi");

            migrationBuilder.RenameColumn(
                name: "Category_id",
                table: "CategoryTypes",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "CargoName",
                table: "CargoTypes",
                newName: "KargoAdi");

            migrationBuilder.RenameColumn(
                name: "Cargo_id",
                table: "CargoTypes",
                newName: "CargoId");

            migrationBuilder.AlterColumn<string>(
                name: "AracAdi",
                table: "VehicleTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Araç tipinin adını belirtir",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ValidityInDays",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsValid",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsConfirmed",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListingId1",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IlanDurumu",
                table: "ListingStatuses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "İlanın durumunu belirtir");

            migrationBuilder.AlterColumn<string>(
                name: "KategoriAdi",
                table: "CategoryTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Kategori adını belirtir",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "KargoAdi",
                table: "CargoTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Kargo tipinin adını belirtir",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AracTipleri",
                table: "VehicleTypes",
                column: "VehicleTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "ContactId");

            migrationBuilder.CreateTable(
                name: "CommonUsers",
                columns: table => new
                {
                    CommonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Common", x => x.CommonId);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Common",
                        column: x => x.CommonUserId,
                        principalTable: "CommonUsers",
                        principalColumn: "CommonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    ListingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CargoId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ListingStatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VarisIlceId = table.Column<int>(type: "int", nullable: false),
                    VarisIlId = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    YuklemeIlceId = table.Column<int>(type: "int", nullable: false),
                    YuklemeIlId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                        name: "FK_Ilanlar_IlanDurumu",
                        column: x => x.ListingStatusId,
                        principalTable: "ListingStatuses",
                        principalColumn: "Id",
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
                });

            migrationBuilder.CreateTable(
                name: "RoleUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ListingStatuses",
                keyColumn: "Id",
                keyValue: 10,
                column: "IlanDurumu",
                value: "Aktif");

            migrationBuilder.UpdateData(
                table: "ListingStatuses",
                keyColumn: "Id",
                keyValue: 20,
                column: "IlanDurumu",
                value: "Beklemede");

            migrationBuilder.UpdateData(
                table: "ListingStatuses",
                keyColumn: "Id",
                keyValue: 30,
                column: "IlanDurumu",
                value: "Tamamlandı");

            migrationBuilder.UpdateData(
                table: "ListingStatuses",
                keyColumn: "Id",
                keyValue: 40,
                column: "IlanDurumu",
                value: "İptal Edildi");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListingId",
                table: "Offers",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListingId1",
                table: "Offers",
                column: "ListingId1");

            migrationBuilder.CreateIndex(
                name: "UK_Common_Email",
                table: "CommonUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Common_FullName",
                table: "CommonUsers",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Common_PhoneNumber",
                table: "CommonUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Common_TaxNumber",
                table: "CommonUsers",
                column: "TaxNumber",
                unique: true,
                filter: "[TaxNumber] IS NOT NULL");

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
                name: "IX_Listings_ListingStatusId",
                table: "Listings",
                column: "ListingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_VarisIlceId",
                table: "Listings",
                column: "VarisIlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_VarisIlId",
                table: "Listings",
                column: "VarisIlId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_YuklemeIlceId",
                table: "Listings",
                column: "YuklemeIlceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_RoleId",
                table: "RoleUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommonUserId",
                table: "Users",
                column: "CommonUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Offers_OfferId",
                table: "Notifications",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "OfferId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Listings_ListingId",
                table: "Offers",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Listings_ListingId1",
                table: "Offers",
                column: "ListingId1",
                principalTable: "Listings",
                principalColumn: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
