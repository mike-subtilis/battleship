using BattleshipGame.Text;
using System;
using System.Linq;

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

        /// <summary>
        /// Places a ship on the board
        /// </summary>
        /// <param name="playerId">The player that is placing a ship</param>
        /// <param name="shipStart">The location of the front of the ship</param>
        /// <param name="shipEnd">THe location of the back of the ship</param>
        public void PlaceShip(string playerId, Location shipStart, Location shipEnd)
        {
            AssertIsValidForPlacingAShip(playerId, shipStart, shipEnd);

            var playerNumber = GetPlayerNumber(playerId);
            this.Ships[playerNumber - 1].InitializeLocation(shipStart, shipEnd);
            this.Ships[playerNumber - 1].Status = ShipStatus.Active;

            UpdateGameStateAfterShipPlacement();
        }

        /// <summary>
        /// Attack a location on the board
        /// </summary>
        /// <param name="playerId">The player making the attack</param>
        /// <param name="attackLocation">The attack location</param>
        /// <returns>A record of the attack</returns>
        public AttackRecord Attack(string playerId, Location attackLocation)
        {
            AssertIsValidForAttacking(playerId, attackLocation);

            var playerNumber = GetPlayerNumber(playerId);
            var otherPlayerNumber = playerNumber == 1 ? 2 : 1;

            var attackResult = this.Ships[otherPlayerNumber - 1].Attack(attackLocation);

            var attackRecord = new AttackRecord(attackLocation.ToString(), attackResult);
            this.AttackRecords[playerNumber - 1].Add(attackRecord);

            UpdateGameStateAfterAttack();

            return attackRecord;
        }

        /// <summary>
        /// Ensures that the game state is valid for placing this ship
        /// </summary>
        /// <param name="playerId">The id of the player that is placing a ship</param>
        /// <param name="shipStart">The location of the front of the ship</param>
        /// <param name="shipEnd">The location of the back of the ship</param>
        private void AssertIsValidForPlacingAShip(string playerId, Location shipStart, Location shipEnd)
        {
            if (Status != GameStatus.BoardSetup)
            {
                throw new InvalidOperationException(Messages.CanOnlyPlaceShipsInBoardSetup);
            }

            var playerNumber = GetPlayerNumber(playerId);
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new InvalidOperationException(Messages.PlayerIsNotPartOfThisGame(playerId));
            }

            if (Ships[playerNumber - 1].Status != ShipStatus.Available) {
                throw new InvalidOperationException(Messages.PlayerHasAlreadyPlacedShip(playerNumber));
            }

            var shipLength = Location.GetLength(shipStart, shipEnd);
            if (shipLength != 3)
            {
                throw new InvalidOperationException(Messages.ShipIsNotThatBig(shipStart.ToString(), shipEnd.ToString(), shipLength, 3));
            }
        }

        private void UpdateGameStateAfterShipPlacement()
        {
            // if both players have placed ships, move the game into in progress
            if (Ships[0].Status == ShipStatus.Active && Ships[1].Status == ShipStatus.Active)
            {
                this.Status = GameStatus.InProgress;
                this.CurrentTurnNumber = 1;
                this.CurrentTurnPlayer = 1;
            }
        }

        private void AssertIsValidForAttacking(string playerId, Location attackLocation)
        {
            if (Status != GameStatus.InProgress)
            {
                throw new InvalidOperationException(Messages.CanOnlyAttackWhenGameIsInProgress);
            }

            var playerNumber = GetPlayerNumber(playerId);
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new InvalidOperationException(Messages.PlayerIsNotPartOfThisGame(playerId));
            }

            if (playerNumber != this.CurrentTurnPlayer)
            {
                throw new InvalidOperationException(Messages.ItIsNotThisPlayersTurn);
            }

            if (AttackRecords[playerNumber - 1].Select(ar => ar.ToString()).Contains(attackLocation.ToString()))
            {
                throw new InvalidOperationException(Messages.PlayerHasAlreadyAttackedThisLocation(playerNumber, attackLocation.ToString()));
            }
        }

        private void UpdateGameStateAfterAttack()
        {
            if (Ships[0].Status == ShipStatus.Sunk)
            {
                this.Winner = this.Player2;
                this.Status = GameStatus.Completed;
            }
            else if (Ships[1].Status == ShipStatus.Sunk)
            {
                this.Winner = this.Player1;
                this.Status = GameStatus.Completed;
            }
            else
            {
                if (AttackRecords[0].Count() > AttackRecords[1].Count)
                {
                    this.CurrentTurnPlayer = 2;
                }
                else
                {
                    this.CurrentTurnNumber += 1;
                    this.CurrentTurnPlayer = 1;
                }
            }
        }
    }
}
