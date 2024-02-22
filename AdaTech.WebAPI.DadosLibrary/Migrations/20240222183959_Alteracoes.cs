using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdaTech.WebAPI.DadosLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Alteracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preco",
                table: "ItensVenda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Preco",
                table: "ItensVenda",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
