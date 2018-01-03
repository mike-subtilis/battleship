using BattleshipGame.Core;

namespace BattleshipGame
{
    public interface IGameFactory
    {
        Game Create(string player1Id, string player2Id);
    }
}
