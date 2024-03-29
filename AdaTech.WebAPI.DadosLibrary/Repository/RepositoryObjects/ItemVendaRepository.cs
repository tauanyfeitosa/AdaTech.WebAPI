﻿using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.Entities.Entity.Objects;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects
{
    public class ItemVendaRepository : IRepository<ItemVenda>
    {
        private readonly DataContext _context;

        public ItemVendaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemVenda>> GetAsync()
        {
            return await _context.ItensVenda.ToListAsync();
        }

        public async Task<IEnumerable<ItemVenda>> GetAllAsync()
        {
            return await _context.ItensVenda
                .Where(itemVenda => itemVenda.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemVenda>> GetInactiveAsync()
        {
            var itensVenda = await _context.ItensVenda
                .Where(itemVenda => !itemVenda.Ativo)
                .ToListAsync();

            return itensVenda;
        }

        public async Task<ItemVenda> GetByIdAsync(int id)
        {
            var entity = await _context.ItensVenda.FindAsync(id);

            if (entity == null || !entity.Ativo)
                return null;

            return entity;
        }

        public async Task<bool> AddAsync(ItemVenda entity)
        {
            _context.ItensVenda.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ItemVenda entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ItensVenda.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.ItensVenda.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ItemVenda> GetByIdActivateAsync(int id)
        {
            return await _context.ItensVenda.FindAsync(id);
        }
    }
}
