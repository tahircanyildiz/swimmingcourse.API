using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Persistence.Migrations
{
    public partial class addteachertocourseentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "teacherId",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "teacherId",
                table: "Courses");
        }
    }
}
