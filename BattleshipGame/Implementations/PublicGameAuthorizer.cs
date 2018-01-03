using BattleshipGame.Core;
using BattleshipGame.Text;
using System;
using System.Security;

namespace BattleshipGame
{
    /// <summary>
    /// Example of an authorization policy that allows anyone to create and read (i.e. view) games,
    /// but only the actual players are authorized to place ships and attack.  Of course, the game
    /// itself does additional validation so that only the correct player can make attacks, but 
    /// for consistency and for potentially cutting out earlier in the call chain we do authorization 
    /// on all "api" calls.
    /// </summary>
    public class PublicGameAuthorizer : IAuthorizer
    {
        private readonly IRepository<Game> _repo;

        public PublicGameAuthorizer(IRepository<Game> repo)
        {
            _repo = repo;
        }

        public void AssertAuthorizedForAttack(string userId, Guid gameId)
        {
            var game = _repo.Get(gameId);

            // only the players in this game can actually attack
            if (!string.Equals(userId, game.Player1) && !string.Equals(userId, game.Player2))
            {
                throw new SecurityException(Messages.UserIsNotAuthorized);
            }
        }

        public void AssertAuthorizedForCreate(string userId)
        {
            // anyone can create a game, we're not picky about that
        }

        public void AssertAuthorizedForPlaceShip(string userId, Guid gameId)
        {
            var game = _repo.Get(gameId);

            // only the players in this game can actually place ships
            if (!string.Equals(userId, game.Player1) && !string.Equals(userId, game.Player2))
            {
                throw new SecurityException(Messages.UserIsNotAuthorized);
            }
        }

        public void AssertAuthorizedForRead(string userId, Guid gameId)
        {
            // do nothing... anyone can spectate any game
            // alternative models could be only participants can view the games, or the game itself could have a flag to indicate
            // whether it is spectator friendly
        }
    }
}
