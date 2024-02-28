using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.VendaServiceCRUD;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService
{
    public class GenericsDeleteService<T> where T : class
    {
        private readonly VendaSoftDeleteStrategy _vendaSoftDeleteStrategy;
        private readonly VendaHardDeleteStrategy _vendaHardDeleteStrategy;

        public GenericsDeleteService(
            VendaSoftDeleteStrategy vendaSoftDeleteStrategy,
            VendaHardDeleteStrategy vendaHardDeleteStrategy)
        {
            _vendaSoftDeleteStrategy = vendaSoftDeleteStrategy;
            _vendaHardDeleteStrategy = vendaHardDeleteStrategy;
        }

        public async Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id, bool hardDelete)
        {
            if (typeof(T) == typeof(Venda))
            {
                return hardDelete ? await _vendaHardDeleteStrategy.DeleteAsync((IRepository<Venda>)repository, logger, context, id): 
                    await _vendaSoftDeleteStrategy.DeleteAsync((IRepository<Venda>)repository, logger, context, id);
            }
            else
            {
                IDeleteStrategy<T> strategy = hardDelete ? new HardDeleteStrategy<T>() : new SoftDeleteStrategy<T>();
                return await strategy.DeleteAsync(repository, logger, context, id);
            }
        }
    }
}
