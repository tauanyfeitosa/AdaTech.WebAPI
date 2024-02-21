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
        private readonly IRepository<Endereco> _enderecoRepository;
        private readonly ILogger<EnderecoController> _logger;

        public EnderecoController(IRepository<Endereco> enderecoRepository, ILogger<EnderecoController> logger)
        {
            _enderecoRepository = enderecoRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> Get()
        {
            var enderecos = await _enderecoRepository.GetAllAsync();
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
            var endereco = await _enderecoRepository.GetByIdAsync(id);

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

            var result = await _enderecoRepository.UpdateAsync(endereco);

            if (!result)
            {
                _logger.LogWarning("Erro ao atualizar endereço.");
                throw new FailCreateUpdateException("Erro ao atualizar endereço. Tente novamente!");
            }

            return Ok("Atualizado com sucesso!");
        }

        [HttpDelete("byId")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            _logger.LogInformation("Iniciando exclusão do endereço com ID {Id}", id);

            var endereco = await _enderecoRepository.GetByIdAsync(id);
            if (endereco == null)
            {
                _logger.LogWarning("Endereço com ID {Id} não encontrado para exclusão", id);
                throw new NotFoundException("Endereço não encontrado para exclusão. Experimente buscar por outro ID!");
            }

            if (hardDelete)
            {
                _logger.LogInformation("Realizando hard delete para o endereço com ID {Id}", id);
                await _enderecoRepository.DeleteAsync(endereco.Id);
            }
            else
            {
                _logger.LogInformation("Realizando soft delete para o endereço com ID {Id}", id);
                endereco.Ativo = false;
                await _enderecoRepository.UpdateAsync(endereco);
            }

            _logger.LogInformation("Endereço com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", id, hardDelete);
            return Ok("Excluído com sucesso!");
        }

    }
}
