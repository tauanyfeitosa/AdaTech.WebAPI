
using AdaTech.WebAPI.Entities.Entity.Objects;

namespace AdaTech.WebAPI.Entities.Entity.Relacional
{
    public class ItemDevolucaoTroca
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int DevolucaoTrocaId { get; set; }
        public DevolucaoTroca DevolucaoTroca { get; set; }
        public int Quantidade { get; set; }
    }
}
