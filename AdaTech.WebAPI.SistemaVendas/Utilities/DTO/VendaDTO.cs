using AdaTech.WebAPI.DadosLibrary.DTO.Enums;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.DTO
{
    public class VendaDTO
    {
        [Required(ErrorMessage = "É obrigatório informar a data da venda!")]
        [DataType(DataType.DateTime)]
        [NotFutureDate(ErrorMessage = "A data da venda não pode ser no futuro.")]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o CPF do cliente!")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 dígitos!", MinimumLength = 11)]
        public string ClienteCPF { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o status da venda!")]
        public StatusVenda StatusVenda { get; set; }
    }
}
