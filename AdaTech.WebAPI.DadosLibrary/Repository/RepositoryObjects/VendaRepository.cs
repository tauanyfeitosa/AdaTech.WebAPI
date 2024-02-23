using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class VendaRepository: IRepository<Venda>
    {
        private readonly DataContext _context;

        public VendaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venda>> GetAsync()
        {
            return await _context.Vendas.ToListAsync();
        }

        public async Task<bool> AddAsync(Venda entity)
        {
            await _context.Vendas.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Vendas.FindAsync(id);
            if (entity != null)
            {
                _context.Vendas.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Venda> GetByIdAsync(int id)
        {
            return await _context.Vendas.FindAsync(id);
        }

        public async Task<IEnumerable<Venda>> GetAllAsync()
        {
            return await _context.Vendas
                .Where(venda => venda.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venda>> GetInactiveAsync()
        {
            var vendas = await _context.Vendas
                                       .Where(venda => !venda.Ativo)
                                       .ToListAsync();

            return vendas;
        }

        public async Task<bool> UpdateAsync(Venda entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Venda> GetByIdActivateAsync(int id)
        {
            var entity = await _context.Vendas.FindAsync(id);

            if (!entity.Ativo)
                return null;

            return entity;
        }
    }
}
