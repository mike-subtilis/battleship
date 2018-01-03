using System;
using System.Collections.Generic;

namespace BattleshipGame.Core
{
    public partial class Game
    {
        internal Game()
        {
            this.Id = new Guid();
            this.Player1AttackRecord = new List<AttackRecord>();
            this.Player2AttackRecord = new List<AttackRecord>();
            this.Player1Ship = new Ship();
            this.Player2Ship = new Ship();
            this.Status = GameStatus.Open;
        }

        internal Game(string player1, string player2) : this()
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.Status = GameStatus.BoardSetup;
        }

        #region Game State
        public Guid Id { get; private set; }

        public string Player1 { get; set; }

        public string Player2 { get; set; }

        public Ship Player1Ship { get; private set; }

        public Ship Player2Ship { get; private set; }

        public IList<AttackRecord> Player1AttackRecord { get; private set; }

        public IList<AttackRecord> Player2AttackRecord { get; private set; }
        #endregion

        #region Statistics & Info
        /// <summary>
        /// The status of the game
        /// </summary>
        public GameStatus Status { get; set; }

        /// <summary>
        /// The turn number (e.g. turn 5 starts after each player has made 4 attacks)
        /// </summary>
        public int CurrentTurnNumber { get; set; }

        /// <summary>
        /// The player (1 or 2) who's turn it is
        /// </summary>
        public int CurrentTurnPlayer { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastTurnAt { get; set; }
        
        public DateTime CompletedAt { get; set; }

        public string Winner { get; set; }
        #endregion
    }
}
