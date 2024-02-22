using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services
{
    public class ClienteService
    {
        private readonly IRepository<Cliente> _repository;

        public ClienteService(IRepository<Cliente> repository)
        {
            _repository = repository;
        }

        public async Task<Cliente> GetCPFAsync(string cpf)
        {
            var clientes = await _repository.GetAllAsync();

            var cliente = clientes.FirstOrDefault(c => c.CPF == cpf);

            if (cliente == null)
                return null;

            return cliente;
        }
    }
}
