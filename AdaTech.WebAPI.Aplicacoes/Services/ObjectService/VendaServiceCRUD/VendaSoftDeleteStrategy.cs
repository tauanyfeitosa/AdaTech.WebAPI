using AdaTech.WebAPI.Aplicacoes.Exceptions;
using AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface;
using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.ObjectService.VendaServiceCRUD
{
    public class VendaSoftDeleteStrategy
    {
        private readonly IRepository<ItemVenda> _itemVendaRepository;
        private readonly SoftDeleteStrategy<Venda> _softDelete;

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
