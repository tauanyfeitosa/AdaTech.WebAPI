using AdaTech.WebAPI.DadosLibrary.DTO.Enums;

namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class Venda
    {
        public int Id { get; set; }
        public IEnumerable<ItemVenda> ItensVendas { get; set; }
        public DateTime DataVenda { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public StatusVenda StatusVenda { get; set; }
        public decimal ValorTotal => ItensVendas.Sum(p => p.ValorTotal);
        public bool Ativo { get; set; } = true;
    }
}
