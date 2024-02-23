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
    public class ProdutoRepository: IRepository<Produto>
    {
        private readonly DataContext _context;

        public ProdutoRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Produto>> GetAsync()
        {
            return await _context.Produtos.ToListAsync();
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
            var entity = await _context.Produtos.FindAsync(id);

            if (!entity.Ativo)
                return null;

            return entity;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos
                .Where(produto => produto.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetInactiveAsync()
        {
            var produtos = await _context.Produtos
                .Where(produto => !produto.Ativo)
                .ToListAsync();

            return produtos;
        }

        public async Task<bool> UpdateAsync(Produto entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Produto> GetByIdActivateAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }
    }
}
