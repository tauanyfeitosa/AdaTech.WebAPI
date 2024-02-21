using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly DataContext _context;

        public ClienteRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<bool> AddAsync(Cliente entity)
        {
            var existingCliente = await _context.Clientes
                .AnyAsync(c => c.CPF == entity.CPF);

            if (!existingCliente)
            {
                _context.Clientes.Add(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Cliente entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Clientes.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Clientes.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
