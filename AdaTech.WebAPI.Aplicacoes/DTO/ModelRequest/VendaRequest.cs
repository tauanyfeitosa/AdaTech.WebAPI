using AdaTech.WebAPI.Aplicacoes.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.Aplicacoes.DTO.ModelRequest
{
    public class VendaRequest
    {
        [Required(ErrorMessage = "É obrigatório colocar ao menos um produto!")]
        [MinLength(1, ErrorMessage = "É obrigatório colocar ao menos um produto!")]
        public IEnumerable<ItemVendaDTO> ItensVendas { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a data da venda!")]
        [DataType(DataType.DateTime)]
        [NotFutureDate(ErrorMessage = "A data da venda não pode ser no futuro.")]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o CPF do cliente!")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 dígitos!", MinimumLength = 11)]
        public string ClienteCPF { get; set; }
    }
}
