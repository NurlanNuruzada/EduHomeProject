using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class addedColumnnToCoursee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assesments_CourseDetail_CourseDetailId",
                table: "Assesments");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseDetail_CourseDetailId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_CourseDetail_CourseDetailId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevels_CourseDetail_CourseDetailId",
                table: "SkillLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseDetail",
                table: "CourseDetail");

            migrationBuilder.RenameTable(
                name: "CourseDetail",
                newName: "CourseDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseDetails",
                table: "CourseDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assesments_CourseDetails_CourseDetailId",
                table: "Assesments",
                column: "CourseDetailId",
                principalTable: "CourseDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseDetails_CourseDetailId",
                table: "Courses",
                column: "CourseDetailId",
                principalTable: "CourseDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_CourseDetails_CourseDetailId",
                table: "Languages",
                column: "CourseDetailId",
                principalTable: "CourseDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevels_CourseDetails_CourseDetailId",
                table: "SkillLevels",
                column: "CourseDetailId",
                principalTable: "CourseDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assesments_CourseDetails_CourseDetailId",
                table: "Assesments");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseDetails_CourseDetailId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_CourseDetails_CourseDetailId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevels_CourseDetails_CourseDetailId",
                table: "SkillLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseDetails",
                table: "CourseDetails");

            migrationBuilder.RenameTable(
                name: "CourseDetails",
                newName: "CourseDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseDetail",
                table: "CourseDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assesments_CourseDetail_CourseDetailId",
                table: "Assesments",
                column: "CourseDetailId",
                principalTable: "CourseDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseDetail_CourseDetailId",
                table: "Courses",
                column: "CourseDetailId",
                principalTable: "CourseDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_CourseDetail_CourseDetailId",
                table: "Languages",
                column: "CourseDetailId",
                principalTable: "CourseDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevels_CourseDetail_CourseDetailId",
                table: "SkillLevels",
                column: "CourseDetailId",
                principalTable: "CourseDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
