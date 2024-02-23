﻿using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService
{
    public class GenericsGetService<T> where T : class
    {
        public async Task<IEnumerable<T>> GetAllAsync(IRepository<T> repository, ILogger logger)
        {
            var entities = await repository.GetAllAsync();
            return Get(entities, logger);
        }

        public async Task<IEnumerable<T>> GetInactiveAsync(IRepository<T> repository, ILogger logger)
        {
            var entities = await repository.GetInactiveAsync();
            return Get(entities, logger);
        }

        private IEnumerable<T> Get(IEnumerable<T> entities, ILogger logger)
        {
            if (entities == null || !entities.Any())
            {
                logger.LogWarning($"Nenhum(a) {typeof(T).Name} encontrado(a).");
                throw new NotFoundException($"Nenhum(a) {typeof(T).Name} encontrado(a). Experimente cadastrar um(a) novo(a) {typeof(T).Name}!");
            }
            return entities;
        }

        public async Task<T> GetByIdAsync(IRepository<T> repository, ILogger logger, int id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                logger.LogWarning($"{typeof(T).Name} com ID: {id} não encontrado(a).");
                throw new NotFoundException($"{typeof(T).Name} não encontrado(a). Experimente buscar por outro ID!");
            }
            return entity;
        }
    }
}
