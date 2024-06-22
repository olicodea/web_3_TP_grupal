using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneradorDeExamenes.Entidades.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "Categoria",
            //     table: "Examen");

            migrationBuilder.AddColumn<int>(
                name: "IdCategoria",
                table: "Examen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categoria__19093A0B4B7734FF", x => x.IdCategoria);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Examen_IdCategoria",
                table: "Examen",
                column: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK__Examen__IdCatego__5EBF139D",
                table: "Examen",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Examen__IdCatego__5EBF139D",
                table: "Examen");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Examen_IdCategoria",
                table: "Examen");

            migrationBuilder.DropColumn(
                name: "IdCategoria",
                table: "Examen");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Examen",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
