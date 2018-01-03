using MyUtil;
using System;
using BattleshipGame.Core;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    public class GameApi
    {
        private IRepository<Game> _repo;
        private IAuthorizer _authorizer;
        private ILogger _logger;

        public GameApi(IRepository<Game> repo, IAuthorizer authorizer, ILogger logger)
        {
            _repo = repo;
            _authorizer = authorizer;
            _logger = logger;
        }

        public Guid Create(string player1Id, string player2Id, string userId)
        {
            _authorizer.AssertAuthorizedForCreate(userId);

            var game = new Game(player1Id, player2Id);

            _repo.Add(game);

            return game.Id;
        }
 
        public Game Read(Guid gameId, string userId)
        {
            _authorizer.AssertAuthorizedForRead(userId, gameId);

            return _repo.Get(gameId);
        }

        public void PlaceShip(Guid gameId, string userId, Location shipStart, Location shipEnd)
        {
            _authorizer.AssertAuthorizedForPlaceShip(userId, gameId);

            var game = _repo.Get(gameId);

            // place ship logic

            _logger.Info($"Player {game.GetPlayerNumber(userId)} placed a ship at {shipStart.ToString()}-{shipEnd.ToString()}");

            _repo.Update(gameId, game);
        }

        public void Attack(Guid gameId, string userId, Location target)
        {
            _authorizer.AssertAuthorizedForAttack(userId, gameId);

            var game = _repo.Get(gameId);

            // attack logic

            _logger.Info($"Player {game.GetPlayerNumber(userId)} attacked {target.ToString()}");

            _repo.Update(gameId, game);
        }
    }
}
