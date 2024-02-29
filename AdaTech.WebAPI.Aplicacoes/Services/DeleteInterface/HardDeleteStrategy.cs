using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AdaTech.WebAPI.Aplicacoes.Exceptions;

namespace AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface
{
    public class HardDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    {
        public async Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                logger.LogInformation("Iniciando exclusão (hard delete) da entidade {EntityType} com ID {Id}", typeof(T).Name, id);

                var entity = await repository.GetByIdActivateAsync(id);
                if (entity == null)
                {
                    logger.LogWarning("{EntityType} com ID {Id} não encontrado para exclusão", typeof(T).Name, id);
                    throw new NotFoundException($"{typeof(T).Name} não encontrado para exclusão. Experimente buscar por outro ID!");
                }

                try
                {
                    logger.LogInformation("Realizando hard delete para a entidade {EntityType} com ID {Id}", typeof(T).Name, id);
                    var success = await repository.DeleteAsync(id);

                    if (success)
                    {
                        await transaction.CommitAsync();
                        logger.LogInformation("{EntityType} com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", typeof(T).Name, id, true);
                        return "Excluído com sucesso!";
                    }
                    else
                    {
                        throw new InvalidOperationException("Falha ao realizar hard delete. Tente novamente!");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao realizar hard delete para a entidade {EntityType} com ID {Id}", typeof(T).Name, id);
                    throw new InvalidOperationException("Falha ao realizar hard delete. Tente novamente! Verifique objetos relacionados ou considere um soft delete.");
                }
            }
        }
    }
}
