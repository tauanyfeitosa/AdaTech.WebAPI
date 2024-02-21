using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdaTech.WebAPI.DadosLibrary.Migrations
{
    /// <inheritdoc />
    public partial class PropAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Vendas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "ItensVenda",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Enderecos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "DevolucoesTrocas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Clientes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "ItensVenda");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "DevolucoesTrocas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Clientes");
        }
    }
}
