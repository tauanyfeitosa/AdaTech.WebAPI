using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Endereco>> GetAsync()
        {
            return await _context.Enderecos.ToListAsync();
        }

        public async Task<bool> AddAsync(Endereco endereco)
        {
            var existingEndereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == endereco.Id);

            if (existingEndereco == null)
            {
                await _context.Enderecos.AddAsync(endereco);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }

            return false;
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
            var entity = await _context.Enderecos.FindAsync(id);

            if (!entity.Ativo)
                return null;

            return entity;
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync()
        {
            return await _context.Enderecos
                .Where(endereco => endereco.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Endereco>> GetInactiveAsync()
        {
            var enderecos = await _context.Enderecos
                .Where(endereco => !endereco.Ativo)
                .ToListAsync();

            return enderecos;
        }

        public async Task<bool> UpdateAsync(Endereco entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Endereco> GetByCEPAsync(string cep)
        {
            var entity =  await _context.Enderecos.FirstOrDefaultAsync(e => e.CEP == cep);

            if (entity != null)
            {
                return entity;
            }

            return null;
        }

        public async Task<Endereco> GetByIdActivateAsync(int id)
        {
            return await _context.Enderecos.FindAsync(id);
        }
    }
}
