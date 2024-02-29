using AdaTech.WebAPI.Aplicacoes.Exceptions;
using AdaTech.WebAPI.Aplicacoes.Services.GenericsService;
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.Entities.Entity.Objects;
using Microsoft.AspNetCore.Mvc;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemVendaController : ControllerBase
    {
        private readonly IRepository<ItemVenda> _itemVendaRepository;
        private readonly ILogger<ItemVendaController> _logger;
        private readonly GenericsGetService<ItemVenda> _genericsGetService;
        private readonly GenericsDeleteService<ItemVenda> _genericsDeleteService;
        private readonly DataContext _context;
        private readonly IRepository<Venda> _vendaRepository;

        public ItemVendaController(IRepository<ItemVenda> itemVendaRepository, ILogger<ItemVendaController> logger, 
            GenericsGetService<ItemVenda> genericsService, GenericsDeleteService<ItemVenda> genericsDeleteService,
            DataContext context, IRepository<Venda> repository)
        {
            _itemVendaRepository = itemVendaRepository;
            _logger = logger;
            _genericsGetService = genericsService;
            _genericsDeleteService = genericsDeleteService;
            _context = context;
            _vendaRepository = repository;
        }

        /// <summary>
        /// Obtém todos os itens vendidos ativos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _genericsGetService.GetAllAsync(_itemVendaRepository, _logger));
        }

        /// <summary>
        /// Obtém todos os itens vendidos em soft delete.
        /// </summary>
        [HttpGet("inactive-item-venda")]
        public async Task<IActionResult> GetInactiveItemVenda()
        {
            return Ok(await _genericsGetService.GetInactiveAsync(_itemVendaRepository, _logger));
        }

        /// <summary>
        /// Obtém um item vendido por ID.
        /// </summary>
        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _genericsGetService.GetByIdAsync(_itemVendaRepository, _logger, id));
        }

        /// <summary>
        /// Atualiza um item vendido por ID.
        /// </summary>
        [HttpPatch("byId")]
        public async Task<IActionResult> Patch(int id, int quantItens)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Iniciando atualização do item vendido com ID {Id}", id);
                var itemVenda = await _itemVendaRepository.GetByIdActivateAsync(id);

                if (itemVenda == null)
                {
                    _logger.LogWarning("Item vendido com ID {Id} não encontrado para atualização", id);
                    throw new NotFoundException("Item vendido não encontrado para atualização. Experimente buscar por outro ID!");
                }

                var valorTotal = itemVenda.ValorTotal;
                var produto = await _context.Produtos.FindAsync(itemVenda.ProdutoId);
                itemVenda.Quantidade = quantItens;
                itemVenda.ValorTotal = produto.Preco * quantItens;
                var success = await _itemVendaRepository.UpdateAsync(itemVenda);

                if (!success)
                {
                    _logger.LogError("Falha ao atualizar o item vendido com ID {Id}", id);
                    throw new InvalidOperationException("Falha ao atualizar o item vendido. Tente novamente!");
                }

                var venda = await _context.Vendas.FindAsync(itemVenda.VendaId);
                venda.ValorTotal = venda.ValorTotal - valorTotal + itemVenda.ValorTotal;

                var successVenda = await _vendaRepository.UpdateAsync(venda);

                if (!successVenda)
                {
                    _logger.LogError("Falha ao atualizar a venda do item vendido com ID {Id}", id);
                    throw new InvalidOperationException("Falha ao atualizar a venda do item vendido. Tente novamente!");
                }

                await transaction.CommitAsync();
                return Ok("Item vendido atualizado com sucesso!");
            }
            
        }

        /// <summary>
        /// Realiza a exclusão de um item vendido por ID.
        /// </summary>
        [HttpDelete("byId")]
        public async Task<IActionResult> Delete(int id, bool hardDelete = false)
        {
            var itemVenda = await _itemVendaRepository.GetByIdActivateAsync(id);
            var venda = await _context.Vendas.FindAsync(itemVenda.VendaId);

            if (venda == null || itemVenda == null)
            {
                _logger.LogWarning("Item vendido com ID {Id} não encontrado para exclusão ou venda não associada", id);
                throw new NotFoundException("Item vendido não encontrado para exclusão. Experimente buscar por outro ID!");
            }

            venda.ValorTotal = venda.ValorTotal - itemVenda.ValorTotal;
            var successVenda = await _vendaRepository.UpdateAsync(venda);
            if (!successVenda)
            {
                _logger.LogError("Falha ao atualizar a venda do item vendido com ID {Id}", id);
                throw new InvalidOperationException("Falha ao atualizar a venda do item vendido. Tente novamente!");
            }

            return Ok(await _genericsDeleteService.DeleteAsync(_itemVendaRepository, _logger, _context, id, hardDelete));
        }
    }
}
