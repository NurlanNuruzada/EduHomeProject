using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class addedColumnToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseFee",
                table: "CourseDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseFee",
                table: "CourseDetail");
        }
    }
}
