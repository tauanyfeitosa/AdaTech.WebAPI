using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.Entities.Entity.Objects;

namespace AdaTech.WebAPI.Aplicacoes.Services.ObjectService
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

        public async Task<IEnumerable<Cliente>> GetClientAdress(int id)
        {
            var clientes = await _repository.GetAllAsync();

            var clientesAdress = clientes.Where(c => c.EnderecoId == id);

            if (clientesAdress == null)
                return null;

            return clientesAdress;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            var success = await _repository.UpdateAsync(cliente);
            return success;
        }
    }
}
