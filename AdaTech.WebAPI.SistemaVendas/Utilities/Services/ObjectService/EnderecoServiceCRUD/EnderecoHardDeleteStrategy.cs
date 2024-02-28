using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.EnderecoServiceCRUD
{
    public class EnderecoHardDeleteStrategy
    {
        private readonly HardDeleteStrategy<Endereco> _hardDelete;
        private readonly IRepository<Cliente> _enderecoRepository;

        public EnderecoHardDeleteStrategy(HardDeleteStrategy<Endereco> hardDelete, IRepository<Cliente> enderecoRepository)
        {
            _hardDelete = hardDelete;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<string> DeleteAsync(DadosLibrary.Repository.IRepository<Endereco> repository, ILogger logger, DbContext context, int id)
        {
            var clientes = await _enderecoRepository.GetAsync();
            var clientesFiltrados = clientes.Where(cliente => cliente.EnderecoId == id).ToList();

            if (clientesFiltrados.Any())
            {
                throw new InvalidOperationException($"Não é possível realizar hard delete para o endereço com ID {id} porque existem clientes associados.");
            }

            return await _hardDelete.DeleteAsync(repository, logger, context, id);
        }
    }
}
