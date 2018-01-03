namespace BattleshipGame.Text
{
    internal class Messages
    {
        public const string DiagonalPlacementsAreNotAllowed = "Diagonals are not allowed.";
  
        public static string InvalidLocation(string badFormat)
        {
            return $"The location '{badFormat}' was not in a valid format (e.g. A5).";
        }

        public static string PlayerIsNotPartOfThisGame(string playerId)
        {
            return $"Player '{playerId}' is not part of this game";
        }

        public static string PlayerHasAlreadyPlacedShip(int playerNumber)
        {
            return $"Player {playerNumber} has already placed their ship";
        }

        public static string ShipIsNotThatBig(string loc1, string loc2, int distance, int allowedDistance)
        {
            return $"{loc1}-{loc2} is {distance} squares but the ship should occupy {allowedDistance} squares";
        }
    }
}
