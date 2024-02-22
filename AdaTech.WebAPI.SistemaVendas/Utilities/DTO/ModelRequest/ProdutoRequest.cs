using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest
{
    public class ProdutoRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 999999999, ErrorMessage = "O preço deve ser um valor positivo.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser um valor positivo.")]
        public int Quantidade { get; set; }
    }
}
