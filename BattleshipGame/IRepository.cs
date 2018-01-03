using System;

namespace BattleshipGame
{
    public interface IRepository<T>
    {
        T Get(Guid id);

        Guid Add(T newObject);

        void Update(Guid id, T updatedObject);
    }
}