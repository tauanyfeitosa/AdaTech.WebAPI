using AdaTech.WebAPI.Aplicacoes.DTO;
using AdaTech.WebAPI.Aplicacoes.DTO.ModelRequest;
using AdaTech.WebAPI.Aplicacoes.Exceptions;
using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.ObjectService.VendaServiceCRUD
{
    public class VendaCreateService
    {
        private readonly IRepository<Venda> _vendaRepository;
        private readonly ClienteService _clienteService;
        private readonly IRepository<Produto> _produtoRepository;

        public VendaCreateService(IRepository<Venda> vendaRepository, ClienteService clienteService, IRepository<Produto> produtoRepository)
        {
            _vendaRepository = vendaRepository;
            _clienteService = clienteService;
            _produtoRepository = produtoRepository;
        }

        public async Task CreateVenda(VendaRequest vendaRequest, ILogger logger)
        {
            var cliente = await GetClienteAsync(vendaRequest.ClienteCPF, logger);


            var venda = new Venda
            {
                ClienteId = cliente.Id,
                DataVenda = vendaRequest.DataVenda
            };

            var itensVenda = await PrepareItensVendaAsync(vendaRequest, logger);

            venda.ItensVendas = itensVenda;
            venda.ValorTotal = itensVenda.Sum(x => x.ValorTotal);
            var success = await _vendaRepository.AddAsync(venda);

            if (!success)
            {
                logger.LogError("Falha ao criar a venda.");
                throw new FailCreateUpdateException("Falha ao criar a venda. Tente novamente!");
            }
        }

        private async Task<Cliente> GetClienteAsync(string clienteCPF, ILogger logger)
        {
            var cliente = await _clienteService.GetCPFAsync(clienteCPF);
            if (cliente == null)
            {
                logger.LogWarning("Cliente com CPF: {CPF} não encontrado.", clienteCPF);
                throw new NotFoundException("Cliente não encontrado. A venda não pode ser processada.");
            }
            return cliente;
        }

        private async Task<List<ItemVenda>> PrepareItensVendaAsync(VendaRequest vendaRequest, ILogger logger)
        {
            List<ItemVenda> itensVenda = new List<ItemVenda>();

            foreach (var itemRequest in vendaRequest.ItensVendas)
            {
                if (itemRequest == null)
                {
                    logger.LogWarning("Um item de venda recebido é nulo.");
                    throw new ErrorInputUserException("Um item de venda recebido é nulo. A venda terá que ser refeita.");
                }

                var produto = await ValidateProdutoAsync(itemRequest, logger);

                var itemVenda = new ItemVenda
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemRequest.Quantidade,
                    ValorTotal = produto.Preco * itemRequest.Quantidade
                };

                itensVenda.Add(itemVenda);
            }
            return itensVenda;
        }

        private async Task<Produto> ValidateProdutoAsync(ItemVendaDTO itemRequest, ILogger logger)
        {
            var produto = await _produtoRepository.GetByIdAsync(itemRequest.ProdutoId);

            if (produto == null)
            {
                logger.LogWarning("Produto com ID: {Id} não encontrado.", itemRequest.ProdutoId);
                throw new NotFoundException("Alguns produtos não foram encontrados. A venda terá que ser refeita.");
            }

            if (produto.Quantidade < itemRequest.Quantidade)
            {
                logger.LogWarning("Produto com ID: {Id} não possui quantidade suficiente em estoque.", itemRequest.ProdutoId);
                throw new NotFoundException("Alguns produtos não possuem quantidade suficiente em estoque. A venda terá que ser refeita.");
            }

            produto.Quantidade -= itemRequest.Quantidade;
            var result = await _produtoRepository.UpdateAsync(produto);
            if (!result)
            {
                logger.LogError("Falha ao atualizar a quantidade do produto com ID: {Id}.", itemRequest.ProdutoId);
                throw new FailCreateUpdateException("Falha ao atualizar a quantidade do produto. Tente novamente!");
            }

            return produto;
        }

    }
}
