using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IRepository<Endereco> _enderecoRepository;
        private readonly EnderecoService _enderecoService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IRepository<Cliente> clienteRepository, IRepository<Endereco> enderecoRepository, EnderecoService enderecoService, ILogger<ClienteController> logger)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
            _enderecoService = enderecoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            _logger.LogInformation("Buscando todos os clientes.");
            var clientes = await _clienteRepository.GetAllAsync();
            _logger.LogInformation("Clientes recuperados com sucesso.");
            return Ok(clientes);
        }

        [HttpGet("byId")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            _logger.LogInformation("Buscando cliente com ID: {Id}", id);
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID: {Id} não encontrado.", id);
                return NotFound();
            }
            _logger.LogInformation("Cliente com ID: {Id} recuperado com sucesso.", id);
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] ClienteRequest clienteRequest)
        {
            _logger.LogInformation("Iniciando a criação de um novo cliente.");
            try
            {
                _logger.LogInformation("Buscando endereço para o CEP: {CEP}", clienteRequest.CEP);
                var endereco = await _enderecoService.GetEnderecoDto(clienteRequest);
                if (endereco == null)
                {
                    _logger.LogWarning("CEP inválido ou não encontrado: {CEP}", clienteRequest.CEP);
                    return BadRequest("CEP inválido ou não encontrado.");
                }

                await _enderecoRepository.AddAsync(endereco);

                var cliente = new Cliente
                {
                    Nome = clienteRequest.Nome,
                    Sobrenome = clienteRequest.Sobrenome,
                    Email = clienteRequest.Email,
                    Telefone = clienteRequest.Telefone,
                    CPF = clienteRequest.CPF,
                    EnderecoId = endereco.Id
                };

                _logger.LogInformation("Criando cliente: {Nome}", cliente.Nome);
                var success = await _clienteRepository.AddAsync(cliente);
                if (!success)
                {
                    _logger.LogError("Falha ao criar o cliente: {Nome}", cliente.Nome);
                    return BadRequest("Não foi possível criar o cliente.");
                }

                _logger.LogInformation("Cliente criado com sucesso: {Nome}", cliente.Nome);
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente: {Nome}", clienteRequest.Nome);
                return StatusCode(500, "Um erro ocorreu ao criar o cliente.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            _logger.LogInformation("Iniciando exclusão do cliente com ID {Id}", id);

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID {Id} não encontrado para exclusão", id);
                return NotFound();
            }

            if (hardDelete)
            {
                _logger.LogInformation("Realizando hard delete para o cliente com ID {Id}", id);
                await _clienteRepository.DeleteAsync(cliente.Id);
            }
            else
            {
                _logger.LogInformation("Realizando soft delete para o cliente com ID {Id}", id);
                cliente.Ativo = true;
                await _clienteRepository.UpdateAsync(cliente);
            }

            _logger.LogInformation("Cliente com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", id, hardDelete);
            return NoContent();
        }
    }
}
