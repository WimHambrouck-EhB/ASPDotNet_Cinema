using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPDotNet_Cinema.Migrations
{
    public partial class MovieLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Screenings");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Movies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Screenings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
