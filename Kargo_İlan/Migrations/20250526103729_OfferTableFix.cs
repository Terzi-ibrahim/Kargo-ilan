using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class OfferTableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Freight_Freight_id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_User_User_id",
                table: "Offer");

            migrationBuilder.DropTable(
                name: "SystemSetting");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "ValidityInDays",
                table: "Offer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Offer",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Offer",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmDate",
                table: "Offer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Freight_Freight_id",
                table: "Offer",
                column: "Freight_id",
                principalTable: "Freight",
                principalColumn: "Freight_id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Offer_Freight_Freight_id",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_User_User_id",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "ConfirmDate",
                table: "Offer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Offer",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Offer",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Offer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Offer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ValidityInDays",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SystemSetting",
                columns: table => new
                {
                    SystemSetting_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSetting", x => x.SystemSetting_id);
                });

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
