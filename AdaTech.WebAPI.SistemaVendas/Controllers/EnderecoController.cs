using Microsoft.AspNetCore.Mvc;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Filters;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger;
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services;


namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [SwaggerDisplayName("CRUD - Endereço")]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(LoggingActionFilter))]
    public class EnderecoController : ControllerBase
    {
        private readonly IRepository<Endereco> _enderecoRepository;
        private readonly ILogger<EnderecoController> _logger;
        private readonly DataContext _context;
        private readonly EnderecoService _enderecoService;

        public EnderecoController(IRepository<Endereco> enderecoRepository, ILogger<EnderecoController> logger,
            DataContext dataContext, EnderecoService enderecoService)
        {
            _enderecoRepository = enderecoRepository;
            _logger = logger;
            _context = dataContext;
            _enderecoService = enderecoService;
        }

        /// <summary>
        /// Obtém a lista de todos os endereços.
        /// </summary>
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

        /// <summary>
        /// Obtém todos os endereços em soft delete.
        /// </summary>
        [HttpGet("inactive-address")]
        public async Task<ActionResult<IEnumerable<Endereco>>> GetInactiveAddress()
        {
            var enderecos = await _enderecoRepository.GetInactiveAsync();
            if (enderecos == null)
            {
                _logger.LogWarning("Nenhum endereço encontrado.");
                throw new NotFoundException("Nenhum endereço encontrado. Experimente cadastrar um cliente e seu endereço vinculado!");
            }
            return Ok(enderecos);
        }

        /// <summary>
        /// Obtém um endereço ao informar um Id.
        /// </summary>

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

        /// <summary>
        /// Atualiza os campos de um endereço.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Put(int id, [FromBody] EnderecoUpdateDTO enderecoUpdate)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var endereco = await _enderecoRepository.GetByIdAsync(id);
                if (endereco == null)
                {
                    _logger.LogWarning("Endereço com ID: {Id} não encontrado.", id);
                    throw new NotFoundException("Endereço não encontrado. Experimente buscar por outro ID!");
                }

                endereco = await _enderecoService.UpdateAdress(endereco, enderecoUpdate);

                var result = await _enderecoRepository.UpdateAsync(endereco);

                if (!result)
                {
                    _logger.LogWarning("Erro ao atualizar endereço.");
                    throw new FailCreateUpdateException("Erro ao atualizar endereço. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Endereço com ID: {Id} atualizado com sucesso.", id);
                return Ok("Atualizado com sucesso!");
            }
        }

        /// <summary>
        /// Efetua o hard ou soft delete de um endereço.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
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

                await transaction.CommitAsync();

                _logger.LogInformation("Endereço com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", id, hardDelete);
                return Ok("Excluído com sucesso!");
            }
        }

        /// <summary>
        /// Ativa ou inativa um endereço
        /// </summary>

        [HttpPatch("activate")]
        public async Task<IActionResult> Patch(int id, [FromQuery] bool ativar)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Iniciando ativação/inativação do endereço com ID {Id}", id);

                var endereco = await _enderecoRepository.GetByIdActivateAsync(id);
                if (endereco == null)
                {
                    _logger.LogWarning("Endereço com ID {Id} não encontrado para ativação/inativação", id);
                    throw new NotFoundException("Endereço não encontrado para ativação/inativação. Experimente buscar por outro ID!");
                }

                if (endereco.Ativo == ativar)
                {
                    _logger.LogWarning("Endereço com ID {Id} já está ativado/inativado", id);
                    throw new ErrorInputUserException("Endereço já está ativado/inativado.");
                }
                endereco.Ativo = ativar;
                var result = await _enderecoRepository.UpdateAsync(endereco);

                if (!result)
                {
                    _logger.LogWarning("Erro ao ativar/inativar endereço.");
                    throw new FailCreateUpdateException("Erro ao ativar/inativar endereço. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Endereço com ID {Id} ativado/inativado com sucesso. Ativar: {Ativar}", id, ativar);
                return Ok("Ativado/Inativado com sucesso!");
            }
        }
    }
}
