using BattleshipGame.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Core
{
    public partial class Game
    {
        /// <summary>
        /// Gets the number of the player (1 or 2)
        /// </summary>
        /// <param name="playerId">The potential player Id</param>
        /// <returns>1 or 2 (or -1 for no match)</returns>
        public int GetPlayerNumber(string playerId)
        {
            if (string.Equals(playerId, this.Player1))
            {
                return 1;
            }

            if (string.Equals(playerId, this.Player2))
            {
                return 2;
            }

            return -1;
        }

        public void PlaceShip(string playerId, Location shipStart, Location shipEnd)
        {
            // check to see if this should be allowed given the game state
            var playerNumber = GetPlayerNumber(playerId);
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new InvalidOperationException(Messages.PlayerIsNotPartOfThisGame(playerId));
            }

            if ((playerNumber == 1 && Player1Ship.Status != ShipStatus.Available) ||
                (playerNumber == 2 && Player2Ship.Status != ShipStatus.Available))
            {
                throw new InvalidOperationException(Messages.PlayerHasAlreadyPlacedShip(playerNumber));
            }

            var shipLength = Location.GetLength(shipStart, shipEnd);
            if (shipLength != 3)
            {
                throw new InvalidOperationException(Messages.ShipIsNotThatBig(shipStart.ToString(), shipEnd.ToString(), shipLength, 3));
            }

            // place the ship
            if (playerNumber == 1)
            {
//                Player1Ship.
                Player1Ship.Status = ShipStatus.Active;
            }
            else
            {
                Player2Ship.Status = ShipStatus.Active;
            }

            // if both players have placed ships, move the game into in progress
            if (Player1Ship.Status == ShipStatus.Active && Player2Ship.Status == ShipStatus.Active)
            {
                Status = GameStatus.InProgress;
            }
        }
    }
}
