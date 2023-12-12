using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAQL.Migrations
{
    public partial class newFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Patients_PatientId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_PatientId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DeviceId",
                table: "Patients",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DeviceId",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PatientId",
                table: "Devices",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Patients_PatientId",
                table: "Devices",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
