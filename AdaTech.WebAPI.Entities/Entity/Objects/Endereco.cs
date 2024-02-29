
namespace AdaTech.WebAPI.Entities.Entity.Objects
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string? Complemento { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
