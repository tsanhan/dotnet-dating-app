using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    // we hve 2 methods
    public partial class InitialCreate : Migration
    {
        // what's gonna happen when it's going up
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create a Users table w/2 columns (Id: INTEGER, primary, auto increment. UserName:TEXT )
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }
        // what's gonna happen when it's going down
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // dropping the table
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
