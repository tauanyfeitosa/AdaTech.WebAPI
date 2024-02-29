using AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface;
using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.ObjectService.VendaServiceCRUD
{
    public class VendaHardDeleteStrategy
    {
        private readonly IRepository<ItemVenda> _itemVendaRepository;
        private readonly HardDeleteStrategy<Venda> _hardDelete;

        public VendaHardDeleteStrategy(IRepository<ItemVenda> itemVendaRepository, HardDeleteStrategy<Venda> hardDelete)
        {
            _itemVendaRepository = itemVendaRepository;
            _hardDelete = hardDelete;
        }

        public async Task<string> DeleteAsync(IRepository<Venda> repository, ILogger logger, DbContext context, int id)
        {
            var itensVenda = await _itemVendaRepository.GetAsync();
            var itensFiltrados = itensVenda.Where(item => item.VendaId == id).ToList();

            if (itensFiltrados.Any())
            {
                throw new InvalidOperationException($"Não é possível realizar hard delete para a venda com ID {id} porque existem itens de venda associados.");
            }

            return await _hardDelete.DeleteAsync(repository, logger, context, id);
            
        }
    }

}
