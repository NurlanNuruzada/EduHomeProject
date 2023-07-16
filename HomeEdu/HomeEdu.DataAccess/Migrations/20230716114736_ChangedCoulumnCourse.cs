using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class ChangedCoulumnCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutCourse",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "Certification",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "HowToApply",
                table: "CourseDetails");

            migrationBuilder.AddColumn<string>(
                name: "CourseDescription",
                table: "CourseDetails",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDescription",
                table: "CourseDetails");

            migrationBuilder.AddColumn<string>(
                name: "AboutCourse",
                table: "CourseDetails",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Certification",
                table: "CourseDetails",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HowToApply",
                table: "CourseDetails",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: false,
                defaultValue: "");
        }
    }
}
