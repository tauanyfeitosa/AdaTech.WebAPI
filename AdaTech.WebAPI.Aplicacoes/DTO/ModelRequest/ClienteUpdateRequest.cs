using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.Aplicacoes.DTO.ModelRequest
{
    public class ClienteUpdateRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo {1} caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O número de telefone não é válido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "O CPF deve conter 11 dígitos numéricos.")]
        public string CPF { get; set; }
    }
}
