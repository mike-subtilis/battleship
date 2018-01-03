using BattleshipGame.Core;

namespace BattleshipGameTest.Mocks
{
    public static class MockGame
    {
        public static Game ThatIsAcceptingPlayers()
        {
            var game = new Game();

            return game;
        }

        public static Game ThatIsWaitingForBothPlayersToPlaceAShip()
        {
            var game = new Game("myPlayer1", "myPlayer2");

            return game;
        }

        public static Game ThatIsWaitingForPlayer2ToPlaceAShip()
        {
            var game = new Game("myPlayer1", "myPlayer2");
            game.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));

            return game;
        }

        public static Game ThatIsOnTheFirstTurn()
        {
            var game = new Game("myPlayer1", "myPlayer2");
            game.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
            game.PlaceShip("myPlayer2", new Location("B4"), new Location("B6"));

            return game;
        }

        public static Game ThatIsNearlyDone()
        {
            var game = new Game("myPlayer1", "myPlayer2");
            game.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
            game.PlaceShip("myPlayer2", new Location("B4"), new Location("B6"));
            game.Attack("myPlayer1", new Location("H6"));
            game.Attack("myPlayer2", new Location("H2"));
            game.Attack("myPlayer1", new Location("H1"));
            game.Attack("myPlayer2", new Location("B3")); // hit
            game.Attack("myPlayer1", new Location("B4")); // hit
            game.Attack("myPlayer2", new Location("C3")); // hit
            game.Attack("myPlayer1", new Location("B5")); // hit

            return game;
        }

        public static Game ThatIsDone()
        {
            var game = new Game("myPlayer1", "myPlayer2");
            game.PlaceShip("myPlayer1", new Location("A3"), new Location("C3"));
            game.PlaceShip("myPlayer2", new Location("B4"), new Location("B6"));
            game.Attack("myPlayer1", new Location("H6"));
            game.Attack("myPlayer2", new Location("H2"));
            game.Attack("myPlayer1", new Location("H1"));
            game.Attack("myPlayer2", new Location("B3")); // hit
            game.Attack("myPlayer1", new Location("B4")); // hit
            game.Attack("myPlayer2", new Location("C3")); // hit
            game.Attack("myPlayer1", new Location("B5")); // hit
            game.Attack("myPlayer2", new Location("A3")); // hit

            return game;
        }
    }
}
