using AdaTech.WebAPI.DadosLibrary.DTO.Enums;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO.ModelRequest;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services;
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
        private readonly IRepository<Produto> _produtoRepository;
        private readonly ClienteService _clienteService;

        public VendaController(IRepository<Venda> vendaRepository, ILogger<VendaController> logger, IRepository<Produto> produtoRepository, ClienteService clienteService)
        {
            _vendaRepository = vendaRepository;
            _logger = logger;
            _produtoRepository = produtoRepository;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtêm todas as vendas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendas = await _vendaRepository.GetAllAsync();
            if (vendas == null)
            {
                _logger.LogWarning("Nenhuma venda encontrada.");
                throw new NotFoundException("Nenhuma venda encontrada. Experimente cadastrar uma nova venda!");
            }
            return Ok(vendas);
        }

        /// <summary>
        /// Obtêm uma venda por Id.
        /// </summary>
        [HttpGet("byId")]
        public async Task<IActionResult> Get(int id)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }
            return Ok(venda);
        }

        /// <summary>
        /// Cria uma nova venda e seus itens.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] VendaRequest vendaRequest)
        {
            List<ItemVenda> itensVenda = new List<ItemVenda>();
            _logger.LogInformation("Inserindo nova venda.");
            var cliente = await _clienteService.GetCPFAsync(vendaRequest.ClienteCPF);

            var venda = new Venda
            {
                ClienteId = cliente.Id, 
                DataVenda = vendaRequest.DataVenda
            };

            foreach (var itemRequest in vendaRequest.ItensVendas)
            {
                if (itemRequest == null)
                {
                    _logger.LogWarning("Um item de venda recebido é nulo.");
                    throw new ErrorInputUserException("Um item de venda recebido é nulo. A venda terá que ser refeita.");
                }

                var produto = await _produtoRepository.GetByIdAsync(itemRequest.ProdutoId);

                if (produto == null)
                {
                    _logger.LogWarning("Produto com ID: {Id} não encontrado.", itemRequest.ProdutoId);
                    throw new NotFoundException("Alguns produtos não foram encontrados. A venda terá que ser refeita.");
                }

                if (produto.Quantidade < itemRequest.Quantidade)
                {
                    _logger.LogWarning("Produto com ID: {Id} não possui quantidade suficiente em estoque.", itemRequest.ProdutoId);
                    throw new NotFoundException("Alguns produtos não possuem quantidade suficiente em estoque. A venda terá que ser refeita.");
                }

                produto.Quantidade -= itemRequest.Quantidade;
                await _produtoRepository.UpdateAsync(produto);

                var itemVenda = new ItemVenda
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemRequest.Quantidade,
                    ValorTotal = produto.Preco * itemRequest.Quantidade
                };

                itensVenda.Add(itemVenda);
            }



            venda.ItensVendas = itensVenda;
            venda.ValorTotal = itensVenda.Sum(x => x.ValorTotal);
            var success = await _vendaRepository.AddAsync(venda);

            if (!success)
            {
                _logger.LogError("Falha ao criar a venda.");
                throw new FailCreateUpdateException("Falha ao criar a venda. Tente novamente!");
            }

            _logger.LogInformation("Venda criada com sucesso.");
            return Ok("Criado com sucesso!");
        }

        /// <summary>
        /// Atualiza uma venda.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Put(int id, [FromBody] VendaDTO vendaDTO)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }
            var cliente = await _clienteService.GetCPFAsync(vendaDTO.ClienteCPF);
            venda.ClienteId = cliente.Id;
            venda.DataVenda = vendaDTO.DataVenda;
            venda.StatusVenda = vendaDTO.StatusVenda;

            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                _logger.LogError("Falha ao atualizar a venda.");
                throw new FailCreateUpdateException("Falha ao atualizar a venda. Tente novamente!");
            }

            _logger.LogInformation("Venda atualizada com sucesso.");
            return Ok("Atualizado com sucesso!");
        }

        /// <summary>
        /// Deleta uma venda.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id, bool hardDelete = false)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }

            if (hardDelete)
            {
                _logger.LogInformation("Realizando hard delete para a venda com ID {Id}", id);
                await _vendaRepository.DeleteAsync(venda.Id);
            }
            else
            {
                _logger.LogInformation("Realizando soft delete para a venda com ID {Id}", id);
                venda.Ativo = false;
                await _vendaRepository.UpdateAsync(venda);
            }

            _logger.LogInformation("Venda deletada com sucesso.");
            return Ok("Deletado com sucesso!");
        }

        /// <summary>
        /// Atualiza o status de uma venda.
        /// </summary>
        [HttpPatch("updateStatus")]
        public async Task<IActionResult> Patch(int id, [FromBody] StatusVenda statusVenda)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }

            venda.StatusVenda = statusVenda;
            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                _logger.LogError("Falha ao atualizar o status da venda.");
                throw new FailCreateUpdateException("Falha ao atualizar o status da venda. Tente novamente!");
            }

            _logger.LogInformation("Status da venda atualizado com sucesso.");
            return Ok("Status da venda atualizado com sucesso!");
        }

        /// <summary>
        /// Ativa ou Inativa uma venda.
        /// </summary>
        [HttpPatch("activate")]
        public async Task<IActionResult> Patch(int id, bool ativo)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }
            if (venda.Ativo == ativo)
            {
                _logger.LogWarning("Venda com ID: {Id} já está com o status {Ativo}.", id, ativo);
                throw new ErrorInputUserException($"Venda com ID: {id} já está com o status {ativo}.");
            }

            venda.Ativo = ativo;
            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                _logger.LogError("Falha ao atualizar o status da venda.");
                throw new FailCreateUpdateException("Falha ao atualizar o status da venda. Tente novamente!");
            }

            _logger.LogInformation("Status da venda atualizado com sucesso.");
            return Ok("Status da venda atualizado com sucesso!");
        }
    }
}
