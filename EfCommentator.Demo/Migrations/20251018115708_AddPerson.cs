using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCommenter.Demo.Migrations
{
    /// <inheritdoc />
    public partial class AddPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "A comment to test!!!"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0: Admin | \n1: User | \n2: Guest | ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                },
                comment: "The class declered a person!!!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
