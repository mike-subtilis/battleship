using System;

namespace BattleshipGame
{
    public interface IAuthorizer
    {
        void AssertAuthorizedForCreate(string userId);
        void AssertAuthorizedForRead(string userId, Guid gameId);
        void AssertAuthorizedForPlaceShip(string userId, Guid gameId);
        void AssertAuthorizedForAttack(string userId, Guid gameId);
    }
}