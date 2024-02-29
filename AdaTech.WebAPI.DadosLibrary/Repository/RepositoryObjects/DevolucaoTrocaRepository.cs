using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.Entities.Entity.Objects;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class DevolucaoTrocaRepository : IRepository<DevolucaoTroca>
    {
        private readonly DataContext _context;

        public DevolucaoTrocaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DevolucaoTroca>> GetAsync()
        {
            return await _context.DevolucoesTrocas.ToListAsync();
        }

        public async Task<bool> AddAsync(DevolucaoTroca entity)
        {
            await _context.DevolucoesTrocas.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.DevolucoesTrocas.FindAsync(id);
            if (entity != null)
            {
                _context.DevolucoesTrocas.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<DevolucaoTroca> GetByIdAsync(int id)
        {
            var entity = await _context.DevolucoesTrocas.FindAsync(id);

            if (entity == null || !entity.Ativo)
                return null;

            return entity;
        }

        public async Task<IEnumerable<DevolucaoTroca>> GetAllAsync()
        {
            return await _context.DevolucoesTrocas
                .Where(devolucaoTroca => devolucaoTroca.Ativo)
                .ToListAsync();

        }

        public async Task<IEnumerable<DevolucaoTroca>> GetInactiveAsync()
        {
            var devolucoesTrocas = await _context.DevolucoesTrocas
                .Where(devolucaoTroca => !devolucaoTroca.Ativo)
                .ToListAsync();

            return devolucoesTrocas;
        }

        public async Task<bool> UpdateAsync(DevolucaoTroca entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<DevolucaoTroca> GetByIdActivateAsync(int id)
        {
            return await _context.DevolucoesTrocas.FindAsync(id);
        }
    }
}
