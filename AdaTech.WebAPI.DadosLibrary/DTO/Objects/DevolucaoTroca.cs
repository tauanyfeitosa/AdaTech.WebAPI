
using AdaTech.WebAPI.DadosLibrary.DTO.Enums;

namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class DevolucaoTroca
    {
        public DevolucaoTroca(int id, int vendaId, DateTime dataDevolucaoTroca, TipoDevolucaoTroca tipo, string motivo, IEnumerable<ItemVenda> itensVenda)
        {
            Id = id;
            VendaId = vendaId;
            DataDevolucaoTroca = dataDevolucaoTroca;
            Tipo = tipo;
            Motivo = motivo;
            ItensVenda = itensVenda;
        }

        public DevolucaoTroca() { }

        public int Id { get; set; }
        public int VendaId { get; set; }
        public Venda Venda { get; set; }
        public DateTime DataDevolucaoTroca { get; set; }
        public TipoDevolucaoTroca Tipo { get; set; }
        public string Motivo { get; set; }
        public IEnumerable<ItemVenda> ItensVenda { get; set; }
    }
}
