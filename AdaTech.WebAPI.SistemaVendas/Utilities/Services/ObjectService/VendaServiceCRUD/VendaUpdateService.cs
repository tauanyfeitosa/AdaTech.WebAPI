using AdaTech.WebAPI.DadosLibrary.DTO.Enums;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects;
using AdaTech.WebAPI.SistemaVendas.Utilities.DTO;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.VendaServiceCRUD
{
    public class VendaUpdateService
    {

        private readonly IRepository<Venda> _vendaRepository;
        private readonly ClienteService _clienteService;
        private readonly IRepository<ItemVenda> _itemVendaRepository;

        public VendaUpdateService(IRepository<Venda> vendaRepository, ClienteService clienteService, IRepository<ItemVenda> repository)
        {
            _vendaRepository = vendaRepository;
            _clienteService = clienteService;
            _itemVendaRepository = repository;
        }

        public async void UpdateVenda(VendaDTO vendaDTO, ILogger logger, int id)
        {
            var venda = await VerifyVendaExistsAndStatus(id, false, logger);
            var cliente = await _clienteService.GetCPFAsync(vendaDTO.ClienteCPF);
            venda.ClienteId = cliente.Id;
            venda.DataVenda = vendaDTO.DataVenda;
            venda.StatusVenda = vendaDTO.StatusVenda;

            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                logger.LogError("Falha ao atualizar a venda.");
                throw new FailCreateUpdateException("Falha ao atualizar a venda. Tente novamente!");
            }
        }

        public async void UpdateStatus (int id, StatusVenda statusVenda, ILogger logger)
        {
            var venda = await VerifyVendaExistsAndStatus(id, false, logger);

            venda.StatusVenda = statusVenda;
            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                logger.LogError("Falha ao atualizar o status da venda.");
                throw new FailCreateUpdateException("Falha ao atualizar o status da venda. Tente novamente!");
            }
        }

        public async Task UpdateActivate(int id, bool ativo, ILogger logger)
        {
            var venda = await VerifyVendaExistsAndStatus(id, ativo, logger);

            if (!ativo)
            {
                await UpdateItensVendaStatus(id, ativo, logger);
            }

            await UpdateVendaStatus(venda, ativo, logger);
        }


        private async Task<Venda> VerifyVendaExistsAndStatus(int id, bool ativo, ILogger logger)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null || venda.Ativo == ativo)
            {
                logger.LogWarning("Venda com ID: {Id} não encontrada.", id);
                throw new NotFoundException("Venda não encontrada. Experimente buscar por outro ID!");
            }

            return venda;
        }

        private async Task UpdateItensVendaStatus(int vendaId, bool ativo, ILogger logger)
        {
            var todosItensVenda = await _itemVendaRepository.GetAsync();
            var itensFiltrados = todosItensVenda.Where(item => item.VendaId == vendaId).ToList();

            foreach (var item in itensFiltrados)
            {
                item.Ativo = ativo;
                var result = await _itemVendaRepository.UpdateAsync(item);
                if (!result)
                {
                    logger.LogError("Falha ao atualizar o status do item de venda com ID: {ItemId}.", item.Id);
                    throw new FailCreateUpdateException("Falha ao atualizar o status do item de venda. Tente novamente!");
                }
            }
        }

        private async Task UpdateVendaStatus(Venda venda, bool ativo, ILogger logger)
        {
            venda.Ativo = ativo;
            var success = await _vendaRepository.UpdateAsync(venda);

            if (!success)
            {
                logger.LogError("Falha ao atualizar o status da venda com ID: {Id}.", venda.Id);
                throw new FailCreateUpdateException("Falha ao atualizar o status da venda. Tente novamente!");
            }
        }


    }
}
