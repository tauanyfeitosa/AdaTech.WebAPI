using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class ProdutoRepository: IRepository<Produto>
    {
        private readonly DataContext _context;

        public ProdutoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Produto entity)
        {
            await _context.Produtos.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Produtos.FindAsync(id);
            if (entity != null)
            {
                _context.Produtos.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return _context.Produtos;
        }

        public async Task<bool> UpdateAsync(Produto entity)
        {
            _context.Produtos.Update(entity);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
