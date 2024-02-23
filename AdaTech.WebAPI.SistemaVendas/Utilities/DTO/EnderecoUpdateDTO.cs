using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.DTO
{
    public class EnderecoUpdateDTO
    {
        [Required(ErrorMessage = "O campo Rua é obrigatório")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O campo Número é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo Número deve ser maior que 0")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "O campo Bairro é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo Bairro deve ter no máximo 100 caracteres")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo Cidade deve ter no máximo 100 caracteres")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório")]
        [StringLength(2, ErrorMessage = "O campo Estado deve ter 2 caracteres")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O campo CEP é obrigatório")]
        [StringLength(8, ErrorMessage = "O campo CEP deve ter 8 caracteres")]
        public string CEP { get; set; }

        [StringLength(100, ErrorMessage = "O campo Complemento deve ter no máximo 100 caracteres")]
        public string Complemento { get; set; }
    }
}
