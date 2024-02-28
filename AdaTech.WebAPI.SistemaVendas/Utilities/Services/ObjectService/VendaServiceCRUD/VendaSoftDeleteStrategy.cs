using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.VendaServiceCRUD
{
    public class VendaSoftDeleteStrategy
    {
        private readonly IRepository<ItemVenda> _itemVendaRepository;
        private readonly SoftDeleteStrategy<Venda> _softDelete = new SoftDeleteStrategy<Venda>();

        public VendaSoftDeleteStrategy(IRepository<ItemVenda> itemVendaRepository, SoftDeleteStrategy<Venda> softDeleteStrategy)
        {
            _itemVendaRepository = itemVendaRepository;
            _softDelete = softDeleteStrategy;
        }

        public async Task<string> DeleteAsync(IRepository<Venda> repository, ILogger logger, DbContext context, int id)
        {
            var result = await _softDelete.DeleteAsync(repository, logger, context, id);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var itensVenda = await _itemVendaRepository.GetAsync();
                var itensFiltrados = itensVenda.Where(item => item.VendaId == id).ToList();

                foreach (var item in itensFiltrados)
                {
                    item.Ativo = false;
                    var itemDeleteResult = await _itemVendaRepository.UpdateAsync(item);
                    if (!itemDeleteResult)
                    {
                        throw new FailCreateUpdateException("Falha ao atualizar o status do item de venda. Tente novamente!");
                    }
                }

                await transaction.CommitAsync();
                return result;
            }
            
        }
    }

}
