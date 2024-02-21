
namespace AdaTech.WebAPI.DadosLibrary.DTO.Objects
{
    public class Cliente
    {
        public Cliente(int id, string nome, string sobrenome, string email, string telefone, string endereco, string cpf)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Telefone = telefone;
            Endereco = endereco;
            CPF = cpf;
        }

        public Cliente() { }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string CPF { get; set; }
    }
}
