using AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface;
using AdaTech.WebAPI.Aplicacoes.Services.ObjectService.EnderecoServiceCRUD;
using AdaTech.WebAPI.Aplicacoes.Services.ObjectService.VendaServiceCRUD;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.Entities.Entity.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.GenericsService
{
    public class GenericsDeleteService<T> where T : class
    {
        private readonly VendaSoftDeleteStrategy _vendaSoftDeleteStrategy;
        private readonly EnderecoSoftDeleteStrategy _enderecoSoftDeleteStrategy;
        private readonly VendaHardDeleteStrategy _vendaHardDeleteStrategy;
        private readonly EnderecoHardDeleteStrategy _enderecoHardDeleteStrategy;

        public GenericsDeleteService(VendaSoftDeleteStrategy vendaSoftDeleteStrategy, EnderecoSoftDeleteStrategy enderecoSoftDeleteStrategy,
                       VendaHardDeleteStrategy vendaHardDeleteStrategy, EnderecoHardDeleteStrategy enderecoHardDeleteStrategy)
        {
            _vendaSoftDeleteStrategy = vendaSoftDeleteStrategy;
            _enderecoSoftDeleteStrategy = enderecoSoftDeleteStrategy;
            _vendaHardDeleteStrategy = vendaHardDeleteStrategy;
            _enderecoHardDeleteStrategy = enderecoHardDeleteStrategy;
        }

        public async Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id, bool hardDelete)
        {
            if (typeof(T) == typeof(Venda))
            {
                return hardDelete ? await _vendaHardDeleteStrategy.DeleteAsync((IRepository<Venda>)repository, logger, context, id): 
                    await _vendaSoftDeleteStrategy.DeleteAsync((IRepository<Venda>)repository, logger, context, id);
            } else if (typeof(T) == typeof(Endereco))
            {
                return hardDelete ? await _enderecoHardDeleteStrategy.DeleteAsync((IRepository<Endereco>)repository, logger, context, id): 
                    await _enderecoSoftDeleteStrategy.DeleteAsync((IRepository<Endereco>)repository, logger, context, id);
            }
            else
            {
                IDeleteStrategy<T> strategy = hardDelete ? new HardDeleteStrategy<T>() : new SoftDeleteStrategy<T>();
                return await strategy.DeleteAsync(repository, logger, context, id);
            }
        }
    }
}
