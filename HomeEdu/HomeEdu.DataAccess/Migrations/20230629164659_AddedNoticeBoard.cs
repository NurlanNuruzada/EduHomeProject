using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEdu.DataAccess.Migrations
{
    public partial class AddedNoticeBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "noticeBoards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_noticeBoards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "noticeBoards");
        }
    }
}
