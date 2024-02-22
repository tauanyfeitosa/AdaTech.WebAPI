using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Filters;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services;
using AdaTech.WebAPI.SistemaVendas.Utilities.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [SwaggerDisplayName("CRUD - Cliente")]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(LoggingActionFilter))]
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

        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            if (clientes == null)
            {
                _logger.LogWarning("Nenhum cliente encontrado.");
                throw new NotFoundException("Nenhum cliente encontrado. Experimente cadastrar um novo cliente!");
            }
            return Ok(clientes);
        }

        /// <summary>
        /// Obtém um cliente por Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            _logger.LogInformation("Buscando cliente com ID: {Id}", id);
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID: {Id} não encontrado.", id);
                throw new NotFoundException("Cliente não encontrado. Experimente buscar por outro ID!");
            }
            _logger.LogInformation("Cliente com ID: {Id} recuperado com sucesso.", id);
            return Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente juntamente com seu endereço.
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] ClienteRequest clienteRequest)
        {
            _logger.LogInformation("Buscando endereço para o CEP: {CEP}", clienteRequest.CEP);
            var endereco = await _enderecoService.GetEnderecoDto(clienteRequest);
            if (endereco == null)
            {
                _logger.LogWarning("CEP inválido ou não encontrado: {CEP}", clienteRequest.CEP);
                throw new NotFoundException("CEP inválido ou não encontrado. Experimente outro CEP!");
            }

            endereco.Ativo = true;
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
                throw new FailCreateUpdateException("Falha ao criar o cliente. Tente novamente!");
            }

            _logger.LogInformation("Cliente criado com sucesso: {Nome}", cliente.Nome);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Efetua o hard ou soft delete de um cliente.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            _logger.LogInformation("Iniciando exclusão do cliente com ID {Id}", id);

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID {Id} não encontrado para exclusão", id);
                throw new NotFoundException("Cliente não encontrado para exclusão. Experimente buscar por outro ID!");
            }

            if (hardDelete)
            {
                _logger.LogInformation("Realizando hard delete para o cliente com ID {Id}", id);
                await _clienteRepository.DeleteAsync(cliente.Id);
            }
            else
            {
                _logger.LogInformation("Realizando soft delete para o cliente com ID {Id}", id);
                cliente.Ativo = false;
                await _clienteRepository.UpdateAsync(cliente);
            }

            _logger.LogInformation("Cliente com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", id, hardDelete);
            return Ok("Excluído com sucesso!");
        }

        /// <summary>
        /// Atualiza um cliente.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteUpdateRequest clienteUpdateRequest)
        {
            _logger.LogInformation("Atualizando cliente com ID: {Id}", id);

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID: {Id} não encontrado.", id);
                throw new NotFoundException("Cliente não encontrado. Experimente buscar por outro ID!");
            }

            cliente.Nome = clienteUpdateRequest.Nome;
            cliente.Sobrenome = clienteUpdateRequest.Sobrenome;
            cliente.Email = clienteUpdateRequest.Email;
            cliente.Telefone = clienteUpdateRequest.Telefone;
            cliente.CPF = clienteUpdateRequest.CPF;

            _logger.LogInformation("Atualizando cliente: {Nome}", cliente.Nome);
            var success = await _clienteRepository.UpdateAsync(cliente);
            if (!success)
            {
                _logger.LogError("Falha ao atualizar o cliente: {Nome}", cliente.Nome);
                throw new FailCreateUpdateException("Falha ao atualizar o cliente. Tente novamente!");
            }

            _logger.LogInformation("Cliente atualizado com sucesso: {Nome}", cliente.Nome);
            return Ok("Cliente atualizado com sucesso!");
        }

        /// <summary>
        /// Ativa ou inativa um cliente.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> PatchCliente(int id, bool ativo)
        {
            _logger.LogInformation("Atualizando status do cliente com ID: {Id}", id);

            var cliente = await _clienteRepository.GetByIdActivateAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID: {Id} não encontrado.", id);
                throw new NotFoundException("Cliente não encontrado. Experimente buscar por outro ID!");
            }

            if (cliente.Ativo == ativo)
            {
                _logger.LogWarning("Status do cliente já está como: {Ativo}", ativo);
                throw new FailCreateUpdateException("Status do cliente já está como desejado.");
            }
            cliente.Ativo = ativo;

            _logger.LogInformation("Atualizando status do cliente: {Nome}", cliente.Nome);
            var success = await _clienteRepository.UpdateAsync(cliente);
            if (!success)
            {
                _logger.LogError("Falha ao atualizar o status do cliente: {Nome}", cliente.Nome);
                throw new FailCreateUpdateException("Falha ao atualizar o status do cliente. Tente novamente!");
            }

            _logger.LogInformation("Status do cliente atualizado com sucesso: {Nome}", cliente.Nome);
            return Ok("Status do cliente atualizado com sucesso!");
        }
    }
}
