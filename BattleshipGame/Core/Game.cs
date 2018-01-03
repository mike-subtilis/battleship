using System;
using System.Collections.Generic;

namespace BattleshipGame.Core
{
    public partial class Game
    {
        public const int ColumnCount = 8; // note that the Location class needs to be updated separately if this changes
        public const int RowCount = 8; // note that the Location class needs to be updated separately if this changes

        internal Game()
        {
            this.AttackRecords = new IList<AttackRecord>[2];
            this.AttackRecords[0] = new List<AttackRecord>();
            this.AttackRecords[1] = new List<AttackRecord>();
            this.Ships = new Ship[2];
            this.Ships[0] = new Ship();
            this.Ships[1] = new Ship();
            this.Status = GameStatus.Open;
        }

        internal Game(string player1, string player2) : this()
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.Status = GameStatus.BoardSetup;
        }

        #region Game State
        public string Player1 { get; set; }

        public string Player2 { get; set; }

        public Ship[] Ships { get; private set; }

        public IList<AttackRecord>[] AttackRecords { get; private set; }
        #endregion

        #region Acccess Properties
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

        public string Winner { get; set; }
        #endregion
    }
}
