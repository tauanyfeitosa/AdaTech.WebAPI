using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.Aplicacoes.DTO
{
    public class ItemVendaDTO
    {
        [Required(ErrorMessage = "É obrigatório informar o ID do produto!")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do produto deve ser maior que 0!")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a quantidade do produto!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade do produto deve ser maior que 0!")]
        public int Quantidade { get; set; }
    }
}
