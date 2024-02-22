using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace AdaTech.WebAPI.SistemaVendas.Controllers
{
    [SwaggerDisplayName("CRUD - Produto")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IRepository<Produto> _produtoRepository;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(IRepository<Produto> produtoRepository, ILogger<ProdutoController> logger)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a lista de todos os produtos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Buscando todos os produtos.");
            var produtos = await _produtoRepository.GetAllAsync();

            if (produtos == null)
            {
                _logger.LogWarning("Nenhum produto encontrado.");
                throw new NotFoundException("Nenhum produto encontrado. Experimente cadastrar um novo produto!");
            }
            _logger.LogInformation("Produtos recuperados com sucesso.");
            return Ok(produtos);
        }

        /// <summary>
        /// Obtém um produto ao informar um Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Buscando produto com ID: {Id}", id);
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                _logger.LogWarning("Produto com ID: {Id} não encontrado.", id);
                throw new NotFoundException("Produto não encontrado. Experimente buscar por outro ID!");
            }
            _logger.LogInformation("Produto com ID: {Id} recuperado com sucesso.", id);
            return Ok(produto);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] ProdutoRequest produtoRequest)
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

            _logger.LogInformation("Produto cadastrado com sucesso.");
            return Ok(produto);

        }

        /// <summary>
        /// Atualiza os campos de um produto.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoRequest produtoRequest)
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

            _logger.LogInformation("Produto com ID: {Id} atualizado com sucesso.", id);
            return Ok(produto);
        }

        /// <summary>
        /// Efetua o hard ou soft delete de um produto.
        /// </summary>
        [HttpDelete("delete")]
        public void Delete(int id, bool hardDelete = false)
        {
            _logger.LogInformation("Iniciando exclusão do produto com ID {Id}", id);
            var produto = _produtoRepository.GetByIdAsync(id).Result;
            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {Id} não encontrado para exclusão", id);
                throw new NotFoundException("Produto não encontrado para exclusão. Experimente buscar por outro ID!");
            }

            if (hardDelete)
            {
                _logger.LogInformation("Realizando hard delete para o produto com ID {Id}", id);
                _produtoRepository.DeleteAsync(produto.Id);
            }

            _logger.LogInformation("Realizando soft delete para o produto com ID {Id}", id);
            produto.Ativo = false;
            _produtoRepository.UpdateAsync(produto);

            _logger.LogInformation("Produto com ID {Id} excluído com sucesso. Hard Delete: {HardDelete}", id, hardDelete);
        }

        /// <summary>
        /// Atualiza o estoque de um produto.
        /// </summary>
        [HttpPatch("inventory-control")]
        public async Task<IActionResult> Patch (int id, int estoqueIncremento)
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

            _logger.LogInformation("Estoque do produto com ID: {Id} atualizado com sucesso.", id);
            return Ok(result);
        }

        /// <summary>
        /// Ativa ou inativa um produto.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> Patch(int id, bool ativo)
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

            _logger.LogInformation("Status do produto com ID: {Id} atualizado com sucesso.", id);
            return Ok(result);
        }
    }
}
