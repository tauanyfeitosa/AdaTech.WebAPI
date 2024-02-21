
namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class ItemVenda
    {
        public ItemVenda(int id, int produtoId, int quantidade, decimal preco)
        {
            Id = id;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Preco = preco;
        }

        public ItemVenda() { }

        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int VendaId { get; set; }
        public Venda Venda { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal ValorTotal => Quantidade * Preco;
    }
}
