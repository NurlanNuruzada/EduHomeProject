using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class addedCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCatagories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Catagory = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCatagories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutCourse = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    HowToApply = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    Certification = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assesments = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assesments_CourseDetail_CourseDetailId",
                        column: x => x.CourseDetailId,
                        principalTable: "CourseDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CourseCatagoryId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_CourseCatagories_CourseCatagoryId",
                        column: x => x.CourseCatagoryId,
                        principalTable: "CourseCatagories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_CourseDetail_CourseDetailId",
                        column: x => x.CourseDetailId,
                        principalTable: "CourseDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_CourseDetail_CourseDetailId",
                        column: x => x.CourseDetailId,
                        principalTable: "CourseDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillLevels_CourseDetail_CourseDetailId",
                        column: x => x.CourseDetailId,
                        principalTable: "CourseDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assesments_CourseDetailId",
                table: "Assesments",
                column: "CourseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCatagoryId",
                table: "Courses",
                column: "CourseCatagoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseDetailId",
                table: "Courses",
                column: "CourseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CourseDetailId",
                table: "Languages",
                column: "CourseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevels_CourseDetailId",
                table: "SkillLevels",
                column: "CourseDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assesments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "SkillLevels");

            migrationBuilder.DropTable(
                name: "CourseCatagories");

            migrationBuilder.DropTable(
                name: "CourseDetail");
        }
    }
}
