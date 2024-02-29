using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AdaTech.WebAPI.Aplicacoes.Exceptions;


namespace AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface 
{
    public class SoftDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    {
        public async Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                logger.LogInformation("Iniciando exclusão (soft delete) da entidade {EntityType} com ID {Id}", typeof(T).Name, id);

                var entity = await repository.GetByIdActivateAsync(id);
                if (entity == null)
                {
                    logger.LogWarning("{EntityType} com ID {Id} não encontrado para exclusão", typeof(T).Name, id);
                    throw new NotFoundException($"{typeof(T).Name} não encontrado para exclusão. Experimente buscar por outro ID!");
                }

                var propertyInfo = entity.GetType().GetProperty("Ativo");
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    bool isActive = (bool)propertyInfo.GetValue(entity);
                    if (!isActive)
                    {
                        throw new InvalidOperationException("Entidade já está inativa.");
                    }

                    propertyInfo.SetValue(entity, false, null);
                    var success = await repository.UpdateAsync(entity);
                    if (success)
                    {
                        await transaction.CommitAsync();
                        logger.LogInformation("{EntityType} com ID {Id} marcado como inativo com sucesso.", typeof(T).Name, id);
                        return "Marcado como inativo com sucesso!";
                    }
                    else
                    {
                        throw new InvalidOperationException("Falha ao marcar a entidade como inativa. Tente novamente!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Não foi possível realizar soft delete, a propriedade 'Ativo' não foi encontrada ou não pode ser escrita.");
                }
            }
        }
    }
}
