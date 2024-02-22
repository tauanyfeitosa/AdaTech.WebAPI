using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class VendaRepository: IRepository<Venda>
    {
        private readonly DataContext _context;

        public VendaRepository(DataContext context)
        {
            _context = context;
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
            return _context.Vendas;
        }

        public async Task<bool> UpdateAsync(Venda entity)
        {
            _context.Vendas.Update(entity);
            await _context.SaveChangesAsync();
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
