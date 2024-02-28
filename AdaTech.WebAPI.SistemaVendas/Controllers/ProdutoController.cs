using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger;
using Microsoft.AspNetCore.Mvc;
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [SwaggerDisplayName("CRUD - Produto")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IRepository<Produto> _produtoRepository;
        private readonly ILogger<ProdutoController> _logger;
        private readonly DataContext _context;
        private readonly GenericsGetService<Produto> _genericsGetService;
        private readonly GenericsDeleteService<Produto> _genericsDeleteService;

        public ProdutoController(IRepository<Produto> produtoRepository, ILogger<ProdutoController> logger,
            DataContext dataContext, GenericsGetService<Produto> genericsService, GenericsDeleteService<Produto> genericsDeleteService)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
            _context = dataContext;
            _genericsGetService = genericsService;
            _genericsDeleteService = genericsDeleteService;
        }

        /// <summary>
        /// Obtém a lista de todos os produtos ativos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _genericsGetService.GetAllAsync(_produtoRepository, _logger));
        }

        /// <summary>
        /// Obtém todos os produtos em soft delete.
        /// </summary>
        [HttpGet("inactive-product")]
        public async Task<IActionResult> GetInactiveProduct()
        {
            return Ok(await _genericsGetService.GetInactiveAsync(_produtoRepository, _logger));
        }

        /// <summary>
        /// Obtém um produto ao informar um Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _genericsGetService.GetByIdAsync(_produtoRepository, _logger, id));
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] ProdutoRequest produtoRequest)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Cadastrando novo produto.");
                var produto = new Produto
                {
                    Nome = produtoRequest.Nome,
                    Descricao = produtoRequest.Descricao,
                    Preco = produtoRequest.Preco,
                    Quantidade = produtoRequest.Quantidade
                };
                var result = await _produtoRepository.AddAsync(produto);

                if (!result)
                {
                    _logger.LogWarning("Erro ao cadastrar produto.");
                    throw new FailCreateUpdateException("Erro ao cadastrar produto. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Produto cadastrado com sucesso.");
                return Ok(produto);
            }
        }

        /// <summary>
        /// Atualiza os campos de um produto.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoRequest produtoRequest)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Atualizando produto com ID: {Id}", id);
                var produto = await _produtoRepository.GetByIdAsync(id);
                if (produto == null)
                {
                    _logger.LogWarning("Produto com ID: {Id} não encontrado.", id);
                    throw new NotFoundException("Produto não encontrado. Experimente buscar por outro ID!");
                }

                produto.Nome = produtoRequest.Nome;
                produto.Descricao = produtoRequest.Descricao;
                produto.Preco = produtoRequest.Preco;
                produto.Quantidade = produtoRequest.Quantidade;

                var result = await _produtoRepository.UpdateAsync(produto);
                if (!result)
                {
                    _logger.LogWarning("Erro ao atualizar produto.");
                    throw new FailCreateUpdateException("Erro ao atualizar produto. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Produto com ID: {Id} atualizado com sucesso.", id);
                return Ok(produto);
            }
        }

        /// <summary>
        /// Efetua o hard ou soft delete de um produto.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, bool hardDelete = false)
        {
            return Ok(await _genericsDeleteService.DeleteAsync(_produtoRepository, _logger, _context, id, hardDelete));
        }

        /// <summary>
        /// Atualiza o estoque de um produto.
        /// </summary>
        [HttpPatch("inventory-control")]
        public async Task<IActionResult> Patch (int id, int estoqueIncremento)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _logger.LogInformation("Atualizando estoque do produto com ID: {Id}", id);
                var produto = await _produtoRepository.GetByIdAsync(id);
                if (produto == null)
                {
                    _logger.LogWarning("Produto com ID: {Id} não encontrado.", id);
                    throw new NotFoundException("Produto não encontrado. Experimente buscar por outro ID!");
                }

                produto.Quantidade += estoqueIncremento;
                var result = await _produtoRepository.UpdateAsync(produto);
                if (!result)
                {
                    _logger.LogWarning("Falha ao atualizar o estoque do produto.");
                    throw new FailCreateUpdateException("Falha ao atualizar o estoque do produto. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Estoque do produto com ID: {Id} atualizado com sucesso.", id);
                return Ok(result);
            }
        }

        /// <summary>
        /// Ativa ou inativa um produto.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> Patch(int id, bool ativo)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _logger.LogInformation("Atualizando status do produto com ID: {Id}", id);
                var produto = await _produtoRepository.GetByIdActivateAsync(id);
                if (produto == null)
                {
                    _logger.LogWarning("Produto com ID: {Id} não encontrado.", id);
                    throw new NotFoundException("Produto não encontrado. Experimente buscar por outro ID!");
                }

                if (produto.Ativo == ativo)
                {
                    _logger.LogWarning("Status do cliente já está como: {Ativo}", ativo);
                    throw new FailCreateUpdateException("Status do cliente já está como desejado.");
                }

                produto.Ativo = ativo;
                var result = await _produtoRepository.UpdateAsync(produto);
                if (!result)
                {
                    _logger.LogWarning("Falha ao atualizar o status do produto.");
                    throw new FailCreateUpdateException("Falha ao atualizar o status do produto. Tente novamente!");
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Status do produto com ID: {Id} atualizado com sucesso.", id);
                return Ok(result);
            }
        }
    }
}
