using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kargo_İlan.Migrations
{
    public partial class Usercompanyrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompany_User_User_id",
                table: "UserCompany");

      
            migrationBuilder.AddForeignKey(
                name: "FK_UserCompany_User_User_id",  
                table: "UserCompany",
                column: "User_id",  
                principalTable: "User",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Restrict);  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddForeignKey(
                name: "FK_UserCompany_User_User_id",  
                table: "UserCompany",
                column: "User_id",
                principalTable: "User",
                principalColumn: "User_id");
        }
    }
}
