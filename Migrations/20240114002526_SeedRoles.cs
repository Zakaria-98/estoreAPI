using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeAPI.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                   table: "AspNetRoles",
                   columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                   values: new object[] { "1", "User", "USER", "100" }
               );

            migrationBuilder.InsertData(
       table: "AspNetRoles",
       columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
       values: new object[] { "2", "Admin", "ADMIN", "200" }
   );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM[AspNetRoles]");

        }
    }
}
