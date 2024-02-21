using Microsoft.AspNetCore.Mvc;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Filters;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(LoggingActionFilter))]
    public class EnderecoController : ControllerBase
    {
        private readonly IRepository<Endereco> _enderecoService;
        private readonly ILogger<EnderecoController> _logger;

        public EnderecoController(IRepository<Endereco> enderecoService, ILogger<EnderecoController> logger)
        {
            _enderecoService = enderecoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> Get()
        {
            var enderecos = await _enderecoService.GetAllAsync();
            if (enderecos == null)
            {
                _logger.LogWarning("Nenhum endereço encontrado.");
                throw new NotFoundException("Nenhum endereço encontrado. Experimente cadastrar um cliente e seu endereço vinculado!");
            }
            return Ok(enderecos);
        }

        [HttpGet("byId")]
        public async Task<ActionResult<Endereco>> Get(int id)
        {
            var endereco = await _enderecoService.GetByIdAsync(id);

            if (endereco == null)
            {
                _logger.LogWarning("Endereço com ID: {Id} não encontrado.", id);
                throw new NotFoundException("Endereço não encontrado. Experimente buscar por outro ID!");
            }

            return endereco;
        }

        [HttpPut("byId")]
        public async Task<IActionResult> Put(int id, [FromBody] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                _logger.LogWarning("ID do endereço não corresponde ao ID informado.");
                throw new ErrorInputUserException("ID do endereço não corresponde ao ID informado.");
            }

            var result = await _enderecoService.UpdateAsync(endereco);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _enderecoService.DeleteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
