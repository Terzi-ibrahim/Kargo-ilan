using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    /// <inheritdoc />
    public partial class addoffercolumb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Offer",
                newName: "CreateAt");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Offer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Offer",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Offer");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Offer",
                newName: "CreatedDate");
        }
    }
}
