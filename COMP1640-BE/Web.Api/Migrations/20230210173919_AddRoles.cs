using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a6f6f9e8-b994-4812-b2d9-7eb9ee9fbcbf", "d2aa95c3-4d10-43b5-a614-fb5cda302ee2", "Administrator", "ADMINISTRATOR" },
                    { "e7bccec5-63c3-4204-a891-eb162e484aa5", "0f7db73c-4efe-4401-83d6-88e2ba4a0f73", "QA Manager", "QA MANAGER" },
                    { "35c69a33-434f-4f86-989d-04b5e73b82f4", "8525ab48-15fd-4f28-b4bb-3ec1dde7b0dc", "QA Coordinator", "QA COORDINATOR" },
                    { "6db80264-d78d-4802-9546-69e7e00b80cb", "314bc5c3-56bc-4d85-81d9-3e3b14ba4277", "Staff", "STAFF" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole");
        }
    }
}
