using BattleshipGame;
using BattleshipGame.Core;
using MyUtil;
using System;

namespace ConsoleGameRunner
{
    class Program
    {
        private const string GameCreatorUserId = "power-user";

        private static void CaptureShipLocation(GameApi gameApi, Guid gameId, string playerName)
        {
            var game = gameApi.Read(gameId, GameCreatorUserId);

            var playerIndex = playerName == game.Player1 ? 0 : 1;

            while (game.Ships[playerIndex].Status != ShipStatus.Active)
            {
                Console.WriteLine($"{playerName}: Enter your ship location (e.g. 'A3 A5')");
                var shipLocation = Console.ReadLine();
                var shipLocationTokens = shipLocation.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    gameApi.PlaceShip(gameId, playerName, new Location(shipLocationTokens[0]), new Location(shipLocationTokens[1]));

                    game = gameApi.Read(gameId, GameCreatorUserId);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"'{shipLocation}' is not in the expected format");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"A serious error occurred: {ex.Message}");
                }
            }
        }

        private static void CaptureAttack(GameApi gameApi, Guid gameId)
        {
            var game = gameApi.Read(gameId, GameCreatorUserId);

            var playerNumber = game.CurrentTurnPlayer;
            var playerName = playerNumber == 1 ? game.Player1 : game.Player2;
            var attackProceededWithoutException = false;

            while (!attackProceededWithoutException)
            {
                Console.WriteLine($"{playerName}: Enter a location to attack (e.g. 'D4')");
                var attackLocation = Console.ReadLine();

                try
                {
                    gameApi.Attack(gameId, playerName, new Location(attackLocation));

                    attackProceededWithoutException = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"'{attackLocation}' is not in the expected format");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"A serious error occurred: {ex.Message}");
                }
            }
        }

        static void Main(string[] args)
        {
            var repo = new InMemoryRepository<Game>();
            var logger = new NLogLogger("ConsoleGameRunner");

            // TODO: move to IOC Container if this gets more complicated
            var gameApi = new GameApi(new SimpleGameFactory(), repo, new PublicGameAuthorizer(repo), logger);

            Console.WriteLine("Welcome to Battleship");
            logger.Info("Welcome to Battleship");

            var player1Name = args.Length == 2 ? args[0] : "Player 1";
            var player2Name = args.Length == 2 ? args[1] : "Player 2";

            var gameId = gameApi.Create(player1Name, player2Name, GameCreatorUserId);

            CaptureShipLocation(gameApi, gameId, player1Name);
            CaptureShipLocation(gameApi, gameId, player2Name);

            var game = gameApi.Read(gameId, GameCreatorUserId);

            while (game.Status != GameStatus.Completed)
            {
                Console.WriteLine();
                Console.WriteLine(GameDisplayer.Display(game));
                Console.WriteLine();
                CaptureAttack(gameApi, gameId);
            }

            Console.WriteLine();
            Console.WriteLine(GameDisplayer.Display(game));

            Console.ReadLine();
        }
    }
}
