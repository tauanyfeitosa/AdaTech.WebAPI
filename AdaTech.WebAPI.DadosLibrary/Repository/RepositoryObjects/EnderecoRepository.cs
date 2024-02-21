using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class EnderecoRepository: IRepository<Endereco>
    {
        private readonly DataContext _context;

        public EnderecoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Endereco entity)
        {
            await _context.Enderecos.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Enderecos.FindAsync(id);
            if (entity != null)
            {
                _context.Enderecos.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Endereco> GetByIdAsync(int id)
        {
            return await _context.Enderecos.FindAsync(id);
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync()
        {
            return _context.Enderecos;
        }

        public async Task<bool> UpdateAsync(Endereco entity)
        {
            _context.Enderecos.Update(entity);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
