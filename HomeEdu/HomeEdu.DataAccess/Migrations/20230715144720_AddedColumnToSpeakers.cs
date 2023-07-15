using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class AddedColumnToSpeakers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "passCode",
                table: "Speakers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "passCode",
                table: "Speakers");
        }
    }
}
