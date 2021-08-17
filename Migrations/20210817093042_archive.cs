using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_school_ms.Migrations
{
    public partial class archive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Schools",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Classes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Classes");
        }
    }
}
