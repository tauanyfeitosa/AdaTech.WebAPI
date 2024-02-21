
using AdaTech.WebAPI.DadosLibrary.DTO.Enums;
using AdaTech.WebAPI.DadosLibrary.DTO.Relacional;

namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class DevolucaoTroca
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public Venda Venda { get; set; }
        public DateTime DataDevolucaoTroca { get; set; }
        public TipoDevolucaoTroca Tipo { get; set; }
        public string Motivo { get; set; }
        public IEnumerable<ItemDevolucaoTroca> Itens { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
