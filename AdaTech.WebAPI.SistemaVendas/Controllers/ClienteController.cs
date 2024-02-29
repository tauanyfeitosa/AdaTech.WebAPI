using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Filters;
using Microsoft.AspNetCore.Mvc;
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.Aplicacoes.Attributes.Swagger;
using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.Aplicacoes.Services.ObjectService;
using AdaTech.WebAPI.Aplicacoes.Services.GenericsService;
using AdaTech.WebAPI.Aplicacoes.DTO.ModelRequest;
using AdaTech.WebAPI.Aplicacoes.Exceptions;

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
        private readonly DataContext _context;
        private readonly GenericsGetService<Cliente> _genericsGetService;
        private readonly GenericsDeleteService<Cliente> _genericsDeleteService;

        public ClienteController(IRepository<Cliente> clienteRepository, IRepository<Endereco> enderecoRepository, 
            EnderecoService enderecoService, ILogger<ClienteController> logger, DataContext dataContext, 
            GenericsGetService<Cliente> genericsService, GenericsDeleteService<Cliente> genericsDeleteService)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
            _enderecoService = enderecoService;
            _logger = logger;
            _genericsGetService = genericsService;
            _genericsDeleteService = genericsDeleteService;
            _context = dataContext;
        }

        /// <summary>
        /// Obtém todos os clientes ativos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            return Ok(await _genericsGetService.GetAllAsync(_clienteRepository, _logger));
        }

        /// <summary>
        /// Obtém todos os clientes em soft delete.
        /// </summary>
        [HttpGet("inactive-client")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetInactiveClient()
        {
            return Ok(await _genericsGetService.GetInactiveAsync(_clienteRepository, _logger));
        }

        /// <summary>
        /// Obtém um cliente por Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            return Ok(await _genericsGetService.GetByIdAsync(_clienteRepository, _logger, id));
        }

        /// <summary>
        /// Cria um novo cliente juntamente com seu endereço.
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] ClienteRequest clienteRequest)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Buscando endereço para o CEP: {CEP}", clienteRequest.CEP);
                var endereco = await _enderecoService.GetEnderecoDto(clienteRequest);
                if (endereco == null)
                {
                    _logger.LogWarning("CEP inválido ou não encontrado: {CEP}", clienteRequest.CEP);
                    throw new NotFoundException("CEP inválido ou não encontrado. Experimente outro CEP!");
                }

                endereco.Ativo = true;
                await _enderecoRepository.UpdateAsync(endereco);

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

                await transaction.CommitAsync();

                _logger.LogInformation("Cliente criado com sucesso: {Nome}", cliente.Nome);
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
        }

        /// <summary>
        /// Efetua o hard ou soft delete de um cliente.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            return Ok(await _genericsDeleteService.DeleteAsync(_clienteRepository, _logger, _context, id, hardDelete));
        }

        /// <summary>
        /// Atualiza um cliente.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteUpdateRequest clienteUpdateRequest)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
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

                await transaction.CommitAsync();

                _logger.LogInformation("Cliente atualizado com sucesso: {Nome}", cliente.Nome);
                return Ok("Cliente atualizado com sucesso!");
            }
        }

        /// <summary>
        /// Ativa ou inativa um cliente.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> PatchCliente(int id, bool ativo)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
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

                await transaction.CommitAsync();

                _logger.LogInformation("Status do cliente atualizado com sucesso: {Nome}", cliente.Nome);
                return Ok("Status do cliente atualizado com sucesso!");
            }
        }
    }
}
