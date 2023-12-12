using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAQL.Migrations
{
    public partial class diagnosePatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Diagnose",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diagnose",
                table: "Patients");
        }
    }
}
