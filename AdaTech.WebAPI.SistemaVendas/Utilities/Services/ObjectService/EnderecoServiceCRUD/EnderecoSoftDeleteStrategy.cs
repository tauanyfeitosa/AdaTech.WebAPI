using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.EnderecoServiceCRUD
{
    public class EnderecoSoftDeleteStrategy
    {
        private readonly ClienteService _clienteService;
        private readonly SoftDeleteStrategy<Endereco> _softDelete;

        public EnderecoSoftDeleteStrategy(ClienteService clienteService, SoftDeleteStrategy<Endereco> softDelete)
        {
            _clienteService = clienteService;
            _softDelete = softDelete;
        }

        public async Task<string> DeleteAsync(IRepository<Endereco> repository, ILogger logger, DbContext context, int id)
        {
            var result = await _softDelete.DeleteAsync(repository, logger, context, id);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var clientes = await _clienteService.GetClientAdress(id);

                foreach (var cliente in clientes)
                {

                    cliente.Ativo = false;
                    var sucess = await _clienteService.UpdateAsync(cliente);
                    if (!sucess)
                    {
                        logger.LogWarning("Erro ao desativar endereço do cliente com ID {Id}", cliente.Id);
                        throw new FailCreateUpdateException("Erro ao desativar endereço do cliente. Tente novamente!");
                    }
                }

                await transaction.CommitAsync();
            }

            return result;
        }
    }
}
