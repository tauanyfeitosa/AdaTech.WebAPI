using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Enums;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.VendaServiceCRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [SwaggerDisplayName("CRUD - Venda")]
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly IRepository<Venda> _vendaRepository;
        private readonly ILogger<VendaController> _logger;
        private readonly DataContext _context;
        private readonly GenericsGetService<Venda> _genericsGetService;
        private readonly VendaCreateService _vendaCreateService;
        private readonly VendaUpdateService _vendaUpdateService;
        private readonly GenericsDeleteService<Venda> _genericsDeleteService;

        public VendaController(IRepository<Venda> vendaRepository, ILogger<VendaController> logger, DataContext dataContext,
            GenericsGetService<Venda> genericsGetService, VendaCreateService vendaCreateService,
            VendaUpdateService vendaUpdateService, GenericsDeleteService<Venda> genericsDeleteService)
        {
            _vendaRepository = vendaRepository;
            _logger = logger;
            _context = dataContext;
            _genericsGetService = genericsGetService;
            _vendaCreateService = vendaCreateService;
            _vendaUpdateService = vendaUpdateService;
            _genericsDeleteService = genericsDeleteService;
        }

        /// <summary>
        /// Obtêm todas as vendas ativas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _genericsGetService.GetAllAsync(_vendaRepository, _logger));
        }

        /// <summary>
        /// Obtém todas as vendas em soft delete.
        /// </summary>
        [HttpGet("inactive-venda")]
        public async Task<IActionResult> GetInactiveVenda()
        {
            return Ok(await _genericsGetService.GetInactiveAsync(_vendaRepository, _logger));
        }

        /// <summary>
        /// Obtêm uma venda por Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _genericsGetService.GetByIdAsync(_vendaRepository, _logger, id));
        }

        /// <summary>
        /// Cria uma nova venda e seus itens.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] VendaRequest vendaRequest)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _vendaCreateService.CreateVenda(vendaRequest, _logger);

                await transaction.CommitAsync();

                _logger.LogInformation("Venda criada com sucesso.");
                return Ok("Criado com sucesso!");
            }
        }

        /// <summary>
        /// Atualiza uma venda.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Put(int id, [FromBody] VendaDTO vendaDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _vendaUpdateService.UpdateVenda(vendaDTO, _logger, id);

                await transaction.CommitAsync();

                _logger.LogInformation("Venda atualizada com sucesso.");
                return Ok("Atualizado com sucesso!");
            }
        }

        /// <summary>
        /// Deleta uma venda.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, bool hardDelete = false)
        {
            var result = await _genericsDeleteService.DeleteAsync(_vendaRepository, _logger, _context, id, hardDelete);

            _logger.LogInformation("Venda deletada com sucesso.");

            return Ok(result);
            
        }

        /// <summary>
        /// Atualiza o status de uma venda.
        /// </summary>
        [HttpPatch("updateStatus")]
        public async Task<IActionResult> Patch(int id, [FromBody] StatusVenda statusVenda)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _vendaUpdateService.UpdateStatus(id, statusVenda, _logger);

                await transaction.CommitAsync();

                _logger.LogInformation("Status da venda atualizado com sucesso.");
                return Ok("Status da venda atualizado com sucesso!");
            }
            
        }

        /// <summary>
        /// Ativa ou Inativa uma venda.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> Patch(int id, bool ativo)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await _vendaUpdateService.UpdateActivate(id, ativo, _logger);

                await transaction.CommitAsync();

                _logger.LogInformation("Status da venda atualizado com sucesso.");
                return Ok("Status da venda atualizado com sucesso!");
            }
        }

    }
}
