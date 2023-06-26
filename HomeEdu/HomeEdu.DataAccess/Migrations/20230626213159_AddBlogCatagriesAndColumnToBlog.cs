using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class AddBlogCatagriesAndColumnToBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Blogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "BlogCatagoryId",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlogCatagories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Catagory = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCatagories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCatagoryId",
                table: "Blogs",
                column: "BlogCatagoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCatagories_BlogCatagoryId",
                table: "Blogs",
                column: "BlogCatagoryId",
                principalTable: "BlogCatagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCatagories_BlogCatagoryId",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "BlogCatagories");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogCatagoryId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BlogCatagoryId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Blogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
