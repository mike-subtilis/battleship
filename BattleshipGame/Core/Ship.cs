using System.Collections.Generic;
using System.Linq;

namespace BattleshipGame.Core
{
    public class Ship
    {
        /// <summary>
        /// Represents part of a ship that occupies a single location
        /// </summary>
        private class ShipPart : Location
        {
            public ShipPart(string locationCode) : base(locationCode)
            {
                IsHit = false;
            }

            public bool IsHit { get; set; }
        }

        private List<ShipPart> Parts { get; set; }

        internal Ship()
        {
            Parts = new List<ShipPart>();
            Status = ShipStatus.Available;
        }

        public ShipStatus Status { get; set; }

        public bool IsLocationPartOfShip(Location test)
        {
            return this.Parts.Any(p => p.Row == test.Row && p.Col == test.Col);
        }

        /// <summary>
        /// Initialize the location of this ship
        /// </summary>
        /// <param name="start">The location of the front of the ship</param>
        /// <param name="end">The location of the end of the ship</param>
        internal void InitializeLocation(Location start, Location end)
        {
            var locations = Location.GetLocationsInBetween(start, end);
            this.Parts.Clear();
            this.Parts.AddRange(locations.Select(loc => new ShipPart(loc.ToString())));
        }

        /// <summary>
        /// Attack this location on the ship
        /// </summary>
        /// <param name="attackLocation">The location to attack</param>
        /// <returns>True if it's a hit, false otherwise</returns>
        internal bool Attack(Location attackLocation)
        {
            var shipPartAtLocation = this.Parts.Find(p => p.Row == attackLocation.Row && p.Col == attackLocation.Col);

            if (shipPartAtLocation == null)
            {
                return false;
            }

            shipPartAtLocation.IsHit = true;

            if (this.Parts.All(p => p.IsHit))
            {
                this.Status = ShipStatus.Sunk;
            }

            return true;
        }
    }
}
