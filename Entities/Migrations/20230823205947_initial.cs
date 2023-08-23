using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Burgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Burgers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Burgers",
                columns: new[] { "Id", "Ingredients", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Beef,Cheese,Tomato", "Cheeseburger", 12m },
                    { 2, "Beef,Cheese,Bacon,Caramelized_Onion,Mushroom", "Special", 15m },
                    { 3, "\"Beef,Cheese,Tomato,Egg,Letucce\"", "Egger", 13m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Burgers");
        }
    }
}
