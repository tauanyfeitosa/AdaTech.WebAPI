using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.Aplicacoes.DTO.ModelRequest
{
    public class ClienteRequest
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

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [Range(10000000, 99999999, ErrorMessage = "O CEP deve ser um número de 8 dígitos.")]
        public int CEP { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O número deve ser positivo.")]
        public int Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
