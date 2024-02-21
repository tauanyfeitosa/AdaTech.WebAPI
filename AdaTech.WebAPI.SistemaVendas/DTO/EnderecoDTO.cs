using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdaTech.WebAPI.SistemaVendas.DTO
{
    public class EnderecoDTO
    {
        [JsonPropertyName("street")]
        [Required(ErrorMessage = "O campo Rua é obrigatório")]
        public string? Rua { get; set; }

        [JsonIgnore]
        public int Numero { get; set; }

        [JsonPropertyName("neighborhood")]
        [Required(ErrorMessage = "O campo Bairro é obrigatório")]
        public string? Bairro { get; set; }

        [JsonPropertyName("city")]
        [Required(ErrorMessage = "O campo Cidade é obrigatório")]
        public string? Cidade { get; set; }

        [JsonPropertyName("state")]
        [Required(ErrorMessage = "O campo Estado é obrigatório")]
        public string? Estado { get; set; }

        [JsonPropertyName("cep")]
        [Required(ErrorMessage = "O campo CEP é obrigatório")]
        public string? CEP { get; set; }
    }
}
