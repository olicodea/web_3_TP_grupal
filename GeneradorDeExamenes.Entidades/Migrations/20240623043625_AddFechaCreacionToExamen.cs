using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneradorDeExamenes.Entidades.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaCreacionToExamen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Examen",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Examen");
        }
    }
}
