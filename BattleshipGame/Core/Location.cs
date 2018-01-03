using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BattleshipGame.Core
{
    /// <summary>
    /// Represents a location on the board
    /// </summary>
    public class Location
    {
        public static Regex LocationCodeFormat = new Regex("^([A-H])([1-8])$");

        /// <summary>
        /// Converts a row index to a row code
        /// </summary>
        /// <param name="rowIndex">(e.g. 3)</param>
        /// <returns>(e.g. "4")</returns>
        public static string GetRowCodeFromRowIndex(int rowIndex)
        {
            return (rowIndex + 1).ToString();
        }

        /// <summary>
        /// Converts a column index to a column code
        /// </summary>
        /// <param name="colIndex">(e.g. 3)</param>
        /// <returns>(e.g. "D")</returns>
        public static string GetColCodeFromColIndex(int colIndex)
        {
            return ((char)(colIndex + (int)'A')).ToString();
        }

        /// <summary>
        /// Gets the zero-indexed location row from the location code
        /// </summary>
        /// <param name="locationCode">(e.g. "A5")</param>
        /// <returns>(e.g. 4)</returns>
        /// <exception cref="FormatException">If the location code does not match the allowed format</exception>
        public static int GetRowFromCode(string locationCode)
        {
            var match = LocationCodeFormat.Match(locationCode);
            if (!match.Success)
            {
                throw new FormatException(Text.Messages.InvalidLocation(locationCode));
            }

            var rowGroup = match.Groups[2];
            return int.Parse(rowGroup.Value) - 1; // -1 because we want 0-indexed
        }

        /// <summary>
        /// Gets the zero-indexed location column from the location code
        /// </summary>
        /// <param name="locationCode">(e.g. "A5")</param>
        /// <returns>(e.g. 0)</returns>
        /// <exception cref="FormatException">If the location code does not match the allowed format</exception>
        public static int GetColFromCode(string locationCode)
        {
            var match = LocationCodeFormat.Match(locationCode);
            if (!match.Success)
            {
                throw new FormatException(Text.Messages.InvalidLocation(locationCode));
            }

            var colGroup = match.Groups[1];
            return ((int)colGroup.Value[0]) - (int)'A';
        }

        /// <summary>
        /// Returns the length in squares between two locations (minimum 1).  Diagonals are not allowed.
        /// </summary>
        /// <param name="loc1">(e.g. "D5")</param>
        /// <param name="loc2">(e.g. "D1")</param>
        /// <returns>The length between the locations (e.g. 5)</returns>
        /// <exception cref="InvalidOperationException">Diagonal lengths are not allowed</exception>
        public static int GetLength(Location loc1, Location loc2)
        {
            if ((loc1.Row != loc2.Row) && (loc1.Col != loc2.Col))
            {
                throw new InvalidOperationException(Text.Messages.DiagonalPlacementsAreNotAllowed);
            }

            var rowLength = Math.Abs(loc1.Row - loc2.Row) + 1;
            var colLength = Math.Abs(loc1.Col - loc2.Col) + 1;

            return Math.Max(rowLength, colLength);
        }

        /// <summary>
        /// Gets a list of locations in between 2 locations (including both end points)
        /// </summary>
        /// <param name="loc1">(e.g. "D5")</param>
        /// <param name="loc2">(e.g. "D3")</param>
        /// <returns>The list of locations (e.g. ["D3", "D4", "D5"])</returns>
        /// <exception cref="InvalidOperationException">Diagonals are not allowed</exception>
        public static IEnumerable<Location> GetLocationsInBetween(Location loc1, Location loc2)
        {
            if ((loc1.Row != loc2.Row) && (loc1.Col != loc2.Col))
            {
                throw new InvalidOperationException(Text.Messages.DiagonalPlacementsAreNotAllowed);
            }

            var list = new List<Location>();
            if (loc1.Row == loc2.Row)
            {
                for (var i = Math.Min(loc1.Col, loc2.Col); i <= Math.Max(loc1.Col, loc2.Col); i++)
                {
                    list.Add(new Location { Row = loc1.Row, Col = i });
                }
            } else
            {
                for (var i = Math.Min(loc1.Row, loc2.Row); i <= Math.Max(loc1.Row, loc2.Row); i++)
                {
                    list.Add(new Location { Row = i, Col = loc1.Col });
                }
            }

            return list;
        }

        public Location()
        {
        }

        /// <summary>
        /// Creates a location from the location code
        /// </summary>
        /// <param name="locationCode">(e.g. "A5")</param>
        public Location(string locationCode)
        {
            Row = GetRowFromCode(locationCode);
            Col = GetColFromCode(locationCode);
        }

        /// <summary>
        /// 0-indexed internal numeric row index
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 0-indexed internal numeric column index
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Returns the location code format of this location
        /// </summary>
        /// <returns>(e.g. "A3")</returns>
        public override string ToString()
        {
            return $"{GetColCodeFromColIndex(Col)}{GetRowCodeFromRowIndex(Row)}";
        }
    }
}
