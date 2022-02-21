using Microsoft.EntityFrameworkCore.Migrations;

namespace MHRSLiteDataAccessLayer.Migrations
{
    public partial class HospitalTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Hospitals",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Hospitals",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Hospitals");
        }
    }
}
