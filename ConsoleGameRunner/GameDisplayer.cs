using BattleshipGame.Core;
using System;
using System.Linq;
using System.Text;

namespace ConsoleGameRunner
{
    public static class GameDisplayer
    {
        private const int RowHeaderIndentWidth = 2;
        private const int ColumnWidth = 2;
        private const int PlayerSeparatorWidth = 3;
        private const int SinglePlayerBoardWidth = RowHeaderIndentWidth + Game.ColumnCount * ColumnWidth;

        private static string DisplayPlayerNames(Game game)
        {
            var sb = new StringBuilder();

            sb.Append(game.Player1 + new String(' ', SinglePlayerBoardWidth - game.Player1.Length));
            sb.Append(new String(' ', PlayerSeparatorWidth));
            sb.Append(game.Player2 + new String(' ', SinglePlayerBoardWidth - game.Player2.Length));

            return sb.ToString();
        }

        public static string DisplayGameStatus(Game game)
        {
            var sb = new StringBuilder();

            switch (game.Status)
            {
                case GameStatus.Open:
                    sb.Append("Waiting for players...");
                    break;
                case GameStatus.BoardSetup:
                    if (game.Ships[0].Status == ShipStatus.Available)
                    {
                        sb.Append($"Waiting for {game.Player1} to place their ship");
                    }
                    else if (game.Ships[1].Status == ShipStatus.Available)
                    {
                        sb.Append($"Waiting for {game.Player2} to place their ship");
                    }
                    break;
                case GameStatus.InProgress:
                    sb.Append($"Turn #{game.CurrentTurnNumber}: waiting for '{(game.CurrentTurnPlayer == 1 ? game.Player1 : game.Player2)}'");
                    break;
                case GameStatus.Completed:
                    sb.AppendLine();
                    sb.AppendLine(new String('*', game.Winner.Length + 9));
                    sb.AppendLine(new String(' ', 2) + game.Winner + " WINS");
                    sb.AppendLine(new String('*', game.Winner.Length + 9));
                    sb.AppendLine();
                    sb.Append($"Game over: {game.Winner} is the winner!");
                    break;
            }

            return sb.ToString();
        }

        private static string DisplayBoardHeaderRow(Game game)
        {
            var sb = new StringBuilder();

            sb.Append(new String(' ', RowHeaderIndentWidth));
            for (var i = 0; i < Game.ColumnCount; i++)
            {
                var columnLabel = Location.GetColCodeFromColIndex(i);
                sb.Append(columnLabel + new string(' ', ColumnWidth - columnLabel.Length));
            }

            return sb.ToString();
        }

        private static string DisplayBoardRow(Game game, int playerIndex, int rowIndex)
        {
            var sb = new StringBuilder();

            var rowLabel = Location.GetRowCodeFromRowIndex(rowIndex);
            sb.Append(rowLabel + new string(' ', RowHeaderIndentWidth - rowLabel.Length));

            var displayShipPlayerIndex = playerIndex;
            var displayAttackPlayerIndex = playerIndex == 0 ? 1 : 0;

            for (var i = 0; i < Game.ColumnCount; i++)
            {
                var location = new Location { Row = rowIndex, Col = i };
                var columnContents = "-";
                var locationAttack = game.AttackRecords[displayAttackPlayerIndex].FirstOrDefault(ar => ar.Row == rowIndex && ar.Col == i);
                if (locationAttack != null)
                {
                    columnContents = locationAttack.IsHit ? "H" : "M";
                }
                else if (game.Ships[displayShipPlayerIndex].IsLocationPartOfShip(location))
                {
                    columnContents = "O";
                }

                sb.Append(columnContents + new string(' ', ColumnWidth - "-".Length));
            }

            return sb.ToString();
        }

        public static string Display(Game game)
        {
            var sb = new StringBuilder();

            sb.AppendLine(DisplayGameStatus(game));

            sb.AppendLine(DisplayPlayerNames(game));

            sb.Append(DisplayBoardHeaderRow(game));
            sb.Append(new String(' ', PlayerSeparatorWidth));
            sb.Append(DisplayBoardHeaderRow(game));
            sb.Append(System.Environment.NewLine);

            for (var i = 0; i < Game.RowCount; i++)
            {
                sb.Append(DisplayBoardRow(game, 0, i));
                sb.Append(new String(' ', PlayerSeparatorWidth));
                sb.Append(DisplayBoardRow(game, 1, i));
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
