
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
        public string Endereco { get; set; }
        public string CPF { get; set; }
        public IEnumerable<Venda> Vendas { get; set; }
    }
}
