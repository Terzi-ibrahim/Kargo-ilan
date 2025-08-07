using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    public partial class Companychangename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tablo adını "Companies"dan "Company"ye değiştir
            migrationBuilder.RenameTable(
                name: "Companies", // Eski tablo adı
                newName: "Company" // Yeni tablo adı
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback işlemi: Tablo adını "Company"den tekrar "Companies"e geri al
            migrationBuilder.RenameTable(
                name: "Company", // Yeni tablo adı
                newName: "Companies" // Eski tablo adı
            );
        }
    }
}
