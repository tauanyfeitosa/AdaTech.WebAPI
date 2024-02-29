using AdaTech.WebAPI.Aplicacoes.Exceptions;
using AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface;
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.ObjectService.EnderecoServiceCRUD
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
