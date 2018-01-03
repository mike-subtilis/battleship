namespace BattleshipGame.Core
{
    public enum GameStatus
    {
        Open, // game is created, but needs players
        BoardSetup, // game has players assigned, and is waiting for the board to be set up
        InProgress, // game is in progress
        Completed // game is completed
    }
}
