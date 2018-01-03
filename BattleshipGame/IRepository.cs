using System;

namespace BattleshipGame
{
    public interface IRepository<T>
    {
        T Get(Guid id);

        void Add(T newObject);

        void Update(Guid id, T updatedObject);
    }
}