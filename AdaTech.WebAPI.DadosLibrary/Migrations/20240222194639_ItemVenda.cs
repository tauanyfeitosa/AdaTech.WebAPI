using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdaTech.WebAPI.DadosLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ItemVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "ItensVenda",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "ItensVenda");
        }
    }
}
