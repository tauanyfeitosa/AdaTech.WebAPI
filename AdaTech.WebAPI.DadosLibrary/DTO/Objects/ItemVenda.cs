
namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class ItemVenda
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int VendaId { get; set; }
        public Venda Venda { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
