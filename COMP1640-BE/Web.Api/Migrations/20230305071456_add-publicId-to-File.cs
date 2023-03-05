using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class addpublicIdtoFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Ideas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Files",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Files");
        }
    }
}
