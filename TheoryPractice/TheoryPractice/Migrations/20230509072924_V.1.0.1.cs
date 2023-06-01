using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheoryPractice.Migrations
{
    public partial class V101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6df6c362-3d13-4082-95f4-7e6b476aa8f0", "4e997804-beaf-4d6c-8e50-9e49d0ddad68", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "846a4c3c-94bd-4b76-87f0-0fcfe1301ff3", "8ac38107-a5b9-4ee0-99ef-b52dbca0ab6e", "Oyuncu", "OYUNCU" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "938c1d83-8413-43c2-811d-af070fa8c5d4", "5118172b-2843-421d-ad4b-72abeb40064e", "Editör", "EDITOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6df6c362-3d13-4082-95f4-7e6b476aa8f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "846a4c3c-94bd-4b76-87f0-0fcfe1301ff3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "938c1d83-8413-43c2-811d-af070fa8c5d4");
        }
    }
}
