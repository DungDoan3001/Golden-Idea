using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class indexDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ideas_CreatedAt",
                table: "Ideas",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_Slug",
                table: "Ideas",
                column: "Slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ideas_CreatedAt",
                table: "Ideas");

            migrationBuilder.DropIndex(
                name: "IX_Ideas_Slug",
                table: "Ideas");
        }
    }
}
