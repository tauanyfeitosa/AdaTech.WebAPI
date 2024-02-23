using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService;
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

        public ItemVendaController(IRepository<ItemVenda> itemVendaRepository, ILogger<ItemVendaController> logger, 
            GenericsGetService<ItemVenda> genericsService)
        {
            _itemVendaRepository = itemVendaRepository;
            _logger = logger;
            _genericsGetService = genericsService;
        }

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

        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _genericsGetService.GetByIdAsync(_itemVendaRepository, _logger, id));
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
