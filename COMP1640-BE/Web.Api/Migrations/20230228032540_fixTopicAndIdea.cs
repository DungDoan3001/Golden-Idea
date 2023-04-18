using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class fixTopicAndIdea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Topics");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Ideas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Ideas");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Topics",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
