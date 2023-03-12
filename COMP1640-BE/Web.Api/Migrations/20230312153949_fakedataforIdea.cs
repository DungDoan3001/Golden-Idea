using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class fakedataforIdea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFakeData",
                table: "Ideas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFakeData",
                table: "Ideas");
        }
    }
}
