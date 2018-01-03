namespace BattleshipGame.Core
{
    public enum ShipStatus
    {
        Available, // ship hasn't been placed on the board yet
        Active, // ship is placed on the board and is still "alive"
        Sunk // ship is sunk
    }
}
