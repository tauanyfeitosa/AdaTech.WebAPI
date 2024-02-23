using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemVendaController : ControllerBase
    {
        private readonly IRepository<ItemVenda> _itemVendaRepository;
        private readonly ILogger<ItemVendaController> _logger;

        public ItemVendaController(IRepository<ItemVenda> itemVendaRepository, ILogger<ItemVendaController> logger)
        {
            _itemVendaRepository = itemVendaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var itemVendas = await _itemVendaRepository.GetAllAsync();
            if (itemVendas == null)
            {
                _logger.LogWarning("Nenhum item de venda encontrado.");
                throw new NotFoundException("Nenhum item de venda encontrado. Experimente cadastrar um novo item de venda!");
            }
            return Ok(itemVendas);
        }

        /// <summary>
        /// Obtém todos os itens vendidos em soft delete.
        /// </summary>
        [HttpGet("inactive-item-venda")]
        public async Task<IActionResult> GetInactiveItemVenda()
        {
            var itemVendas = await _itemVendaRepository.GetInactiveAsync();
            if (itemVendas == null)
            {
                _logger.LogWarning("Nenhum item de venda encontrado.");
                throw new NotFoundException("Nenhum item de venda encontrado. Experimente cadastrar um novo item de venda!");
            }
            return Ok(itemVendas);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
