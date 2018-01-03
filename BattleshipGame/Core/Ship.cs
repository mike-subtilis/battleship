using System.Collections.Generic;

namespace BattleshipGame.Core
{
    public class Ship
    {
        internal Ship()
        {
            Parts = new List<ShipPart>();
            Status = ShipStatus.Available;
        }

        public ShipStatus Status { get; set; }

        public IList<ShipPart> Parts { get; private set; }

        internal void AddParts(Location start, Location end)
        {

        }
    }
}
