using MobilitywaysCodeTest.DataService.Abstractions;
using MobilitywaysCodeTest.Entities;
using System.Collections.Concurrent;

namespace MobilitywaysCodeTest.DataService.InMemory
{
    public class InMemoryDataService<T> : IDataService<T> where T : Entity
    {
        private readonly ConcurrentBag<T> _dataStore;

        public InMemoryDataService() {
            _dataStore = new ConcurrentBag<T>();
        }

        public Task Create(T entity)
        {
            var entities = _dataStore.Select(e => e.Id);
            var id = entities.Any() ? entities.Max() + 1 : 1;
            entity.Id = id;
            _dataStore.Add(entity);
            return Task.CompletedTask;
        }

        public Task<T?> Read(int id)
        {
            var entity = _dataStore.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(entity);
        }

        public Task<List<T>> ReadMultiple()
        {
            var entities = _dataStore.ToList();
            return Task.FromResult(entities);
        }
    }
}