using MyUtil;
using System;
using BattleshipGame.Core;

namespace BattleshipGame
{
    public class GameApi
    {
        private readonly IGameFactory _factory;
        private readonly IRepository<Game> _repo;
        private readonly IAuthorizer _authorizer;
        private readonly ILogger _logger;

        public GameApi(IGameFactory factory, IRepository<Game> repo, IAuthorizer authorizer, ILogger logger)
        {
            _factory = factory;
            _repo = repo;
            _authorizer = authorizer;
            _logger = logger;
        }

        public Guid Create(string player1Id, string player2Id, string userId)
        {
            try
            {
                _authorizer.AssertAuthorizedForCreate(userId);

                var game = _factory.Create(player1Id, player2Id);

                var id = _repo.Add(game);

                _logger.Info($"Created a game between '{player1Id}' and '{player2Id}'.");

                return id;
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred creating a game", ex);
                throw;
            }
        }

        public Game Read(Guid gameId, string userId)
        {
            try
            {
                _authorizer.AssertAuthorizedForRead(userId, gameId);

                return _repo.Get(gameId);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred reading a game", ex);
                throw;
            }
        }

        public void PlaceShip(Guid gameId, string userId, Location shipStart, Location shipEnd)
        {
            try
            {
                _authorizer.AssertAuthorizedForPlaceShip(userId, gameId);

                var game = _repo.Get(gameId);

                game.PlaceShip(userId, shipStart, shipEnd);

                _logger.Info($"Player {game.GetPlayerNumber(userId)} placed a ship at {shipStart.ToString()}-{shipEnd.ToString()}");

                _repo.Update(gameId, game);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn($"An error occurred placing a ship: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred placing a ship", ex);
                throw;
            }
        }

        public void Attack(Guid gameId, string userId, Location target)
        {
            try
            {
                _authorizer.AssertAuthorizedForAttack(userId, gameId);

                var game = _repo.Get(gameId);

                game.Attack(userId, target);

                _logger.Info($"Player {game.GetPlayerNumber(userId)} attacked {target.ToString()}");

                _repo.Update(gameId, game);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn($"An error occurred attacking: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred attacking", ex);
                throw;
            }
        }
    }
}
