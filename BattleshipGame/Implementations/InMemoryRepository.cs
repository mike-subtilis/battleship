using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    public class InMemoryRepository<T> : IRepository<T>
    {
        private readonly IDictionary<Guid, T> _storage = new Dictionary<Guid, T>();

        public Guid Add(T newObject)
        {
            var id = new Guid();
            _storage.Add(id, newObject);
            return id;
        }

        public T Get(Guid id)
        {
            return _storage[id];
        }

        public void Update(Guid id, T updatedObject)
        {
            _storage[id] = updatedObject;
        }
    }
}
