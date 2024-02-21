using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class DevolucaoTrocaRepository: IRepository<DevolucaoTroca>
    {
        private readonly DataContext _context;

        public DevolucaoTrocaRepository(DataContext context)
        {
            _context = context;
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
            return await _context.DevolucoesTrocas.FindAsync(id);
        }

        public async Task<IEnumerable<DevolucaoTroca>> GetAllAsync()
        {
            return _context.DevolucoesTrocas;
        }

        public async Task<bool> UpdateAsync(DevolucaoTroca entity)
        {
            _context.DevolucoesTrocas.Update(entity);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
