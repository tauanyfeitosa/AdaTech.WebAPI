
using AdaTech.WebAPI.DadosLibrary.DTO.Relacional;

namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public int EnderecoId { get; set; }
        public string CPF { get; set; }
        public IEnumerable<Venda> Vendas { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
