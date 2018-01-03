using BattleshipGame.Core;

namespace BattleshipGame
{
    public class SimpleGameFactory : IGameFactory
    {
        public Game Create(string player1Id, string player2Id)
        {
            return new Game(player1Id, player2Id);
        }
    }
}
